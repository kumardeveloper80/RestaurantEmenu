using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMenuApplication.Repository
{
    public class ItemTagRepository : IItemTagRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;


        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public ItemTagRepository(EMenuDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Function for add item tag
        /// </summary>
        /// <param name="itemTags_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(ItemTag_VM itemTag_VM, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var itemTag = new Emenu_ItemTags();
                    itemTag.Name = itemTag_VM.Name;
                    itemTag.Status = itemTag_VM.Status;
                    itemTag.CreatedOn = DateTime.Now;
                    itemTag.CreatedBy = loginUserId;
                    itemTag.IconTagName = itemTag_VM.IconTagName;
                    itemTag.LabelEN = itemTag_VM.LabelEN;
                    itemTag.LabelAR = itemTag_VM.LabelAR;
                    _context.Emenu_ItemTags.Add(itemTag);
                    _context.SaveChanges();

                    var itemTagConcepts = new List<Emenu_ItemTagsConcepts>();
                    foreach (var conceptId in itemTag_VM.ConceptIds)
                    {
                        var itemTagConcept = new Emenu_ItemTagsConcepts();
                        itemTagConcept.ItemTagId = itemTag.Id;
                        itemTagConcept.ConceptId = conceptId;
                        itemTagConcepts.Add(itemTagConcept);
                    }

                    _context.Emenu_ItemTagsConcepts.AddRange(itemTagConcepts);
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
        /// Function for delete item tag
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var isItemTagRef = (from menuItemTags in _context.Emenu_MenuItemTags
                                join menuItem in _context.Emenu_MenuItems on menuItemTags.EMenuItemId equals menuItem.Id
                                where menuItemTags.ItemTagId == id && menuItem.IsDeleted != true && menuItem.CreatedBy == loginUserId
                                select menuItemTags.ItemTagId
                                ).FirstOrDefault();
            if (isItemTagRef == 0)
            {
                var itemTag = _context.Emenu_ItemTags.Where(x => x.Id == id && x.IsDeleted == false && x.CreatedBy == loginUserId).FirstOrDefault();
                if (itemTag != null)
                {
                    itemTag.IsDeleted = true;
                    itemTag.DeletedOn = DateTime.Now;
                    itemTag.DeletedBy = loginUserId;
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
        /// Function for get item tag by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public ItemTag_VM Get(int id, int loginUserId, bool isAdmin)
        {
            var itemTag = (from c in _context.Emenu_ItemTags
                           where c.IsDeleted == false && c.Id == id
                           select new ItemTag_VM
                           {
                               Id = c.Id,
                               Name = c.Name,
                               Status = c.Status,
                               CreatedBy = c.CreatedBy,
                               LabelAR = c.LabelAR,
                               LabelEN = c.LabelEN,
                               IconTagName = c.IconTagName,
                               ConceptIds = _context.Emenu_ItemTagsConcepts.Where(x => x.ItemTagId == id).Select(x => x.ConceptId).ToList()
                           }).FirstOrDefault();
            if (!isAdmin && itemTag != null)
            {
                itemTag = itemTag.CreatedBy == loginUserId ? itemTag : null;
            }
            return itemTag;
        }

        /// <summary>
        /// Function for get item tag list
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public List<ItemTag_VM> GetList(int loginUserId, bool isAdmin)
        {
            var itemTagConceptList = _context.Emenu_ItemTagsConcepts.ToList();
            var setConcepts = _context.Set_Concepts.ToList();
            //var conceptList = _context.Set_Concepts.AsEnumerable();

            List<ItemTag_VM> itemTag_VMs = new List<ItemTag_VM>();

            itemTag_VMs = (from itemTag in _context.Emenu_ItemTags
                           where itemTag.IsDeleted == false
                           select new ItemTag_VM
                           {
                               Id = itemTag.Id,
                               Name = itemTag.Name,
                               LabelAR = itemTag.LabelAR,
                               LabelEN = itemTag.LabelEN,
                               Status = itemTag.Status,
                               CreatedBy = itemTag.CreatedBy,
                               ItemTagsConcepts = (from itemTagConcept in _context.Emenu_ItemTagsConcepts
                                                   join concept in _context.Set_Concepts on itemTagConcept.ConceptId equals concept.Id
                                                   where itemTagConcept.ItemTagId == itemTag.Id
                                                   select new Concepts_VM
                                                   {
                                                       Id = concept.Id,
                                                       ConceptName = concept.ConceptName
                                                   }).ToList(),

                               ConceptIds = (from itemTagConcept in _context.Emenu_ItemTagsConcepts
                                             join concept in _context.Set_Concepts on itemTagConcept.ConceptId equals concept.Id
                                             where itemTagConcept.ItemTagId == itemTag.Id
                                             select concept.Id).ToList(),


                               IconTagName = itemTag.IconTagName != null ? Helper.GetImagePath(itemTag.IconTagName) : "",
                           }).ToList();





            //get clietid of logged in user.
            var ClientId = _context.Sec_Users.Where(c => c.Id == loginUserId).Select(c => c.ClientId).FirstOrDefault();


            //if (isAdmin)
            //{
            //    var allSubUsers = _context.Sec_Users.Where(c => c.ClientId == ClientId).Select(c => c.Id).ToList();
            //    //display all the items created by this user and the users under it.
            //    itemTag_VMs = itemTag_VMs.Where(x => x.CreatedBy == loginUserId || allSubUsers.Contains(x.CreatedBy)).ToList();
            //}
            //else
            //{
            //check the concepts accessible to this user.
            var StoreIds = _context.Sec_UserStores.Where(c => c.UserId == loginUserId).Select(c => c.StoreId).ToList();
            var UserConceptIds = _context.Set_StoresConcepts.Where(c => StoreIds.Contains(c.StoreId)).Select(c => c.ConceptId).ToList();

            //filter records for this concepts only.
            itemTag_VMs = itemTag_VMs.Where(c => c.ConceptIds.Intersect(UserConceptIds).Any() == true).Select(c => c).ToList();
            //}
            return itemTag_VMs;
        }

        /// <summary>
        /// Function for check unique item tag name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="conceptId"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public bool IsUniqueItemTagName(int id, string name, List<int> conceptIds, int loginUserId, bool isAdmin)
        {
            var ret = false;
            var result = (from itemTag in _context.Emenu_ItemTags
                          join itemtagConcept in _context.Emenu_ItemTagsConcepts on itemTag.Id equals itemtagConcept.ItemTagId
                          where itemTag.IsDeleted != true
                          && itemTag.Name == name && conceptIds.Contains(itemtagConcept.ConceptId)
                          select new
                          {
                              Id = itemTag.Id,
                              CreatedBy = itemTag.CreatedBy
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
        /// Function for update item tag
        /// </summary>
        /// <param name="itemTag_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(ItemTag_VM itemTag_VM, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var itemTag = _context.Emenu_ItemTags.Where(x => x.Id == itemTag_VM.Id && x.IsDeleted == false).FirstOrDefault();
                    if (itemTag != null)
                    {
                        itemTag.Name = itemTag_VM.Name;
                        itemTag.Status = itemTag_VM.Status;
                        itemTag.ModifiedBy = loginUserId;
                        itemTag.ModifiedOn = DateTime.Now;
                        itemTag.IconTagName = itemTag_VM.IconTagName;
                        itemTag.LabelEN = itemTag_VM.LabelEN;
                        itemTag.LabelAR = itemTag_VM.LabelAR;
                        _context.SaveChanges();

                        //Remove old
                        var oldItemTagConcepts = _context.Emenu_ItemTagsConcepts.Where(x => x.ItemTagId == itemTag_VM.Id).ToList();
                        if (oldItemTagConcepts.Any())
                        {
                            _context.Emenu_ItemTagsConcepts.RemoveRange(oldItemTagConcepts);
                            _context.SaveChanges();
                        }

                        //Add new
                        var itemTagConcepts = new List<Emenu_ItemTagsConcepts>();
                        foreach (var conceptId in itemTag_VM.ConceptIds)
                        {
                            var itemTagConcept = new Emenu_ItemTagsConcepts();
                            itemTagConcept.ItemTagId = itemTag.Id;
                            itemTagConcept.ConceptId = conceptId;
                            itemTagConcepts.Add(itemTagConcept);
                        }

                        _context.Emenu_ItemTagsConcepts.AddRange(itemTagConcepts);
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
    }
}
