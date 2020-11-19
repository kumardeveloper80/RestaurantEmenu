using EMenuApplication.ViewModels;
using System.Collections.Generic;

namespace EMenuApplication.Repository.Interface
{
    public interface IClientRepository
    {
        public List<Client_VM> GetList(int loginUserId);
        public Client_VM Get(int id, int loginUserId);
        public int Add(Client_VM client_VM, int loginUserId);
        public int Update(Client_VM client_VM, int loginUserId);
        public int Delete(int id, int loginUserId);
        public bool IsUniqueCompanyName(int id, string companyName, int loginUserId);
        public bool IsUniqueUniqueEmail(int id, string email, int loginUserId);
    }
}
