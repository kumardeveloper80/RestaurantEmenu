using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository
{
    public class VoucherSetupRepository : IVoucherSetupRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public VoucherSetupRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function for get voucher list
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public List<VoucherSetup_VM> GetList(int loginUserId, bool isAdmin)
        {
            var voucherSetups = (from c in _context.VoucherSetup
                                 where c.IsDeleted == false && c.CreatedBy == loginUserId
                                 select new VoucherSetup_VM
                                 {
                                     Id = c.Id,
                                     Name = c.Name != null ? c.Name : string.Empty,
                                     Description = c.Description,
                                     Terms = c.Terms,
                                     Limitations = c.Limitations,
                                     Type = c.Type,
                                     Value = c.Value,
                                     IsMultipleTimeUsage = c.IsMultipleTimeUsage,
                                     Usage = c.IsMultipleTimeUsage ? "Multiple times" : "One time",
                                     StartDate = c.StartDate != null ? c.StartDate : string.Empty,
                                     EndDate = c.EndDate != null ? c.EndDate : string.Empty,
                                     Status = c.Status,
                                 }).ToList();
            return voucherSetups;
        }

        /// <summary>
        /// Function for get voucher by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public VoucherSetup_VM Get(int id, int loginUserId, bool isAdmin)
        {
            var voucherSetup = (from c in _context.VoucherSetup
                                where c.IsDeleted == false && c.Id == id && c.CreatedBy == loginUserId
                                select new VoucherSetup_VM
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    Description = c.Description,
                                    Terms = c.Terms,
                                    Limitations = c.Limitations,
                                    Type = c.Type,
                                    Value = c.Value,
                                    IsMultipleTimeUsage = c.IsMultipleTimeUsage,
                                    StartDate = c.StartDate,
                                    EndDate = c.EndDate,
                                    Status = c.Status,
                                    StoreIds = _context.Set_VoucherStore.Where(x => x.VoucherId == id).Select(x => x.StoreId).ToList(),
                                    MenuItemIds = _context.VoucherItemType.Where(x => x.VoucherId == id).Select(x => x.MenuItemId).ToList(),
                                    SurveyIds = _context.Set_VoucherSurvey.Where(x => x.VoucherId == id).Select(x => x.SurveyId).ToList()
                                }).FirstOrDefault();
            return voucherSetup;
        }

        /// <summary>
        /// Function for add voucher
        /// </summary>
        /// <param name="voucherSetup_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(VoucherSetup_VM voucherSetup_VM, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var voucherSetup = new VoucherSetup();
                    voucherSetup.Name = voucherSetup_VM.Name;
                    voucherSetup.Description = voucherSetup_VM.Description;
                    voucherSetup.Terms = voucherSetup_VM.Terms;
                    voucherSetup.Limitations = voucherSetup_VM.Limitations;
                    voucherSetup.Type = voucherSetup_VM.Type;
                    voucherSetup.Value = voucherSetup_VM.Value;
                    voucherSetup.IsMultipleTimeUsage = voucherSetup_VM.IsMultipleTimeUsage;
                    voucherSetup.StartDate = voucherSetup_VM.StartDate;
                    voucherSetup.EndDate = voucherSetup_VM.EndDate;
                    voucherSetup.Status = voucherSetup_VM.Status;
                    voucherSetup.CreatedOn = DateTime.Now;
                    voucherSetup.CreatedBy = loginUserId;
                    _context.VoucherSetup.Add(voucherSetup);
                    _context.SaveChanges();

                    //Add voucher stores
                    var voucherStores = new List<Set_VoucherStore>();
                    foreach (var storeId in voucherSetup_VM.StoreIds)
                    {
                        var obj = new Set_VoucherStore();
                        obj.VoucherId = voucherSetup.Id;
                        obj.StoreId = storeId;
                        voucherStores.Add(obj);
                    }

                    _context.Set_VoucherStore.AddRange(voucherStores);
                    _context.SaveChanges();

                    if (voucherSetup_VM.Type == (int)VoucherSetupType.ItemType)
                    {
                        //Add voucher menu item
                        var voucherItemTypes = new List<VoucherItemType>();
                        foreach (var menuItemId in voucherSetup_VM.MenuItemIds)
                        {
                            var obj = new VoucherItemType();
                            obj.VoucherId = voucherSetup.Id;
                            obj.MenuItemId = menuItemId;
                            voucherItemTypes.Add(obj);
                        }
                        _context.VoucherItemType.AddRange(voucherItemTypes);
                        _context.SaveChanges();
                    }

                    //Add voucher survey
                    var voucherSurveys = new List<Set_VoucherSurvey>();
                    foreach (var surveyId in voucherSetup_VM.SurveyIds)
                    {
                        var obj = new Set_VoucherSurvey();
                        obj.VoucherId = voucherSetup.Id;
                        obj.SurveyId = surveyId;
                        voucherSurveys.Add(obj);
                    }
                    _context.Set_VoucherSurvey.AddRange(voucherSurveys);
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
        /// Function for update voucher
        /// </summary>
        /// <param name="voucherSetup_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(VoucherSetup_VM voucherSetup_VM, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var voucherSetup = _context.VoucherSetup.Where(x => x.Id == voucherSetup_VM.Id && x.IsDeleted == false && x.CreatedBy == loginUserId).FirstOrDefault();
                    if (voucherSetup != null)
                    {
                        voucherSetup.Name = voucherSetup_VM.Name;
                        voucherSetup.Description = voucherSetup_VM.Description;
                        voucherSetup.Terms = voucherSetup_VM.Terms;
                        voucherSetup.Limitations = voucherSetup_VM.Limitations;
                        voucherSetup.Type = voucherSetup_VM.Type;
                        voucherSetup.Value = voucherSetup_VM.Value;
                        voucherSetup.IsMultipleTimeUsage = voucherSetup_VM.IsMultipleTimeUsage;
                        voucherSetup.StartDate = voucherSetup_VM.StartDate;
                        voucherSetup.EndDate = voucherSetup_VM.EndDate;
                        voucherSetup.Status = voucherSetup_VM.Status;
                        voucherSetup.ModifiedOn = DateTime.Now;
                        voucherSetup.ModifiedBy = loginUserId;
                        _context.SaveChanges();

                        //Remove old voucher stores
                        var oldVoucherStores = _context.Set_VoucherStore.Where(x => x.VoucherId == voucherSetup.Id).ToList();
                        if (oldVoucherStores.Any())
                        {
                            _context.Set_VoucherStore.RemoveRange(oldVoucherStores);
                            _context.SaveChanges();
                        }

                        //Remove old voucher menu item
                        var oldVoucherItemTypess = _context.VoucherItemType.Where(x => x.VoucherId == voucherSetup.Id).ToList();
                        if (oldVoucherItemTypess.Any())
                        {
                            _context.VoucherItemType.RemoveRange(oldVoucherItemTypess);
                            _context.SaveChanges();
                        }

                        //Remove old voucher survey
                        var oldVoucherSurvey = _context.Set_VoucherSurvey.Where(x => x.VoucherId == voucherSetup.Id).ToList();
                        if (oldVoucherSurvey.Any())
                        {
                            _context.Set_VoucherSurvey.RemoveRange(oldVoucherSurvey);
                            _context.SaveChanges();
                        }


                        //Add voucher stores
                        var voucherStores = new List<Set_VoucherStore>();
                        foreach (var storeId in voucherSetup_VM.StoreIds)
                        {
                            var obj = new Set_VoucherStore();
                            obj.VoucherId = voucherSetup.Id;
                            obj.StoreId = storeId;
                            voucherStores.Add(obj);
                        }

                        _context.Set_VoucherStore.AddRange(voucherStores);
                        _context.SaveChanges();

                        if (voucherSetup_VM.Type == (int)VoucherSetupType.ItemType)
                        {
                            //Add voucher menu item
                            var voucherItemTypes = new List<VoucherItemType>();
                            foreach (var menuItemId in voucherSetup_VM.MenuItemIds)
                            {
                                var obj = new VoucherItemType();
                                obj.VoucherId = voucherSetup.Id;
                                obj.MenuItemId = menuItemId;
                                voucherItemTypes.Add(obj);
                            }
                            _context.VoucherItemType.AddRange(voucherItemTypes);
                            _context.SaveChanges();
                        }

                        //Add voucher survey
                        var voucherSurveys = new List<Set_VoucherSurvey>();
                        foreach (var surveyId in voucherSetup_VM.SurveyIds)
                        {
                            var obj = new Set_VoucherSurvey();
                            obj.VoucherId = voucherSetup.Id;
                            obj.SurveyId = surveyId;
                            voucherSurveys.Add(obj);
                        }
                        _context.Set_VoucherSurvey.AddRange(voucherSurveys);
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
        /// Function for delete voucher
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var voucher = _context.VoucherSetup.Where(x => x.Id == id && x.IsDeleted == false && x.CreatedBy == loginUserId).FirstOrDefault();
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
        /// Function for Get voucher for issuance 
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public List<VoucherSetup_VM> GetVoucherForIssuance(int loginUserId)
        {
            var vouchers = (from us in _context.Sec_UserStores
                            join vs in _context.Set_VoucherStore on us.StoreId equals vs.StoreId
                            join voucher in _context.VoucherSetup on vs.VoucherId equals voucher.Id
                            where us.UserId == loginUserId && voucher.IsDeleted != true && voucher.Status
                            select new VoucherSetup_VM
                            {
                                Id = voucher.Id,
                                Name = voucher.Name
                            }).DistinctBy(x=>x.Id).ToList();
            return vouchers;
        }
    }
}
