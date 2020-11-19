using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository.Interface
{
    public interface IDashboardRepository 
    {
        public Dashboard_VM GetCounts(int loginUserId, bool isAdmin);
    }
}
