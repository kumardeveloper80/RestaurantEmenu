using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository
{
    public class MenuRepository : IMenuRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public MenuRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function for add menu
        /// </summary>
        /// <param name="menu_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(Menu_VM menu_VM, int loginUserId)
        {
            if (menu_VM != null)
            {
                using (var dbcxtransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var menu = new Emenu_Menus();
                        menu.Code = menu_VM.Code;
                        menu.Name = menu_VM.Name;
                        menu.ConceptId = menu_VM.ConceptId;
                        menu.Status = menu_VM.Status;
                        menu.CreatedOn = DateTime.Now;
                        menu.CreatedBy = loginUserId;
                        _context.Emenu_Menus.Add(menu);
                        _context.SaveChanges();

                        var menuItems = new List<Emenu_MenuMItems>();
                        foreach (var menuItemId in menu_VM.MenuItemIds)
                        {
                            var menuItem = new Emenu_MenuMItems()
                            {
                                EMenusId = menu.Id,
                                EMenuItemsId = menuItemId,
                            };
                            menuItems.Add(menuItem);
                        }
                        _context.Emenu_MenuMItems.AddRange(menuItems);
                        _context.SaveChanges();
                        dbcxtransaction.Commit();
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        dbcxtransaction.Rollback();
                        return 0;
                    }
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Function for delete menu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var ismenuRef = _context.Emenu_MenuSchedules.Where(x => x.MenuId == id && x.CreatedBy == loginUserId && x.IsDeleted != true).FirstOrDefault();
            if (ismenuRef == null)
            {
                var menu = _context.Emenu_Menus.Where(x => x.Id == id && x.IsDeleted == false && x.CreatedBy == loginUserId).FirstOrDefault();
                if (menu != null)
                {
                    menu.IsDeleted = true;
                    menu.DeletedOn = DateTime.Now;
                    menu.DeletedBy = loginUserId;
                    return _context.SaveChanges();
                }
                return 0;
            }
            else
            {
                return Helper.refernce_error_code;
            }
        }

        /// <summary>
        /// Function for get menu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public Menu_VM Get(int id, int loginUserId, bool isAdmin)
        {
            var result = (from menu in _context.Emenu_Menus
                          where menu.Id == id && menu.IsDeleted == false
                          select new Menu_VM
                          {
                              Code = menu.Code,
                              Name = menu.Name,
                              Status = menu.Status,
                              MenuItemIds = _context.Emenu_MenuMItems.Where(x => x.EMenusId == id).Select(x => x.EMenuItemsId).ToList(),
                              CreatedBy = menu.CreatedBy,
                              ConceptId = menu.ConceptId,
                          }).FirstOrDefault();

            if (!isAdmin && result != null)
            {
                result = result.CreatedBy == loginUserId ? result : null;
            }
            return result;
        }

        /// <summary>
        /// Function for get list of menu
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public List<Menu_VM> GetList(int loginUserId, bool isAdmin)
        {
            var menus = (from menu in _context.Emenu_Menus
                         join concept in _context.Set_Concepts on menu.ConceptId equals concept.Id
                         where menu.IsDeleted == false && concept.IsDeleted != true
                         select new Menu_VM
                         {
                             Id = menu.Id,
                             Code = menu.Code,
                             Name = menu.Name,
                             Status = menu.Status,
                             CreatedBy = menu.CreatedBy,
                             ConceptId = menu.ConceptId,
                             ConceptName = concept.ConceptName
                         }).ToList();


            //var ClientId = _context.Sec_Users.Where(c => c.Id == loginUserId).Select(c => c.ClientId).FirstOrDefault();
            //if (isAdmin)
            //{
            //    var allSubUsers = _context.Sec_Users.Where(c => c.ClientId == ClientId).Select(c => c.Id).ToList();
            //    //display all the items created by this user and the users under it.
            //    menus = menus.Where(x => x.CreatedBy == loginUserId || allSubUsers.Contains(x.CreatedBy)).ToList();
            //}
            //else
            //{
                var StoreIds = _context.Sec_UserStores.Where(c => c.UserId == loginUserId).Select(c => c.StoreId).ToList();
                var UserConceptIds = _context.Set_StoresConcepts.Where(c => StoreIds.Contains(c.StoreId)).Select(c => c.ConceptId).ToList();
                menus = menus.Where(c => UserConceptIds.Contains(c.ConceptId)).ToList();
           // }


            return menus;

        }

        /// <summary>
        /// Fuction for check unique menu code
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="conceptId"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public bool IsUniqueMenuCode(int id, string code, int conceptId, int loginUserId, bool isAdmin)
        {
            var ret = false;
            var menu = _context.Emenu_Menus.Where(x => x.Code == code && !x.IsDeleted && x.ConceptId == conceptId).ToList();
            if (menu.Count > 0)
            {
                if (!isAdmin)
                {
                    menu = menu.Where(x => x.CreatedBy == loginUserId).ToList();
                }

                // For edit mode
                if (id > 0)
                {
                    var isSameId = menu.Where(x => x.Id == id).FirstOrDefault();
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
        /// Function for check unique menu name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="conceptId"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public bool IsUniqueMenuName(int id, string name, int conceptId, int loginUserId, bool isAdmin)
        {
            var ret = false;
            var menu = _context.Emenu_Menus.Where(x => x.Name == name && !x.IsDeleted && x.ConceptId == conceptId).ToList();
            if (menu.Count > 0)
            {
                if (!isAdmin)
                {
                    menu = menu.Where(x => x.CreatedBy == loginUserId).ToList();
                }

                // For edit mode
                if (id > 0)
                {
                    var isSameId = menu.Where(x => x.Id == id).FirstOrDefault();
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
        /// Function for udpae menu
        /// </summary>
        /// <param name="menu_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(Menu_VM menu_VM, int loginUserId)
        {
            var menu = _context.Emenu_Menus.Where(x => x.Id == menu_VM.Id && x.IsDeleted == false).FirstOrDefault();
            if (menu != null)
            {
                using (var dbcxtransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        menu.Code = menu_VM.Code;
                        menu.Name = menu_VM.Name;
                        menu.ConceptId = menu_VM.ConceptId;
                        menu.Status = menu_VM.Status;
                        menu.ModifiedOn = DateTime.Now;
                        menu.ModifiedBy = loginUserId;
                        _context.SaveChanges();

                        // Delete old
                        var oldMenuMItems = _context.Emenu_MenuMItems.Where(x => x.EMenusId == menu_VM.Id).ToList();
                        if (oldMenuMItems.Any())
                        {
                            _context.Emenu_MenuMItems.RemoveRange(oldMenuMItems);
                            _context.SaveChanges();
                        }

                        // Add new
                        var menuItems = new List<Emenu_MenuMItems>();
                        foreach (var menuItemId in menu_VM.MenuItemIds)
                        {
                            var menuItem = new Emenu_MenuMItems()
                            {
                                EMenusId = menu_VM.Id,
                                EMenuItemsId = menuItemId,
                            };
                            menuItems.Add(menuItem);
                        }
                        _context.Emenu_MenuMItems.AddRange(menuItems);
                        _context.SaveChanges();

                        dbcxtransaction.Commit();
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        dbcxtransaction.Rollback();
                        return 0;
                    }
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Get Category by menuId
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public List<CategorySequence_VM> GetMenuCategory(int menuId, int loginUserId, bool isAdmin)
        {
            var result = (from menu in _context.Emenu_Menus
                          join sMenuMItems in _context.Emenu_MenuMItems on menu.Id equals sMenuMItems.EMenusId
                          join menuItems in _context.Emenu_MenuItems on sMenuMItems.EMenuItemsId equals menuItems.Id // all menu items
                          join category in _context.Emenu_Category on menuItems.CategoryId equals category.Id // categories

                          join categorySeq in _context.Category_Sequence.Where(x => x.MenuId == menuId) on category.Id equals categorySeq.CategoryId into cs
                          from categorySeq in cs.DefaultIfEmpty()

                          where menu.Id == menuId
                          select new CategorySequence_VM
                          {
                              CategorySequence = categorySeq != null && categorySeq.Sequence != 0 ? categorySeq.Sequence : 0,
                              CategoryId = category.Id,
                              CategoryName = category.Name,
                              MenuId = menuId,
                              CreatedBy = menu.CreatedBy
                          }).ToList();

            if (result.Any())
            {
                if (!isAdmin)
                {
                    result = result.Where(x => x.CreatedBy == loginUserId).ToList();
                }
                result = result.DistinctBy(x => x.CategoryId).OrderBy(x => x.CategorySequence == 0).ThenBy(x => x.CategorySequence).ToList();
            }
            return result;
        }

        /// <summary>
        /// Manage Category Sequence
        /// </summary>
        /// <returns></returns>
        public int ManageCategorySequence(List<CategorySequence_VM> categorySequence_VMs)
        {
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var category in categorySequence_VMs)
                    {
                        var categorySeq = _context.Category_Sequence.Where(x => x.CategoryId == category.CategoryId && x.MenuId == category.MenuId).FirstOrDefault();
                        if (categorySeq != null)
                        {
                            categorySeq.Sequence = category.CategorySequence;
                            _context.SaveChanges();
                        }
                        else
                        {
                            var obj = new Category_Sequence();
                            obj.CategoryId = category.CategoryId;
                            obj.MenuId = category.MenuId;
                            obj.Sequence = category.CategorySequence;
                            _context.Category_Sequence.Add(obj);
                            _context.SaveChanges();
                        }
                    }
                    dbcxtransaction.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    dbcxtransaction.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// Get Menu item by menuId and categoryId
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="categoryId"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public List<ItemSequence_VM> GetMenuItem(int menuId, int categoryId, int loginUserId, bool isAdmin)
        {
            var result = (from menu in _context.Emenu_Menus
                          join sMenuMItems in _context.Emenu_MenuMItems on menu.Id equals sMenuMItems.EMenusId
                          join menuItems in _context.Emenu_MenuItems on sMenuMItems.EMenuItemsId equals menuItems.Id // all menu items

                          join itemSeq in _context.Item_Sequence.Where(x => x.MenuId == menuId) on menuItems.Id equals itemSeq.MenuItemId into itemsequence
                          from itemSeq in itemsequence.DefaultIfEmpty()

                          where menu.Id == menuId && menuItems.CategoryId == categoryId
                          select new ItemSequence_VM
                          {
                              MenuId = menuId,
                              MenuItemId = menuItems.Id,
                              MenuItemName = menuItems.Name,
                              ItemSequence = itemSeq != null && itemSeq.Sequence != 0 ? itemSeq.Sequence : 0,
                              CreatedBy = menu.CreatedBy
                          }).ToList();
            if (result.Any())
            {
                if (!isAdmin)
                {
                    result = result.Where(x => x.CreatedBy == loginUserId).ToList();
                }
                result = result.OrderBy(x => x.ItemSequence == 0).ThenBy(x => x.ItemSequence).ToList();
            }
            return result;
        }

        /// <summary>
        /// Manage Menu Item Sequence
        /// </summary>
        /// <returns></returns>
        public int ManageMenuItemSequence(List<ItemSequence_VM> itemSequence_VMs)
        {
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var menitem in itemSequence_VMs)
                    {
                        var menitemSeq = _context.Item_Sequence.Where(x => x.MenuItemId == menitem.MenuItemId && x.MenuId == menitem.MenuId).FirstOrDefault();
                        if (menitemSeq != null)
                        {
                            menitemSeq.Sequence = menitem.ItemSequence;
                            _context.SaveChanges();
                        }
                        else
                        {
                            var obj = new Item_Sequence();
                            obj.MenuItemId = menitem.MenuItemId;
                            obj.MenuId = menitem.MenuId;
                            obj.Sequence = menitem.ItemSequence;
                            _context.Item_Sequence.Add(obj);
                            _context.SaveChanges();
                        }
                    }
                    dbcxtransaction.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    dbcxtransaction.Rollback();
                    return 0;
                }
            }
        }
    }
}
