using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository.Interface
{
    public interface ICategoryRepository
    {
        public List<Category_VM> GetList(int loginUserId, bool isAdmin);
        public Category_VM Get(int id, int loginUserId, bool isAdmin);
        public int Add(Category_VM category_VM, int loginUserId);
        public int Update(Category_VM category_VM, int loginUserId);
        public int Delete(int id, int loginUserId);
        public bool IsUniqueCategoryName(int id, string name, List<int> conceptIds, int loginUserId, bool isAdmin);
        public bool IsUniqueCategoryCode(int id, string code, List<int> conceptIds, int loginUserId, bool isAdmin);
    }
}
