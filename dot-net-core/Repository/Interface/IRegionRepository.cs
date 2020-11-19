using EMenuApplication.ViewModels;
using System.Collections.Generic;

namespace EMenuApplication.Repository.Interface
{
    public interface IRegionRepository
    {
        public Region_VM Get(int id);
        public List<Region_VM> GetList();
        public bool IsUniqueRegionName(int id, string regionName);
        public int Delete(int id, int loginUserId);
        public int Add(Region_VM region_VM, int loginUserId);
        public int Update(Region_VM region_VM, int loginUserId);
    }
}
