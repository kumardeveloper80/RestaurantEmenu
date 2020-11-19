using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository.Interface
{
    public interface IMenuItemRepository
    {
        public List<MenuItem_VM> GetList(int loginUserId, bool isAdmin);
        public MenuItem_VM Get(int id, int loginUserId, bool isAdmin);
        public int Add(MenuItem_VM menuItem_VM, int loginUserId);
        public int Update(MenuItem_VM menuItem_VM, int loginUserId);
        public int Delete(int id, int loginUserId);
        public bool IsUniqueMenuItemPLU(int id, string PLU, List<int> conceptIds, int loginUserId, bool isAdmin);
        public bool IsUniqueMenuItemName(int id, string name, List<int> conceptIds, int loginUserId, bool isAdmin);
        public List<MenuItem_VM> GetMenuItemForVoucherSetUp(int loginUserId, int clientId);

    }
}
