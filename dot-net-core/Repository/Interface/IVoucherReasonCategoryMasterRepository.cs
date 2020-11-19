using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository.Interface
{
    public interface IVoucherReasonCategoryMasterRepository
    {
        public List<ReasonCategoryMaster_VM> GetList();
        public ReasonCategoryMaster_VM Get(int id);
        public int Delete(int id, int loginUserId);
        public int Add(ReasonCategoryMaster_VM reasonCategoryMaster_VM, int loginUserId);
        public int Update(ReasonCategoryMaster_VM reasonCategoryMaster_VM, int loginUserId);
    }
}
