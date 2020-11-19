using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository.Interface
{
    public interface IVoucherReasonSubCategoryMasterRepository
    {
        public List<ReasonSubCategoryMaster_VM> GetList();
        public ReasonSubCategoryMaster_VM Get(int id);
        public int Delete(int id, int loginUserId);
        public int Add(ReasonSubCategoryMaster_VM subCategoryMaster_VM, int loginUserId);
        public int Update(ReasonSubCategoryMaster_VM subCategoryMaster_VM, int loginUserId);
    }
}
