using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository.Interface
{
    public interface IStoresRespository
    {
        public List<Store_VM> GetList(int loginUserId);
        public Store_VM Get(int id, int loginUserId);
        public int Add(Store_VM store_VM, int loginUserId);
        public int Update(Store_VM store_VM, int loginUserId);
        public int Delete(int id, int loginUserId);
        public List<Store_VM> GetStoresByUserId(int userId);
        public bool IsUniqueStoreCode(int id, string storeCode, int clientId, int loginUserId);
        public bool IsUniqueStoreName(int id, string storeName, int clientId, int loginUserId);
    }
}
