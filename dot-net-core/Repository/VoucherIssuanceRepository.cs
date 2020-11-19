using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository
{
    public class VoucherIssuanceRepository : IVoucherIssuanceRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public VoucherIssuanceRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function for get voucher issuance by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public VoucherIssuance_VM Get(int id, int loginUserId, bool isAdmin)
        {
            var voucherSetups = (from c in _context.VoucherIssuance
                                 where c.IsDeleted == false && c.Id == id
                                 select new VoucherIssuance_VM
                                 {
                                     Id = c.Id,
                                     CustomerId = c.CustomerId,
                                     VoucherId = c.VoucherId,
                                     ReasonCategoryId = c.ReasonCategoryId,
                                     ReasonSubCategoryId = c.ReasonSubCategoryId,
                                     Source = c.Source,
                                     UniqueId = c.UniqueId,
                                     CampaignText = c.CampaignText,
                                 }).FirstOrDefault();
            return voucherSetups;
        }

        /// <summary>
        /// Function for get voucher issuance list
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public List<VoucherIssuance_VM> GetList(int loginUserId, bool isAdmin)
        {
            var voucherIssuance = (from vi in _context.VoucherIssuance
                                   join vs in _context.VoucherSetup.Where(x => x.IsDeleted != true && x.Status) on vi.VoucherId equals vs.Id

                                   join c in _context.Set_Customers on vi.CustomerId equals c.Id into gc
                                   from c in gc.DefaultIfEmpty()

                                   join category in _context.VoucherReasonCategoryMaster on vi.ReasonCategoryId equals category.Id into gcategory
                                   from category in gcategory.DefaultIfEmpty()

                                   join subcategory in _context.VoucherReasonSubCategoryMaster on vi.ReasonSubCategoryId equals subcategory.Id into gsubcategory
                                   from subcategory in gsubcategory.DefaultIfEmpty()

                                   where vi.IsDeleted == false
                                   select new VoucherIssuance_VM
                                   {
                                       Id = vi.Id,
                                       CustomerId = vi.CustomerId,
                                       ReasonCategoryId = vi.ReasonCategoryId,
                                       ReasonSubCategoryId = vi.ReasonSubCategoryId,
                                       Source = vi.Source,
                                       UniqueId = vi.UniqueId,
                                       CampaignText = vi.CampaignText,
                                       VoucherName = vs.Name,
                                       CustomerName = c != null ? c.FirstName + " " + c.LastName : string.Empty,
                                       CategoryName = category != null ? category.Name : string.Empty,
                                       SubCategoryName = subcategory != null ? subcategory.Name : string.Empty,
                                       StoreIds = _context.Set_VoucherStore.Where(x => x.VoucherId == vs.Id).Select(x => x.StoreId).ToList(),
                                       ApprovedStatus = vi.ApprovedStatus != null ? vi.ApprovedStatus : "Default"
                                   }).ToList();


            var StoreIds = _context.Sec_UserStores.Where(c => c.UserId == loginUserId).Select(c => c.StoreId).ToList();

            // check stores permission
            voucherIssuance = voucherIssuance.Where(x => x.StoreIds.Any(x => StoreIds.Any(c2 => c2 == x))).ToList();
            return voucherIssuance;
        }

        /// <summary>
        /// Function for add voucher issuance
        /// </summary>
        /// <param name="voucherIssuance_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(VoucherIssuance_VM voucherIssuance_VM, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var voucherIssuance = new VoucherIssuance();
                    voucherIssuance.VoucherId = voucherIssuance_VM.VoucherId;
                    voucherIssuance.CustomerId = voucherIssuance_VM.CustomerId;
                    voucherIssuance.ReasonCategoryId = voucherIssuance_VM.ReasonCategoryId;
                    voucherIssuance.ReasonSubCategoryId = voucherIssuance_VM.ReasonSubCategoryId;
                    voucherIssuance.Source = voucherIssuance_VM.Source;
                    voucherIssuance.UniqueId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
                    voucherIssuance.CampaignText = voucherIssuance_VM.Source == (int)VoucherIssuanceSource.Campaign ? voucherIssuance_VM.CampaignText : string.Empty;
                    voucherIssuance.CreatedOn = DateTime.Now;
                    voucherIssuance.CreatedBy = loginUserId;
                    _context.VoucherIssuance.Add(voucherIssuance);
                    _context.SaveChanges();
                    dbcxtransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbcxtransaction.Rollback();
                    ret = 0;
                }
            }
            return ret;
        }

        /// <summary>
        /// Function for update voucher issuance by id
        /// </summary>
        /// <param name="voucherIssuance_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(VoucherIssuance_VM voucherIssuance_VM, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var voucherIssuance = _context.VoucherIssuance.Where(x => x.Id == voucherIssuance_VM.Id && x.IsDeleted != true).FirstOrDefault();
                    if (voucherIssuance != null)
                    {
                        voucherIssuance.VoucherId = voucherIssuance_VM.VoucherId;
                        voucherIssuance.CustomerId = voucherIssuance_VM.CustomerId;
                        voucherIssuance.ReasonCategoryId = voucherIssuance_VM.ReasonCategoryId;
                        voucherIssuance.ReasonSubCategoryId = voucherIssuance_VM.ReasonSubCategoryId;
                        voucherIssuance.Source = voucherIssuance_VM.Source;
                        voucherIssuance.CampaignText = voucherIssuance_VM.Source == (int)VoucherIssuanceSource.Campaign ? voucherIssuance_VM.CampaignText : string.Empty;
                        voucherIssuance.ModifiedOn = DateTime.Now;
                        voucherIssuance.ModifiedBy = loginUserId;
                        _context.SaveChanges();
                        dbcxtransaction.Commit();
                    }
                    else
                    {
                        ret = 0;
                    }
                }
                catch (Exception ex)
                {
                    dbcxtransaction.Rollback();
                    ret = 0;
                }
            }
            return ret;
        }

        /// <summary>
        /// Function for delete voucher issuance
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var voucher = _context.VoucherIssuance.Where(x => x.Id == id && x.IsDeleted == false && x.CreatedBy == loginUserId).FirstOrDefault();
            if (voucher != null)
            {
                voucher.IsDeleted = true;
                voucher.DeletedOn = DateTime.Now;
                voucher.DeletedBy = loginUserId;
                return _context.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// Function for update voucher approve status
        /// </summary>
        /// <param name="voucherIssuance_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int SetApproveStatus(VoucherIssuance_VM voucherIssuance_VM, int loginUserId)
        {
            var voucherIssuance = _context.VoucherIssuance.Where(x => x.Id == voucherIssuance_VM.Id && x.IsDeleted != true).FirstOrDefault();
            if (voucherIssuance != null)
            {
                voucherIssuance.ReasonDescription = voucherIssuance_VM.ReasonDescription;
                voucherIssuance.ApprovedStatus = voucherIssuance_VM.IsApproved.Value ? Helper.Approved : Helper.Reject;
                voucherIssuance.ModifiedOn = DateTime.Now;
                voucherIssuance.ModifiedBy = loginUserId;
                return _context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Function for Get Voucher Approve Status
        /// </summary>
        /// <param name="voucherIssuance_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public VoucherIssuance_VM GetVoucherApproveStatus(int id)
        {
            var voucherSetups = (from c in _context.VoucherIssuance
                                 where c.IsDeleted == false && c.Id == id
                                 select new VoucherIssuance_VM
                                 {
                                     Id = c.Id,
                                     ReasonDescription = c.ReasonDescription,
                                     ApprovedStatus = c.ApprovedStatus,
                                     IsApproved = c.ApprovedStatus == null ? (bool?)null : (c.ApprovedStatus == Helper.Approved ? true : false)
                                 }).FirstOrDefault();
            return voucherSetups;
        }
    }
}
