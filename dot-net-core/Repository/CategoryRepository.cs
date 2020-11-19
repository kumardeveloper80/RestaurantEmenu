using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMenuApplication.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;


        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public CategoryRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function for add category
        /// </summary>
        /// <param name="category_VM"></param>
        /// <returns></returns>
        public int Add(Category_VM category_VM, int loginUserId)
        {

            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var category = new Emenu_Category();
                    category.Code = category_VM.Code;
                    category.Name = category_VM.Name;
                    category.LabelAR = category_VM.LabelAR;
                    category.LabelEN = category_VM.LabelEN;
                    category.DetailsAR = category_VM.DetailsAR;
                    category.DetailsEN = category_VM.DetailsEN;
                    category.ImageName = category_VM.ImageName;
                    category.Status = category_VM.Status;
                    category.CreatedOn = DateTime.Now;
                    category.CreatedBy = loginUserId;
                    _context.Emenu_Category.Add(category);
                    _context.SaveChanges();

                    var categoryConcepts = new List<Emenu_CategoryConcepts>();
                    foreach (var conceptId in category_VM.ConceptIds)
                    {
                        var categoryConcept = new Emenu_CategoryConcepts();
                        categoryConcept.CategoryId = category.Id;
                        categoryConcept.ConceptId = conceptId;
                        categoryConcepts.Add(categoryConcept);
                    }

                    _context.Emenu_CategoryConcepts.AddRange(categoryConcepts);
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
        /// Function for delete category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var isCategoryRef = (from menuItem in _context.Emenu_MenuItems
                                 where menuItem.CategoryId == id && menuItem.IsDeleted != true && menuItem.CreatedBy == loginUserId
                                 select menuItem.CategoryId
                                ).FirstOrDefault();
            if (isCategoryRef == 0)
            {

                var category = _context.Emenu_Category.Where(x => x.Id == id && x.IsDeleted == false && x.CreatedBy == loginUserId).FirstOrDefault();
                if (category != null)
                {
                    category.IsDeleted = true;
                    category.DeletedOn = DateTime.Now;
                    category.DeletedBy = loginUserId;
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
        /// Function for get category by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public Category_VM Get(int id, int loginUserId, bool isAdmin)
        {
            var category = (from c in _context.Emenu_Category
                            where c.IsDeleted == false && c.Id == id
                            select new Category_VM
                            {
                                Id = c.Id,
                                Code = c.Code,
                                Name = c.Name,
                                LabelAR = c.LabelAR,
                                LabelEN = c.LabelEN,
                                DetailsAR = c.DetailsAR,
                                DetailsEN = c.DetailsEN,
                                ImageName = c.ImageName,
                                Status = c.Status,
                                CreatedBy = c.CreatedBy,
                                ConceptIds = _context.Emenu_CategoryConcepts.Where(x => x.CategoryId == id).Select(x => x.ConceptId).ToList()
                            }).FirstOrDefault();
            if (!isAdmin && category != null)
            {
                category = category.CreatedBy == loginUserId ? category : null;
            }
            return category;
        }

        /// <summary>
        /// Function for get category list
        /// <param name="loginUserId"></param>
        /// </summary>
        /// <returns></returns>
        public List<Category_VM> GetList(int loginUserId, bool isAdmin)
        {
            var categories = (from category in _context.Emenu_Category
                              where category.IsDeleted == false
                              select new Category_VM
                              {
                                  Id = category.Id,
                                  Code = category.Code != null ? category.Code : "",
                                  Name = category.Name != null ? category.Name : "",
                                  LabelAR = category.LabelAR != null ? category.LabelAR : "",
                                  LabelEN = category.LabelEN != null ? category.LabelEN : "",
                                  Status = category.Status,
                                  CreatedBy = category.CreatedBy,
                                  CategoryConcepts = (from categoryConcepts in _context.Emenu_CategoryConcepts
                                                      join concept in _context.Set_Concepts on categoryConcepts.ConceptId equals concept.Id
                                                      where categoryConcepts.CategoryId == category.Id
                                                      select new Concepts_VM
                                                      {
                                                          Id = concept.Id,
                                                          ConceptName = concept.ConceptName
                                                      }).ToList(),

                                  ConceptIds = (from categoryConcepts in _context.Emenu_CategoryConcepts
                                                join concept in _context.Set_Concepts on categoryConcepts.ConceptId equals concept.Id
                                                where categoryConcepts.CategoryId == category.Id
                                                select concept.Id).ToList()
                              }).ToList();

            //get clietid of logged in user.
            var ClientId = _context.Sec_Users.Where(c => c.Id == loginUserId).Select(c => c.ClientId).FirstOrDefault();


            //if (isAdmin)
            //{
            //    var allSubUsers = _context.Sec_Users.Where(c => c.ClientId == ClientId).Select(c => c.Id).ToList();
            //    //display all the items created by this user and the users under it.
            //    categories = categories.Where(x => x.CreatedBy == loginUserId || allSubUsers.Contains(x.CreatedBy)).ToList();
            //}
            //else
            //{
            //check the concepts accessible to this user.
            var StoreIds = _context.Sec_UserStores.Where(c => c.UserId == loginUserId).Select(c => c.StoreId).ToList();
            var UserConceptIds = _context.Set_StoresConcepts.Where(c => StoreIds.Contains(c.StoreId)).Select(c => c.ConceptId).ToList();

            //filter records for this concepts only.
            categories = categories.Where(c => c.ConceptIds.Intersect(UserConceptIds).Any() == true).Select(c => c).ToList();
            //}

            return categories;
        }

        /// <summary>
        /// Function for check unique cateogry name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <param name="loginUserId"></param>
        /// <param name="conceptId"></param>
        /// <returns></returns>
        public bool IsUniqueCategoryName(int id, string name, List<int> conceptIds, int loginUserId, bool isAdmin)
        {
            var ret = false;
            var result = (from category in _context.Emenu_Category
                          join categoryConcepts in _context.Emenu_CategoryConcepts on category.Id equals categoryConcepts.CategoryId
                          where category.IsDeleted != true
                          && category.Name == name && conceptIds.Contains(categoryConcepts.ConceptId)
                          select new
                          {
                              Id = category.Id,
                              CreatedBy = category.CreatedBy
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
        /// Function for check unique cateogry code
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <param name="loginUserId"></param>
        /// <param name="conceptId"></param>
        /// <returns></returns>
        public bool IsUniqueCategoryCode(int id, string code, List<int> conceptIds, int loginUserId, bool isAdmin)
        {
            var ret = false;
            var result = (from category in _context.Emenu_Category
                          join categoryConcepts in _context.Emenu_CategoryConcepts on category.Id equals categoryConcepts.CategoryId
                          where category.IsDeleted != true
                          && category.Code == code && conceptIds.Contains(categoryConcepts.ConceptId)
                          select new
                          {
                              Id = category.Id,
                              CreatedBy = category.CreatedBy
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
        /// Function for update cateogry
        /// </summary>
        /// <param name="category_VM"></param>
        /// <returns></returns>
        public int Update(Category_VM category_VM, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var category = _context.Emenu_Category.Where(x => x.Id == category_VM.Id && x.IsDeleted == false).FirstOrDefault();
                    if (category != null)
                    {
                        category.Code = category_VM.Code;
                        category.Name = category_VM.Name;
                        category.LabelAR = category_VM.LabelAR;
                        category.LabelEN = category_VM.LabelEN;
                        category.DetailsAR = category_VM.DetailsAR;
                        category.DetailsEN = category_VM.DetailsEN;
                        category.ImageName = category_VM.ImageName;
                        category.Status = category_VM.Status;
                        category.ModifiedBy = loginUserId;
                        category.ModifiedOn = DateTime.Now;
                        _context.SaveChanges();

                        //Remove old
                        var oldCategoyConcepts = _context.Emenu_CategoryConcepts.Where(x => x.CategoryId == category_VM.Id).ToList();
                        if (oldCategoyConcepts.Any())
                        {
                            _context.Emenu_CategoryConcepts.RemoveRange(oldCategoyConcepts);
                            _context.SaveChanges();
                        }

                        //Add new
                        var categoryConcepts = new List<Emenu_CategoryConcepts>();
                        foreach (var conceptId in category_VM.ConceptIds)
                        {
                            var categoryConcept = new Emenu_CategoryConcepts();
                            categoryConcept.CategoryId = category.Id;
                            categoryConcept.ConceptId = conceptId;
                            categoryConcepts.Add(categoryConcept);
                        }

                        _context.Emenu_CategoryConcepts.AddRange(categoryConcepts);
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
