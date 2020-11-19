using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMenuApplication.Repository
{
    public class RegionRepository : IRegionRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public RegionRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function for get region by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Region_VM Get(int id)
        {
            var region = (from r in _context.Set_Region
                          where r.IsDeleted != true && r.Id == id
                          select new Region_VM()
                          {
                              RegionId = r.Id,
                              Region = r.Region,
                              Status = r.Status
                          }).FirstOrDefault();
            return region;
        }

        /// <summary>
        /// Function for get region list
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public List<Region_VM> GetList()
        {
            var region = (from r in _context.Set_Region
                          where r.IsDeleted != true
                          select new Region_VM()
                          {
                              RegionId = r.Id,
                              Region = r.Region,
                              Status = r.Status
                          }).ToList();
            return region;
        }

        /// <summary>
        /// Function for check unique region name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyName"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public bool IsUniqueRegionName(int id, string regionName)
        {
            var ret = false;
            var result = _context.Set_Region.Where(x => x.IsDeleted != true && x.Region == regionName).FirstOrDefault();
            if (result != null)
            {
                if (id > 0 && result.Id == id)
                {
                    ret = true;
                }
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Function for delete region
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var region = _context.Set_Region.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
            if (region != null)
            {
                region.IsDeleted = true;
                region.DeletedOn = DateTime.Now;
                region.DeletedBy = loginUserId;
                return _context.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// Fucntion for save region
        /// </summary>
        /// <param name="region_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(Region_VM region_VM, int loginUserId)
        {
            var obj = new Set_Region();
            obj.Region = region_VM.Region;
            obj.Status = region_VM.Status;
            obj.CreatedBy = loginUserId;
            obj.CreatedOn = DateTime.Now;
            _context.Set_Region.Add(obj);
            return _context.SaveChanges();
        }

        /// <summary>
        /// Fucntion for update region
        /// </summary>
        /// <param name="region_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(Region_VM region_VM, int loginUserId)
        {
            var obj = _context.Set_Region.Where(x => x.Id == region_VM.RegionId && x.IsDeleted == false).FirstOrDefault();
            if (obj != null)
            {
                obj.Region = region_VM.Region;
                obj.Status = region_VM.Status;
                obj.ModifiedBy = loginUserId;
                obj.ModifiedOn = DateTime.Now;
                return _context.SaveChanges();
            }
            return 0;
        }
    }
}
