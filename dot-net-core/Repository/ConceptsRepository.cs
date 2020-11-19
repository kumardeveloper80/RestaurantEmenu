using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMenuApplication.Repository
{
    public class ConceptsRepository : IConceptsRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public ConceptsRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function for get list of concepts
        /// </summary>
        /// <param name="category_VM"></param>
        /// <returns></returns>
        public List<Concepts_VM> GetConceptsPermissionList(int userId)
        {
            var result = (from userStores in _context.Sec_UserStores
                          join storesConcepts in _context.Set_StoresConcepts on userStores.StoreId equals storesConcepts.StoreId
                          join concepts in _context.Set_Concepts on storesConcepts.ConceptId equals concepts.Id
                          where userStores.UserId == userId && concepts.IsDeleted != true
                          && userStores.Status && userStores.IsDeleted != true && concepts.Active
                          select new Concepts_VM
                          {
                              Id = concepts.Id,
                              ConceptName = concepts.ConceptName
                          }).Distinct().ToList();

            return result;
        }

        /// <summary>
        /// Function for add concept
        /// </summary>
        /// <param name="concepts_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(Concepts_VM concepts_VM, int loginUserId)
        {
            var concept = new Set_Concepts();
            concept.ConceptName = concepts_VM.ConceptName;
            concept.Active = concepts_VM.Active;
            concept.ClientId = concepts_VM.ClientId;
            concept.CreatedOn = DateTime.Now;
            concept.CreatedBy = loginUserId;
            _context.Set_Concepts.Add(concept);
            return _context.SaveChanges();
        }

        /// <summary>
        /// Function for delete concept by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var concept = _context.Set_Concepts.Where(x => x.Id == id && !x.IsDeleted && x.CreatedBy == loginUserId).FirstOrDefault();
                    if (concept != null)
                    {
                        // delete item tag concept
                        var itemTagConceptList = _context.Emenu_ItemTagsConcepts.Where(x => x.ConceptId == id).ToList();

                        if (itemTagConceptList.Any())
                        {
                            foreach (var itemTagConcept in itemTagConceptList)
                            {
                                var itemTagConceptListByItemTagId = _context.Emenu_ItemTagsConcepts.Where(x => x.ItemTagId == itemTagConcept.ItemTagId).ToList();
                                if (itemTagConceptListByItemTagId.Any() && itemTagConceptListByItemTagId.Count == 1)
                                {
                                    // if only 1 concept than remove the item tag
                                    var itemTag = _context.Emenu_ItemTags.Where(x => x.Id == itemTagConceptListByItemTagId[0].ItemTagId && x.IsDeleted != true).FirstOrDefault();
                                    if (itemTag != null)
                                    {
                                        itemTag.IsDeleted = true;
                                        itemTag.DeletedBy = loginUserId;
                                        itemTag.DeletedOn = DateTime.Now;
                                        _context.SaveChanges();
                                    }
                                }
                            }

                            _context.Emenu_ItemTagsConcepts.RemoveRange(itemTagConceptList);
                            _context.SaveChanges();
                        }

                        // delete category concept
                        var categoryConceptList = _context.Emenu_CategoryConcepts.Where(x => x.ConceptId == id).ToList();
                        if (categoryConceptList.Any())
                        {
                            foreach (var categoryConcept in categoryConceptList)
                            {
                                var categoryConceptListByItemTagId = _context.Emenu_CategoryConcepts.Where(x => x.CategoryId == categoryConcept.CategoryId).ToList();
                                if (categoryConceptListByItemTagId.Any() && categoryConceptListByItemTagId.Count == 1)
                                {
                                    // if only 1 concept than remove the category
                                    var category = _context.Emenu_Category.Where(x => x.Id == categoryConceptListByItemTagId[0].CategoryId && x.IsDeleted != true).FirstOrDefault();
                                    if (category != null)
                                    {
                                        category.IsDeleted = true;
                                        category.DeletedBy = loginUserId;
                                        category.DeletedOn = DateTime.Now;
                                        _context.SaveChanges();
                                    }
                                }
                            }

                            _context.Emenu_CategoryConcepts.RemoveRange(categoryConceptList);
                            _context.SaveChanges();

                        }

                        // delete menuitem concept
                        var menuItemConceptList = _context.Emenu_MenuItemsConcepts.Where(x => x.ConceptId == id).ToList();
                        if (menuItemConceptList.Any())
                        {
                            foreach (var menuItemConcept in menuItemConceptList)
                            {
                                var menuItemConceptByMenuItemId = _context.Emenu_MenuItemsConcepts.Where(x => x.MenuItemId == menuItemConcept.MenuItemId).ToList();
                                if (menuItemConceptByMenuItemId.Any() && menuItemConceptByMenuItemId.Count == 1)
                                {
                                    // if only 1 concept than remove the menu item
                                    var menuItem = _context.Emenu_MenuItems.Where(x => x.Id == menuItemConceptByMenuItemId[0].MenuItemId && x.IsDeleted != true).FirstOrDefault();
                                    if (menuItem != null)
                                    {
                                        menuItem.IsDeleted = true;
                                        menuItem.DeletedBy = loginUserId;
                                        menuItem.DeletedOn = DateTime.Now;
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            _context.Emenu_MenuItemsConcepts.RemoveRange(menuItemConceptList);
                            _context.SaveChanges();
                        }

                        // delete menu concept
                        var menuList = _context.Emenu_Menus.Where(x => x.ConceptId == id && x.IsDeleted != true).ToList();
                        if (menuList.Any())
                        {
                            foreach (var menu in menuList)
                            {
                                menu.IsDeleted = true;
                                menu.DeletedBy = loginUserId;
                                menu.DeletedOn = DateTime.Now;
                                _context.SaveChanges();
                            }
                        }

                        // delete menu schedule
                        var menuScheduleList = _context.Emenu_MenuSchedules.Where(x => x.ConceptId == id && x.IsDeleted != true).ToList();
                        if (menuScheduleList.Any())
                        {
                            foreach (var menuSchedule in menuScheduleList)
                            {
                                menuSchedule.IsDeleted = true;
                                menuSchedule.DeletedBy = loginUserId;
                                menuSchedule.DeletedOn = DateTime.Now;
                                _context.SaveChanges();
                            }
                        }

                        // delete concept theme
                        var conceptThemeList = _context.Concept_Theme.Where(x => x.ConceptId == id && x.IsDeleted != true).ToList();
                        if (conceptThemeList.Any())
                        {
                            foreach (var conceptTheme in conceptThemeList)
                            {
                                conceptTheme.IsDeleted = true;
                                conceptTheme.DeletedBy = loginUserId;
                                conceptTheme.DeletedOn = DateTime.Now;
                                _context.SaveChanges();
                            }
                        }

                        // delete concept
                        concept.IsDeleted = true;
                        concept.DeletedOn = DateTime.Now;
                        concept.DeletedBy = loginUserId;
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
        /// Function for get concept by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Concepts_VM Get(int id, int loginUserId)
        {
            var result = (from concept in _context.Set_Concepts
                          join client in _context.Sec_Client on concept.ClientId equals client.Id
                          where concept.IsDeleted != true && concept.CreatedBy == loginUserId
                          && client.IsDeleted != true && concept.Id == id
                          select new Concepts_VM
                          {
                              Id = concept.Id,
                              ConceptName = concept.ConceptName,
                              Active = concept.Active,
                              ClientId = concept.ClientId,
                              ClientName = client.CompanyName
                          }).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Function for get concept list
        /// </summary>
        /// <returns></returns>
        public List<Concepts_VM> GetList(int loginUserId)
        {
            var list = (from concept in _context.Set_Concepts
                        join client in _context.Sec_Client on concept.ClientId equals client.Id
                        where concept.IsDeleted != true && concept.CreatedBy == loginUserId
                        && client.IsDeleted != true
                        select new Concepts_VM
                        {
                            Id = concept.Id,
                            ConceptName = concept.ConceptName,
                            Active = concept.Active,
                            ClientId = concept.ClientId,
                            ClientName = client.CompanyName
                        }).ToList();
            return list;
        }

        /// <summary>
        /// Function for check unique concept name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="conceptName"></param>
        /// <returns></returns>
        public bool IsUniqueConceptName(int id, string conceptName, int clientId, int loginUserId)
        {
            var ret = false;
            var obj = _context.Set_Concepts.Where(x => x.ConceptName == conceptName && !x.IsDeleted && x.CreatedBy == loginUserId && x.ClientId == clientId).FirstOrDefault();
            if (obj != null)
            {
                // For edit mode
                if (id > 0 && obj != null && obj.Id == id)
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
        /// Function for update concept
        /// </summary>
        /// <param name="concepts_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(Concepts_VM concepts_VM, int loginUserId)
        {
            var concept = _context.Set_Concepts.Where(x => x.Id == concepts_VM.Id && !x.IsDeleted && x.CreatedBy == loginUserId).FirstOrDefault();
            if (concept != null)
            {
                concept.ConceptName = concepts_VM.ConceptName;
                concept.Active = concepts_VM.Active;
                concept.ClientId = concepts_VM.ClientId;
                concept.ModifiedBy = loginUserId;
                concept.ModifiedOn = DateTime.Now;
                return _context.SaveChanges();
            }
            return 0;
        }
    }
}
