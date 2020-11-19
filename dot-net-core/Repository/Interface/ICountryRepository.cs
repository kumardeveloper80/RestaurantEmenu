using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository.Interface
{
    public interface ICountryRepository
    {
        public List<Country_VM> GetList();
        public Country_VM Get(int id);
        public bool IsUniqueCountryName(int id, string regionName);
        public int Delete(int id, int loginUserId);
        public int Add(Country_VM country_VM, int loginUserId);
        public int Update(Country_VM country_VM, int loginUserId);
    }
}
