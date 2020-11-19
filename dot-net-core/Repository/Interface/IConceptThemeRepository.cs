using EMenuApplication.ViewModels;
using System.Collections.Generic;

namespace EMenuApplication.Repository.Interface
{
    public interface IConceptThemeRepository
    {
        public List<ConceptTheme_VM> GetList(int loginUserId);
        public ConceptTheme_VM Get(int Id, int loginUserId);
        public int Add(ConceptTheme_VM conceptTheme_VM, int loginUserId);
        public int Update(ConceptTheme_VM conceptTheme_VM, int loginUserId);
        public int Delete(int id, int loginUserId);
    }
}
