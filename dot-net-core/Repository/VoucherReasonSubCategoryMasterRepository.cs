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
    public class VoucherReasonSubCategoryMasterRepository : IVoucherReasonSubCategoryMasterRepository
    {
        // <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;


        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public VoucherReasonSubCategoryMasterRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get list
        /// </summary>
        /// <returns></returns>
        public List<ReasonSubCategoryMaster_VM> GetList()
        {
            var list = (from subcategory in _context.VoucherReasonSubCategoryMaster
                        join category in _context.VoucherReasonCategoryMaster on subcategory.ReasonCategoryId equals category.Id
                        where subcategory.IsDeleted != true
                        select new ReasonSubCategoryMaster_VM()
                        {
                            Id = subcategory.Id,
                            Name = subcategory.Name,
                            ReasonCategoryId = subcategory.ReasonCategoryId,
                            ReasonCategoryName = category.Name,
                            Status = subcategory.Status,
                            ClientId = subcategory.ClientId
                        }).ToList();
            return list;
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <returns></returns>
        public ReasonSubCategoryMaster_VM Get(int id)
        {
            var subcategory = (from c in _context.VoucherReasonSubCategoryMaster
                               where c.IsDeleted != true && c.Id == id
                               select new ReasonSubCategoryMaster_VM()
                               {
                                   Id = c.Id,
                                   Name = c.Name,
                                   ReasonCategoryId = c.ReasonCategoryId,
                                   Status = c.Status
                               }).FirstOrDefault();
            return subcategory;
        }

        /// <summary>
        /// Function for detele sub-category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var subcategory = _context.VoucherReasonSubCategoryMaster.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
            if (subcategory != null)
            {
                var isSubCategoryRef = _context.VoucherIssuance.Where(x => x.IsDeleted != true && x.ReasonSubCategoryId == id).FirstOrDefault();
                if (isSubCategoryRef == null)
                {
                    subcategory.IsDeleted = true;
                    subcategory.DeletedOn = DateTime.Now;
                    subcategory.DeletedBy = loginUserId;
                    return _context.SaveChanges();
                }
                else
                {
                    return Helper.refernce_error_code;
                }
            }
            return 0;
        }

        /// <summary>
        /// Function add sub-category
        /// </summary>
        /// <param name="country_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(ReasonSubCategoryMaster_VM subCategoryMaster_VM, int loginUserId)
        {
            var obj = new VoucherReasonSubCategoryMaster();
            obj.Name = subCategoryMaster_VM.Name;
            obj.Status = subCategoryMaster_VM.Status;
            obj.ClientId = subCategoryMaster_VM.ClientId;
            obj.ReasonCategoryId = subCategoryMaster_VM.ReasonCategoryId;
            obj.CreatedBy = loginUserId;
            obj.CreatedOn = DateTime.Now;
            _context.VoucherReasonSubCategoryMaster.Add(obj);
            return _context.SaveChanges();
        }

        /// <summary>
        /// Function for update sub category
        /// </summary>
        /// <param name="country_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(ReasonSubCategoryMaster_VM subCategoryMaster_VM, int loginUserId)
        {
            var obj = _context.VoucherReasonSubCategoryMaster.Where(x => x.Id == subCategoryMaster_VM.Id && x.IsDeleted == false).FirstOrDefault();
            if (obj != null)
            {
                obj.Name = subCategoryMaster_VM.Name;
                obj.Status = subCategoryMaster_VM.Status;
                obj.ReasonCategoryId = subCategoryMaster_VM.ReasonCategoryId;
                obj.ClientId = subCategoryMaster_VM.ClientId;
                obj.ModifiedBy = loginUserId;
                obj.ModifiedOn = DateTime.Now;
                return _context.SaveChanges();
            }
            return 0;
        }
    }
}
