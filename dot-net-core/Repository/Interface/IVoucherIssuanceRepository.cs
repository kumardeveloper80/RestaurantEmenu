using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository.Interface
{
    public interface IVoucherIssuanceRepository
    {
        public List<VoucherIssuance_VM> GetList(int loginUserId, bool isAdmin);
        public VoucherIssuance_VM Get(int id, int loginUserId, bool isAdmin);
        public int Add(VoucherIssuance_VM voucherIssuance_VM, int loginUserId);
        public int Update(VoucherIssuance_VM voucherIssuance_VM, int loginUserId);
        public int Delete(int id, int loginUserId);
        public int SetApproveStatus(VoucherIssuance_VM voucherIssuance_VM, int loginUserId);
        public VoucherIssuance_VM GetVoucherApproveStatus(int id);
    }
}
