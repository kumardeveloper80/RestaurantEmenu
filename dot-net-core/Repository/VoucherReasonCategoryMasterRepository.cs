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
    public class VoucherReasonCategoryMasterRepository : IVoucherReasonCategoryMasterRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;


        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public VoucherReasonCategoryMasterRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get list
        /// </summary>
        /// <returns></returns>
        public List<ReasonCategoryMaster_VM> GetList()
        {
            var list = (from category in _context.VoucherReasonCategoryMaster
                        where category.IsDeleted != true
                        select new ReasonCategoryMaster_VM()
                        {
                            Id = category.Id,
                            Name = category.Name,
                            Status = category.Status,
                            ClientId = category.ClientId
                        }).ToList();
            return list;
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <returns></returns>
        public ReasonCategoryMaster_VM Get(int id)
        {
            var category = (from c in _context.VoucherReasonCategoryMaster
                            where c.IsDeleted != true && c.Id == id
                            select new ReasonCategoryMaster_VM()
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Status = c.Status,
                                ClientId = c.ClientId
                            }).FirstOrDefault();
            return category;
        }

        /// <summary>
        /// Function for detele category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var category = _context.VoucherReasonCategoryMaster.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
            if (category != null)
            {
                var isCategoryRef = _context.VoucherReasonSubCategoryMaster.Where(x => x.IsDeleted != true && x.ReasonCategoryId == id).FirstOrDefault();
                if(isCategoryRef == null)
                {
                    category.IsDeleted = true;
                    category.DeletedOn = DateTime.Now;
                    category.DeletedBy = loginUserId;
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
        /// Function add category
        /// </summary>
        /// <param name="country_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(ReasonCategoryMaster_VM reasonCategoryMaster_VM, int loginUserId)
        {
            var obj = new VoucherReasonCategoryMaster();
            obj.Name = reasonCategoryMaster_VM.Name;
            obj.Status = reasonCategoryMaster_VM.Status;
            obj.ClientId = reasonCategoryMaster_VM.ClientId;
            obj.CreatedBy = loginUserId;
            obj.CreatedOn = DateTime.Now;
            _context.VoucherReasonCategoryMaster.Add(obj);
            return _context.SaveChanges();
        }

        /// <summary>
        /// Function for update category
        /// </summary>
        /// <param name="country_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(ReasonCategoryMaster_VM reasonCategoryMaster_VM, int loginUserId)
        {
            var obj = _context.VoucherReasonCategoryMaster.Where(x => x.Id == reasonCategoryMaster_VM.Id && x.IsDeleted == false).FirstOrDefault();
            if (obj != null)
            {
                obj.Name = reasonCategoryMaster_VM.Name;
                obj.Status = reasonCategoryMaster_VM.Status;
                obj.ClientId = reasonCategoryMaster_VM.ClientId;
                obj.ModifiedBy = loginUserId;
                obj.ModifiedOn = DateTime.Now;
                return _context.SaveChanges();
            }
            return 0;
        }
    }
}
