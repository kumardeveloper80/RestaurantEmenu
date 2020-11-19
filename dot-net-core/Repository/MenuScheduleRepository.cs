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
    public class MenuScheduleRepository : IMenuScheduleRepository
    {

        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public MenuScheduleRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function for add menu schedule
        /// </summary>
        /// <param name="menuSchedule_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(MenuSchedule_VM menuSchedule_VM, int loginUserId)
        {
            string[] dates = menuSchedule_VM.DateRange.Split('-');
            var startDate = DateTime.Parse(dates[0]);
            var endDate = DateTime.Parse(dates[1]);

            var isOnSameDate = _context.Emenu_MenuSchedules.
                Where(x => x.ConceptId == menuSchedule_VM.ConceptId
                && x.StoreId == menuSchedule_VM.StoreId
                && x.Status && !x.IsDeleted
                && ((x.StartDate >= startDate && x.StartDate <= endDate) || (x.EndDate >= startDate && x.EndDate <= endDate))
                )
                .FirstOrDefault();
            if (isOnSameDate == null)
            {
                var menuSchedule = new Emenu_MenuSchedules();
                menuSchedule.ConceptId = menuSchedule_VM.ConceptId;
                menuSchedule.Code = menuSchedule_VM.Code;
                menuSchedule.MenuId = menuSchedule_VM.MenuId;
                menuSchedule.StartDate = startDate;
                menuSchedule.EndDate = endDate;
                menuSchedule.Status = menuSchedule_VM.Status;
                menuSchedule.CreatedOn = DateTime.Now;
                menuSchedule.CreatedBy = loginUserId;
                menuSchedule.UniqueCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12);
                menuSchedule.StoreId = menuSchedule_VM.StoreId;
                _context.Emenu_MenuSchedules.Add(menuSchedule);
                return _context.SaveChanges();
            }
            else
            {
                return Helper.onsamedate_code;
            }
        }

        /// <summary>
        /// Function for delete menu schedule
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var menuSchedule = _context.Emenu_MenuSchedules.Where(x => x.Id == id && x.IsDeleted == false && x.CreatedBy == loginUserId).FirstOrDefault();
            if (menuSchedule != null)
            {
                menuSchedule.IsDeleted = true;
                menuSchedule.DeletedOn = DateTime.Now;
                menuSchedule.DeletedBy = loginUserId;
                return _context.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// Function for get menu schedule
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public MenuSchedule_VM Get(int id, int loginUserId, bool isAdmin)
        {
            var menuSchedule = (from c in _context.Emenu_MenuSchedules
                                where c.IsDeleted == false && c.Id == id
                                select new MenuSchedule_VM
                                {
                                    Id = c.Id,
                                    Code = c.Code,
                                    MenuId = c.MenuId,
                                    Status = c.Status,
                                    CreatedBy = c.CreatedBy,
                                    DateRange = c.StartDate.ToString("MM/dd/yyyy hh:mm tt") + " - " + c.EndDate.ToString("MM/dd/yyyy hh:mm tt"),
                                    UniqueCode = c.UniqueCode,
                                    ConceptId = c.ConceptId,
                                    StoreId = c.StoreId
                                }).FirstOrDefault();

            return menuSchedule;
        }

        /// <summary>
        /// Function for get list of menu schedule
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="baseURL"></param>
        /// <returns></returns>
        public List<MenuSchedule_VM> GetList(int loginUserId, string baseURL, bool isAdmin)
        {
            var menuSchedules = (from menuSchedule in _context.Emenu_MenuSchedules
                                 join menu in _context.Emenu_Menus on menuSchedule.MenuId equals menu.Id
                                 join store in _context.Set_Stores on menuSchedule.StoreId equals store.Id
                                 join concept in _context.Set_Concepts on menuSchedule.ConceptId equals concept.Id
                                 where menuSchedule.IsDeleted == false
                                 select new MenuSchedule_VM
                                 {
                                     Id = menuSchedule.Id,
                                     Code = menuSchedule.Code,
                                     Status = menuSchedule.Status,
                                     MenuName = menu.Name + "(" + menu.Code + ")",
                                     CreatedBy = menuSchedule.CreatedBy,
                                     StartDate = menuSchedule.StartDate.ToString("MM/dd/yyyy hh:mm tt"),
                                     EndDate = menuSchedule.EndDate.ToString("MM/dd/yyyy hh:mm tt"),
                                     URL = baseURL + Helper.Convert_StringvalueToHexvalue(menuSchedule.UniqueCode, System.Text.Encoding.Unicode),
                                     StoreName = store.StoreName,
                                     ConceptName = concept.ConceptName,
                                     ConceptId = concept.Id,
                                     MenuId = menu.Id,
                                     StoreId = store.Id
                                 }).ToList();


            //var ClientId = _context.Sec_Users.Where(c => c.Id == loginUserId).Select(c => c.ClientId).FirstOrDefault();
            //if (isAdmin)
            //{
            //    var allSubUsers = _context.Sec_Users.Where(c => c.ClientId == ClientId).Select(c => c.Id).ToList();
            //    //display all the items created by this user and the users under it.
            //    menuSchedules = menuSchedules.Where(x => x.CreatedBy == loginUserId || allSubUsers.Contains(x.CreatedBy)).ToList();
            //}
            //else
            //{

            var StoreIds = _context.Sec_UserStores.Where(c => c.UserId == loginUserId).Select(c => c.StoreId).ToList();
            var UserConceptIds = _context.Set_StoresConcepts.Where(c => StoreIds.Contains(c.StoreId)).Select(c => c.ConceptId).ToList();
            menuSchedules = menuSchedules.Where(c => UserConceptIds.Contains(c.ConceptId)).ToList();
            //}


            return menuSchedules;
        }

        /// <summary>
        /// Function for check is unique menu schedule code
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="conceptId"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public bool IsUniqueMenuScheduleCode(int id, string code, int conceptId, int loginUserId, bool isAdmin)
        {
            var ret = false;
            var menuSchedules = _context.Emenu_MenuSchedules.Where(x => x.Code == code && !x.IsDeleted && x.ConceptId == conceptId).ToList();
            if (menuSchedules.Count > 0)
            {
                if (!isAdmin)
                {
                    menuSchedules = menuSchedules.Where(x => x.CreatedBy == loginUserId).ToList();
                }

                // For edit mode
                if (id > 0)
                {
                    var isSameId = menuSchedules.Where(x => x.Id == id).FirstOrDefault();
                    if (isSameId != null)
                    {
                        ret = true;
                    }
                }
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Function for update menu schedule
        /// </summary>
        /// <param name="menuSchedule_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(MenuSchedule_VM menuSchedule_VM, int loginUserId)
        {
            string[] dates = menuSchedule_VM.DateRange.Split('-');
            var startDate = DateTime.Parse(dates[0]);
            var endDate = DateTime.Parse(dates[1]);

            var isOnSameDate = _context.Emenu_MenuSchedules.
               Where(x => x.ConceptId == menuSchedule_VM.ConceptId
               && x.Id != menuSchedule_VM.Id
               && x.StoreId == menuSchedule_VM.StoreId
               && x.Status && !x.IsDeleted
               && ((x.StartDate >= startDate && x.StartDate <= endDate) || (x.EndDate >= startDate && x.EndDate <= endDate))
               )
               .FirstOrDefault();

            if (isOnSameDate == null)
            {
                var menuSchedule = _context.Emenu_MenuSchedules.Where(x => x.Id == menuSchedule_VM.Id && x.IsDeleted == false).FirstOrDefault();
                if (menuSchedule != null)
                {
                    menuSchedule.Code = menuSchedule_VM.Code;
                    menuSchedule.ConceptId = menuSchedule_VM.ConceptId;
                    menuSchedule.MenuId = menuSchedule_VM.MenuId;
                    menuSchedule.Status = menuSchedule_VM.Status;
                    menuSchedule.StartDate = startDate;
                    menuSchedule.EndDate = endDate;
                    menuSchedule.ModifiedBy = loginUserId;
                    menuSchedule.ModifiedOn = DateTime.Now;
                    menuSchedule.StoreId = menuSchedule_VM.StoreId;
                    return _context.SaveChanges();
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return Helper.onsamedate_code;
            }
        }

        /// <summary>
        /// Function for get menu by uniquecode
        /// </summary>
        /// <param name="code"></param>
        public MenuSchedule_VM GetMenuByUniqueCode(string lang, string code)
        {
            var MenuScheduleList = _context.Emenu_MenuSchedules.Where(x => x.Status).AsEnumerable();
            var MenuList = _context.Emenu_Menus.Where(x => x.Status).AsEnumerable();
            var ConceptThemeList = _context.Concept_Theme.Where(x => x.Status).AsEnumerable();
            var MenuMItemsList = _context.Emenu_MenuMItems.AsEnumerable();
            var MenuItemsList = _context.Emenu_MenuItems.Where(x => x.Status).AsEnumerable();
            var CategoryList = _context.Emenu_Category.Where(x => x.Status).AsEnumerable();
            var CategorySequenceList = _context.Category_Sequence.AsEnumerable();
            var ItemSequenceList = _context.Item_Sequence.AsEnumerable();
            var CurrencyList = _context.Emenu_Currency.AsEnumerable();
            var MenuItemTagsList = _context.Emenu_MenuItemTags.AsEnumerable();
            var ItemTagsList = _context.Emenu_ItemTags.Where(x => x.Status).AsEnumerable();

            var result = (from menuSchedules in MenuScheduleList
                          join menu in MenuList on menuSchedules.MenuId equals menu.Id

                          join conceptTheme in ConceptThemeList.Where(x => x.IsDeleted != true) on menuSchedules.ConceptId equals conceptTheme.ConceptId into t
                          from conceptTheme in t.DefaultIfEmpty()

                          where menuSchedules.UniqueCode == code && !menuSchedules.IsDeleted && menuSchedules.Status
                          && !menu.IsDeleted && menu.Status
                          select new MenuSchedule_VM
                          {
                              Id = menuSchedules.Id,
                              Code = menuSchedules.Code,
                              dStartDate = menuSchedules.StartDate,
                              dEndDate = menuSchedules.EndDate,
                              Status = menuSchedules.Status,
                              StoreId = menuSchedules.StoreId,
                              StoreGuid = _context.Set_Stores.Where(x => x.Id == menuSchedules.StoreId).Select(x => x.StoreGuid).FirstOrDefault(),
                              ColorCode = conceptTheme != null ? conceptTheme.ColorCode : null,
                              Logo = conceptTheme != null && conceptTheme.LogoName != null ? Helper.GetImagePath(conceptTheme.LogoName) : null,
                              FeedBackIcon = conceptTheme != null && conceptTheme.FeedBackIconName != null ? Helper.GetImagePath(conceptTheme.FeedBackIconName) : null,
                              Categories = (from sMenuMItems in MenuMItemsList
                                            join menuItems in MenuItemsList on sMenuMItems.EMenuItemsId equals menuItems.Id // all menu items
                                            join category in CategoryList on menuItems.CategoryId equals category.Id // categories

                                            join categorySeq in CategorySequenceList on category.Id equals categorySeq.CategoryId into cs
                                            from categorySeq in cs.DefaultIfEmpty()

                                            where sMenuMItems.EMenusId == menu.Id
                                            select new Category_VM
                                            {
                                                Id = category.Id,
                                                Code = category.Code,
                                                Name = category.Name,
                                                LabelEN = lang == Helper.lang_ar ? category.LabelAR : category.LabelEN,
                                                DetailsEN = lang == Helper.lang_ar ? category.DetailsAR : category.DetailsEN,
                                                ImageName = category.ImageName != null ? Helper.GetImagePath(category.ImageName) : "",
                                                Status = category.Status,
                                                CategorySequence = categorySeq != null && categorySeq.Sequence != 0 ? categorySeq.Sequence : 0,
                                            }).ToList(),
                              Menu = new Menu_VM()
                              {
                                  Id = menu.Id,
                                  Code = menu.Code,
                                  Name = menu.Name,
                                  Status = menu.Status,
                                  MenuItems = (from sMenuMItems in MenuMItemsList // seleted menu items
                                               join menuItems in MenuItemsList on sMenuMItems.EMenuItemsId equals menuItems.Id // all menu items

                                               join itemSeq in ItemSequenceList on sMenuMItems.EMenuItemsId equals itemSeq.MenuItemId into itemsequence
                                               from itemSeq in itemsequence.DefaultIfEmpty()

                                               where sMenuMItems.EMenusId == menu.Id && !menuItems.IsDeleted && menuItems.Status
                                               select new MenuItem_VM
                                               {
                                                   Id = menuItems.Id,
                                                   PLU = menuItems.PLU,
                                                   Name = menuItems.Name,
                                                   CategoryId = menuItems.CategoryId,
                                                   Currency = CurrencyList.Where(x => x.Id == menuItems.CurrencyId).Select(x => lang == Helper.lang_ar ? x.SymbolAR : x.Symbol).FirstOrDefault(),
                                                   Price = menuItems.Price,
                                                   sPrice = lang == Helper.lang_ar ? Helper.ConvertNumeralsToArabic(Helper.DecimalToString(menuItems.Price)) : Helper.DecimalToString(menuItems.Price),
                                                   DetailsEN = lang == Helper.lang_ar ? menuItems.DetailsAR : menuItems.DetailsEN,
                                                   ThumbnailImageName = menuItems.ThumbnailImageName != null ? Helper.GetImagePath(menuItems.ThumbnailImageName) : "",
                                                   LargeImageName = menuItems.LargeImageName != null ? Helper.GetImagePath(menuItems.LargeImageName) : "",
                                                   LargeDetailsEN = lang == Helper.lang_ar ? menuItems.LargeDetailsAR : menuItems.LargeDetailsEN,
                                                   OverLayImageName = menuItems.OverLayImageName != null ? Helper.GetImagePath(menuItems.OverLayImageName) : "",
                                                   OverlayDetailsEN = lang == Helper.lang_ar ? menuItems.OverlayDetailsAR : menuItems.OverlayDetailsEN,
                                                   ItemTags = (from sMenuItemTags in MenuItemTagsList // selected menu item tags
                                                               join itemTag in ItemTagsList on sMenuItemTags.ItemTagId equals itemTag.Id // all menu item, tags
                                                               where sMenuItemTags.EMenuItemId == menuItems.Id && !itemTag.IsDeleted && itemTag.Status
                                                               select new ItemTag_VM
                                                               {
                                                                   Id = itemTag.Id,
                                                                   Name = itemTag.Name,
                                                                   Status = itemTag.Status,
                                                                   IconTagName = itemTag.IconTagName != null ? Helper.GetImagePath(itemTag.IconTagName) : "",
                                                                   LabelEN = lang == Helper.lang_ar ? itemTag.LabelAR : itemTag.LabelEN,
                                                               }).ToList(),
                                                   CommentsEN = lang == Helper.lang_ar ? menuItems.CommentsAR : menuItems.CommentsEN,
                                                   LabelEN = lang == Helper.lang_ar ? menuItems.LabelAR : menuItems.LabelEN,
                                                   MenuItemSequence = itemSeq != null && itemSeq.Sequence != 0 ? itemSeq.Sequence : 0,
                                               }).ToList()
                              }
                          }).FirstOrDefault();

            if (result != null)
            {
                if (result.Categories.Any())
                {
                    result.Categories = result.Categories.DistinctBy(x => x.Id).OrderBy(x => x.CategorySequence == 0).ThenBy(x => x.CategorySequence).ToList();
                }
                if (result.Menu != null && result.Menu.MenuItems.Any())
                {
                    result.Menu.MenuItems = result.Menu.MenuItems.DistinctBy(x => x.Id).OrderBy(x => x.MenuItemSequence == 0).ThenBy(x => x.MenuItemSequence).ToList();
                }
            }
            return result;
        }
    }
}
