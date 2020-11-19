using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository.Interface
{
    public interface IItemTagRepository
    {
        public List<ItemTag_VM> GetList(int loginUserId, bool isAdmin);
        public ItemTag_VM Get(int id, int loginUserId, bool isAdmin);
        public int Add(ItemTag_VM itemTag_VM, int loginUserId);
        public int Update(ItemTag_VM itemTag_VM, int loginUserId);
        public int Delete(int id, int loginUserId);
        public bool IsUniqueItemTagName(int id, string name, List<int> conceptIds, int loginUserId, bool isAdmin);
    }
}
