using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository.Interface
{
    public interface IVoucherSetupRepository
    {
        public List<VoucherSetup_VM> GetList(int loginUserId, bool isAdmin);
        public VoucherSetup_VM Get(int id, int loginUserId, bool isAdmin);
        public int Add(VoucherSetup_VM voucherSetup_VM, int loginUserId);
        public int Update(VoucherSetup_VM voucherSetup_VM, int loginUserId);
        public int Delete(int id, int loginUserId);
        public List<VoucherSetup_VM> GetVoucherForIssuance(int loginUserId);
    }
}
