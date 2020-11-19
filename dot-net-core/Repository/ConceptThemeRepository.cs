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
    public class ConceptThemeRepository : IConceptThemeRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public ConceptThemeRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function for add concept theme
        /// </summary>
        /// <param name="theme_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(ConceptTheme_VM conceptTheme_VM, int loginUserId)
        {
            var conceptTheme = new Concept_Theme();
            conceptTheme.ConceptId = conceptTheme_VM.ConceptId;
            conceptTheme.LogoName = conceptTheme_VM.LogoName;
            conceptTheme.FeedBackIconName = conceptTheme_VM.FeedBackIconName;
            conceptTheme.ColorCode = conceptTheme_VM.ColorCode;
            conceptTheme.Status = conceptTheme_VM.Status;
            conceptTheme.CreatedOn = DateTime.Now;
            conceptTheme.CreatedBy = loginUserId;
            _context.Concept_Theme.Add(conceptTheme);
            return _context.SaveChanges();
        }

        /// <summary>
        /// Function for delete concept theme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Delete(int ConceptId, int loginUserId)
        {
            var isValidConcept = Get(ConceptId, loginUserId);
            if (isValidConcept != null)
            {
                var conceptTheme = _context.Concept_Theme.Where(x => x.ConceptId == ConceptId && !x.IsDeleted).FirstOrDefault();
                conceptTheme.IsDeleted = true;
                conceptTheme.DeletedOn = DateTime.Now;
                conceptTheme.DeletedBy = loginUserId;
                return _context.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// Get concept theme by ConceptId
        /// </summary>
        /// <param name="ConceptId"></param>
        /// <returns></returns>
        public ConceptTheme_VM Get(int ConceptId, int loginUserId)
        {
            var result = (from userStores in _context.Sec_UserStores
                          join storesConcepts in _context.Set_StoresConcepts on userStores.StoreId equals storesConcepts.StoreId
                          join concepts in _context.Set_Concepts on storesConcepts.ConceptId equals concepts.Id
                          join conceptTheme in _context.Concept_Theme on concepts.Id equals conceptTheme.ConceptId
                          where userStores.UserId == loginUserId && conceptTheme.IsDeleted != true && concepts.IsDeleted != true
                          && userStores.Status && userStores.IsDeleted != true && concepts.Active && conceptTheme.ConceptId == ConceptId
                          select new ConceptTheme_VM()
                          {
                              Id = conceptTheme.Id,
                              ConceptId = concepts.Id,
                              ConceptName = concepts.ConceptName,
                              ColorCode = conceptTheme != null ? conceptTheme.ColorCode : "-",
                              LogoName = conceptTheme != null ? conceptTheme.LogoName : "-",
                              FeedBackIconName = conceptTheme != null ? conceptTheme.FeedBackIconName : "-",
                              Status = conceptTheme.Status
                          }).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Function for get concept theme list
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public List<ConceptTheme_VM> GetList(int loginUserId)
        {
            var themeList = (from userStores in _context.Sec_UserStores
                             join storesConcepts in _context.Set_StoresConcepts on userStores.StoreId equals storesConcepts.StoreId
                             join concepts in _context.Set_Concepts on storesConcepts.ConceptId equals concepts.Id
                             join conceptTheme in _context.Concept_Theme on concepts.Id equals conceptTheme.ConceptId
                             where userStores.UserId == loginUserId && !conceptTheme.IsDeleted && concepts.IsDeleted != true
                             && userStores.Status && userStores.IsDeleted != true && concepts.Active
                             select new ConceptTheme_VM()
                             {
                                 Id = conceptTheme.Id,
                                 ConceptId = concepts.Id,
                                 ConceptName = concepts.ConceptName,
                                 ColorCode = conceptTheme.ColorCode != null ? conceptTheme.ColorCode : "-",
                                 LogoName = conceptTheme.LogoName != null ? Helper.GetImagePath(conceptTheme.LogoName) : "-",
                                 FeedBackIconName = conceptTheme.FeedBackIconName != null ? Helper.GetImagePath(conceptTheme.FeedBackIconName) : "-"
                             }).Distinct().ToList();
            return themeList;
        }

        /// <summary>
        /// Function for update concept theme
        /// </summary>
        /// <param name="theme_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(ConceptTheme_VM conceptTheme_VM, int loginUserId)
        {
            var conceptTheme = _context.Concept_Theme.Where(x => x.ConceptId == conceptTheme_VM.ConceptId && !x.IsDeleted).FirstOrDefault();
            if (conceptTheme != null)
            {
                conceptTheme.LogoName = conceptTheme_VM.LogoName;
                conceptTheme.FeedBackIconName = conceptTheme_VM.FeedBackIconName;
                conceptTheme.ColorCode = conceptTheme_VM.ColorCode;
                conceptTheme.Status = conceptTheme_VM.Status;
                conceptTheme.ModifiedBy = loginUserId;
                conceptTheme.ModifiedOn = DateTime.Now;
                return _context.SaveChanges();
            }
            return 0;
        }
    }
}
