using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.ViewModels;
using System.Linq;

namespace EMenuApplication.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public DashboardRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function for get total user, category, menu, menu items
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public Dashboard_VM GetCounts(int loginUserId, bool isAdmin)
        {
            var counts = new Dashboard_VM();
            if (isAdmin)
            {
                counts.TotalUser = _context.Sec_Users.Where(x => x.IsAdmin.Value != true && x.IsDeleted.Value != true && x.CreatedBy == loginUserId).Count();
            }





            //if (isAdmin)
            //{
            //    counts.TotalCategories = _context.Emenu_Category.Where(x => !x.IsDeleted).Count();
            //    counts.TotalMenuItems = _context.Emenu_MenuItems.Where(x => !x.IsDeleted).Count();
            //    counts.TotalMenu = _context.Emenu_Menus.Where(x => !x.IsDeleted).Count();
            //}
            //else
            //{
            //    counts.TotalCategories = _context.Emenu_Category.Where(x => !x.IsDeleted && x.CreatedBy == loginUserId).Count();
            //    counts.TotalMenuItems = _context.Emenu_MenuItems.Where(x => !x.IsDeleted && x.CreatedBy == loginUserId).Count();
            //    counts.TotalMenu = _context.Emenu_Menus.Where(x => !x.IsDeleted && x.CreatedBy == loginUserId).Count();
            //}
            return counts;
        }
    }
}
