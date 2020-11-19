using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository.Interface
{
    public interface IMenuRepository
    {
        public List<Menu_VM> GetList(int loginUserId, bool isAdmin);
        public Menu_VM Get(int id, int loginUserId, bool isAdmin);
        public int Add(Menu_VM menu_VM, int loginUserId);
        public int Update(Menu_VM menu_VM, int loginUserId);
        public int Delete(int id, int loginUserId);
        public bool IsUniqueMenuCode(int id, string code, int conceptId, int loginUserId, bool isAdmin);
        public bool IsUniqueMenuName(int id, string name, int conceptId, int loginUserId, bool isAdmin);
        public List<CategorySequence_VM> GetMenuCategory(int menuId, int loginUserId, bool isAdmin);
        public int ManageCategorySequence(List<CategorySequence_VM> categorySequence_VMs);
        public List<ItemSequence_VM> GetMenuItem(int menuId, int categoryId, int loginUserId, bool isAdmin);
        public int ManageMenuItemSequence(List<ItemSequence_VM> itemSequence_VMs);
    }
}
