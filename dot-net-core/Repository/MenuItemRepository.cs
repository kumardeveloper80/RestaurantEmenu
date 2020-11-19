using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMenuApplication.Repository
{
    public class MenuItemRepository : IMenuItemRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public MenuItemRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Funtion for add menu item
        /// </summary>
        /// <param name="menuItem_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(MenuItem_VM menuItem_VM, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var menuItem = new Emenu_MenuItems();
                    menuItem.PLU = menuItem_VM.PLU;
                    menuItem.Name = menuItem_VM.Name;
                    menuItem.CategoryId = menuItem_VM.CategoryId;
                    menuItem.CurrencyId = menuItem_VM.CurrencyId;
                    menuItem.Price = menuItem_VM.Price.Value;
                    menuItem.Status = menuItem_VM.Status;
                    menuItem.DetailsEN = menuItem_VM.DetailsEN;
                    menuItem.DetailsAR = menuItem_VM.DetailsAR;
                    menuItem.ThumbnailImageName = menuItem_VM.ThumbnailImageName;
                    menuItem.LargeImageName = menuItem_VM.LargeImageName;
                    menuItem.LargeDetailsEN = menuItem_VM.LargeDetailsEN;
                    menuItem.LargeDetailsAR = menuItem_VM.LargeDetailsAR;
                    menuItem.OverLayImageName = menuItem_VM.OverLayImageName;
                    menuItem.OverlayDetailsEN = menuItem_VM.OverlayDetailsEN;
                    menuItem.OverlayDetailsAR = menuItem_VM.OverlayDetailsAR;
                    menuItem.CreatedOn = DateTime.Now;
                    menuItem.CreatedBy = loginUserId;
                    menuItem.CommentsEN = menuItem_VM.CommentsEN;
                    menuItem.CommentsAR = menuItem_VM.CommentsAR;
                    menuItem.LabelEN = menuItem_VM.LabelEN;
                    menuItem.LabelAR = menuItem_VM.LabelAR;

                    _context.Emenu_MenuItems.Add(menuItem);
                    _context.SaveChanges();

                    var menuItemTags = new List<Emenu_MenuItemTags>();
                    foreach (var itemTagId in menuItem_VM.ItemTagsId)
                    {
                        var menuItemTag = new Emenu_MenuItemTags()
                        {
                            EMenuItemId = menuItem.Id,
                            ItemTagId = itemTagId,
                        };
                        menuItemTags.Add(menuItemTag);
                    }

                    _context.Emenu_MenuItemTags.AddRange(menuItemTags);
                    _context.SaveChanges();

                    var menuItemConcepts = new List<Emenu_MenuItemsConcepts>();
                    foreach (var conceptId in menuItem_VM.ConceptIds)
                    {
                        var menuItemConcept = new Emenu_MenuItemsConcepts();
                        menuItemConcept.MenuItemId = menuItem.Id;
                        menuItemConcept.ConceptId = conceptId;
                        menuItemConcepts.Add(menuItemConcept);
                    }

                    _context.Emenu_MenuItemsConcepts.AddRange(menuItemConcepts);
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
        /// Function for delete the menu item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var ismenuItemRef = (from menuMItems in _context.Emenu_MenuMItems
                                 join menu in _context.Emenu_MenuItems on menuMItems.EMenusId equals menu.Id
                                 where menuMItems.EMenuItemsId == id && menu.IsDeleted != true && menu.CreatedBy == loginUserId
                                 select menuMItems.EMenuItemsId
                                ).FirstOrDefault();
            if (ismenuItemRef == 0)
            {
                var menuItem = _context.Emenu_MenuItems.Where(x => x.Id == id && x.IsDeleted == false && x.CreatedBy == loginUserId).FirstOrDefault();
                if (menuItem != null)
                {
                    menuItem.IsDeleted = true;
                    menuItem.DeletedOn = DateTime.Now;
                    menuItem.DeletedBy = loginUserId;
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
        /// Function for get menu item by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public MenuItem_VM Get(int id, int loginUserId, bool isAdmin)
        {
            var result = (from menuItem in _context.Emenu_MenuItems
                          where menuItem.Id == id && menuItem.IsDeleted == false
                          select new MenuItem_VM
                          {
                              Id = menuItem.Id,
                              PLU = menuItem.PLU,
                              Name = menuItem.Name,
                              CategoryId = menuItem.CategoryId,
                              CategoryName = _context.Emenu_Category.Where(x => x.Id == menuItem.CategoryId).Select(x => x.Name).FirstOrDefault(),
                              ItemTagsId = _context.Emenu_MenuItemTags.Where(x => x.EMenuItemId == id).Select(x => x.ItemTagId).ToList(),
                              CurrencyId = menuItem.CurrencyId,
                              Price = menuItem.Price,
                              DetailsEN = menuItem.DetailsEN,
                              DetailsAR = menuItem.DetailsAR,
                              LargeDetailsEN = menuItem.LargeDetailsEN,
                              LargeDetailsAR = menuItem.LargeDetailsAR,
                              OverlayDetailsEN = menuItem.OverlayDetailsEN,
                              OverlayDetailsAR = menuItem.OverlayDetailsAR,
                              ThumbnailImageName = menuItem.ThumbnailImageName,
                              OverLayImageName = menuItem.OverLayImageName,
                              LargeImageName = menuItem.LargeImageName,
                              Status = menuItem.Status,
                              CreatedBy = menuItem.CreatedBy,
                              ConceptIds = _context.Emenu_MenuItemsConcepts.Where(x => x.MenuItemId == id).Select(x => x.ConceptId).ToList(),
                              ItemTags = (from sMenuItemTags in _context.Emenu_MenuItemTags // selected menu item tags
                                          join itemTag in _context.Emenu_ItemTags on sMenuItemTags.ItemTagId equals itemTag.Id // all menu item, tags
                                          where sMenuItemTags.EMenuItemId == menuItem.Id && !itemTag.IsDeleted && itemTag.Status
                                          select new ItemTag_VM
                                          {
                                              Id = itemTag.Id,
                                              Name = itemTag.Name,
                                              Status = itemTag.Status
                                          }).ToList(),
                              CommentsEN = menuItem.CommentsEN,
                              CommentsAR = menuItem.CommentsAR,
                              LabelEN = menuItem.LabelEN,
                              LabelAR = menuItem.LabelAR
                          }).FirstOrDefault();

            if (!isAdmin && result != null)
            {
                result = result.CreatedBy == loginUserId ? result : null;
            }
            return result;
        }

        /// <summary>
        /// Function for get list of menu item
        /// </summary>
        /// <param name="isAdmin"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public List<MenuItem_VM> GetList(int loginUserId, bool isAdmin)
        {
            try
            {
                var menuItems = (from menuItem in _context.Emenu_MenuItems
                                 join category in _context.Emenu_Category on menuItem.CategoryId equals category.Id
                                 join currency in _context.Emenu_Currency on menuItem.CurrencyId equals currency.Id
                                 where menuItem.IsDeleted == false
                                 select new MenuItem_VM
                                 {
                                     Id = menuItem.Id,
                                     PLU = menuItem.PLU != null ? menuItem.PLU : "",
                                     Name = menuItem.Name,
                                     CategoryId = category.Id,
                                     CategoryName = category.Name != null ? category.Name : "",
                                     CurrencyId = menuItem.CurrencyId,
                                     Currency = currency.Currency,
                                     Price = menuItem.Price,
                                     Status = menuItem.Status,
                                     //ConceptIds = menuItem.ConceptId != null && menuItem.ConceptId != "" ? Helper.StringToList(menuItem.ConceptId) : new List<string>(),
                                     CreatedBy = menuItem.CreatedBy,
                                     LabelEN = menuItem.LabelEN != null ? menuItem.LabelEN : "",
                                     LabelAR = menuItem.LabelAR != null ? menuItem.LabelAR : "",
                                     MenuItemConcepts = (from menuItemConcepts in _context.Emenu_MenuItemsConcepts
                                                         join concept in _context.Set_Concepts on menuItemConcepts.ConceptId equals concept.Id
                                                         where menuItemConcepts.MenuItemId == menuItem.Id
                                                         select new MenuItemConcepts_VM
                                                         {
                                                             ConceptId = concept.Id,
                                                             ConceptName = concept.ConceptName
                                                         }).ToList(),

                                     ConceptIds = (from menuItemConcepts in _context.Emenu_MenuItemsConcepts
                                                   join concept in _context.Set_Concepts on menuItemConcepts.ConceptId equals concept.Id
                                                   where menuItemConcepts.MenuItemId == menuItem.Id
                                                   select concept.Id).ToList()
                                 }).ToList();

                //get clietid of logged in user.
                var ClientId = _context.Sec_Users.Where(c => c.Id == loginUserId).Select(c => c.ClientId).FirstOrDefault();


                //if (isAdmin)
                //{
                //    var allSubUsers = _context.Sec_Users.Where(c => c.ClientId == ClientId).Select(c => c.Id).ToList();
                //    //display all the items created by this user and the users under it.
                //    menuItems = menuItems.Where(x => x.CreatedBy == loginUserId || allSubUsers.Contains(x.CreatedBy)).ToList();
                //}
                //else
                //{
                //check the concepts accessible to this user.
                var StoreIds = _context.Sec_UserStores.Where(c => c.UserId == loginUserId).Select(c => c.StoreId).ToList();
                var UserConceptIds = _context.Set_StoresConcepts.Where(c => StoreIds.Contains(c.StoreId)).Select(c => c.ConceptId).ToList();

                //filter records for this concepts only.
                menuItems = menuItems.Where(c => c.ConceptIds.Intersect(UserConceptIds).Any() == true).Select(c => c).ToList();
                //}
                return menuItems;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Function for check unique menu-item name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public bool IsUniqueMenuItemName(int id, string name, List<int> conceptIds, int loginUserId, bool isAdmin)
        {
            var ret = false;
            var result = (from menuItem in _context.Emenu_MenuItems
                          join menuItemsConcepts in _context.Emenu_MenuItemsConcepts on menuItem.Id equals menuItemsConcepts.MenuItemId
                          where menuItem.IsDeleted != true
                          && menuItem.Name == name && conceptIds.Contains(menuItemsConcepts.ConceptId)
                          select new
                          {
                              Id = menuItem.Id,
                              CreatedBy = menuItem.CreatedBy
                          }).ToList();
            if (!isAdmin)
            {
                result = result.Where(x => x.CreatedBy == loginUserId).ToList();
            }

            if (result.Count > 0)
            {
                if (id > 0)
                {
                    var isSameId = result.Where(x => x.Id == id).FirstOrDefault();
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
        /// Function for check unique menu-item PLU
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="conceptId"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public bool IsUniqueMenuItemPLU(int id, string PLU, List<int> conceptIds, int loginUserId, bool isAdmin)
        {
            var ret = false;
            var result = (from menuItem in _context.Emenu_MenuItems
                          join menuItemsConcepts in _context.Emenu_MenuItemsConcepts on menuItem.Id equals menuItemsConcepts.MenuItemId
                          where menuItem.IsDeleted != true
                          && menuItem.PLU == PLU && conceptIds.Contains(menuItemsConcepts.ConceptId)
                          select new
                          {
                              Id = menuItem.Id,
                              CreatedBy = menuItem.CreatedBy
                          }).ToList();

            if (!isAdmin)
            {
                result = result.Where(x => x.CreatedBy == loginUserId).ToList();
            }

            if (result.Count > 0)
            {
                if (id > 0)
                {
                    var isSameId = result.Where(x => x.Id == id).FirstOrDefault();
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
        /// Function for update menu-item
        /// </summary>
        /// <param name="menuItem_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(MenuItem_VM menuItem_VM, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var menuItem = _context.Emenu_MenuItems.Where(x => x.Id == menuItem_VM.Id && x.IsDeleted == false).FirstOrDefault();
                    if (menuItem != null)
                    {
                        menuItem.PLU = menuItem_VM.PLU;
                        menuItem.Name = menuItem_VM.Name;
                        menuItem.CategoryId = menuItem_VM.CategoryId;
                        menuItem.CurrencyId = menuItem_VM.CurrencyId;
                        menuItem.Price = menuItem_VM.Price.Value;
                        menuItem.Status = menuItem_VM.Status;
                        menuItem.DetailsEN = menuItem_VM.DetailsEN;
                        menuItem.DetailsAR = menuItem_VM.DetailsAR;
                        menuItem.ThumbnailImageName = menuItem_VM.ThumbnailImageName;
                        menuItem.LargeImageName = menuItem_VM.LargeImageName;
                        menuItem.LargeDetailsEN = menuItem_VM.LargeDetailsEN;
                        menuItem.LargeDetailsAR = menuItem_VM.LargeDetailsAR;
                        menuItem.OverLayImageName = menuItem_VM.OverLayImageName;
                        menuItem.OverlayDetailsEN = menuItem_VM.OverlayDetailsEN;
                        menuItem.OverlayDetailsAR = menuItem_VM.OverlayDetailsAR;
                        menuItem.ModifiedOn = DateTime.Now;
                        menuItem.ModifiedBy = loginUserId;
                        menuItem.CommentsEN = menuItem_VM.CommentsEN;
                        menuItem.CommentsAR = menuItem_VM.CommentsAR;
                        menuItem.LabelEN = menuItem_VM.LabelEN;
                        menuItem.LabelAR = menuItem_VM.LabelAR;
                        _context.SaveChanges();

                        // Delete old MenuItemTags
                        var oldMenuItemTag = _context.Emenu_MenuItemTags.Where(x => x.EMenuItemId == menuItem_VM.Id).ToList();
                        if (oldMenuItemTag.Any())
                        {
                            _context.Emenu_MenuItemTags.RemoveRange(oldMenuItemTag);
                            _context.SaveChanges();
                        }

                        // Add new MenuItemTags
                        var menuItemTags = new List<Emenu_MenuItemTags>();
                        foreach (var itemTagId in menuItem_VM.ItemTagsId)
                        {
                            var menuItemTag = new Emenu_MenuItemTags()
                            {
                                EMenuItemId = menuItem.Id,
                                ItemTagId = itemTagId,
                            };
                            menuItemTags.Add(menuItemTag);
                        }
                        _context.Emenu_MenuItemTags.AddRange(menuItemTags);
                        _context.SaveChanges();


                        //Remove old concept
                        var oldMenuItemConcepts = _context.Emenu_MenuItemsConcepts.Where(x => x.MenuItemId == menuItem_VM.Id).ToList();
                        if (oldMenuItemConcepts.Any())
                        {
                            _context.Emenu_MenuItemsConcepts.RemoveRange(oldMenuItemConcepts);
                            _context.SaveChanges();
                        }

                        //Add new concept
                        var menuItemConcepts = new List<Emenu_MenuItemsConcepts>();
                        foreach (var conceptId in menuItem_VM.ConceptIds)
                        {
                            var menuItemConcept = new Emenu_MenuItemsConcepts();
                            menuItemConcept.MenuItemId = menuItem.Id;
                            menuItemConcept.ConceptId = conceptId;
                            menuItemConcepts.Add(menuItemConcept);
                        }

                        _context.Emenu_MenuItemsConcepts.AddRange(menuItemConcepts);
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
        /// Get Menu Item For VoucherSetup
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public List<MenuItem_VM> GetMenuItemForVoucherSetUp(int loginUserId, int clientId)
        {
            var menuItems = (from userStore in _context.Sec_UserStores
                            join concept in _context.Set_Concepts.Where(x => x.ClientId == clientId) on userStore.ClientId equals concept.ClientId
                            join menuItemConcept in _context.Emenu_MenuItemsConcepts on concept.Id equals menuItemConcept.ConceptId
                            join menuItem in _context.Emenu_MenuItems.Where(x => x.IsDeleted != true && x.Status) on menuItemConcept.MenuItemId equals menuItem.Id
                            where userStore.UserId == loginUserId
                            select new MenuItem_VM
                            {
                                StoreId = userStore.StoreId,
                                Id = menuItem.Id,
                                Name = menuItem.Name,
                            }).DistinctBy(x=>x.Id).ToList();
            return menuItems;
        }
    }
}
