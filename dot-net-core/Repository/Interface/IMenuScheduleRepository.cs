using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository.Interface
{
    public interface IMenuScheduleRepository
    {
        public List<MenuSchedule_VM> GetList(int loginUserId, string baseURL, bool isAdmin);
        public MenuSchedule_VM Get(int id, int loginUserId, bool isAdmin);
        public int Add(MenuSchedule_VM menuSchedule_VM, int loginUserId);
        public int Update(MenuSchedule_VM menuSchedule_VM, int loginUserId);
        public int Delete(int id, int loginUserId);
        public bool IsUniqueMenuScheduleCode(int id, string code, int conceptId, int loginUserId, bool isAdmin);
        public MenuSchedule_VM GetMenuByUniqueCode(string lang,string code);
    }
}
