using EMenuApplication.ViewModels;
using System.Collections.Generic;

namespace EMenuApplication.Repository.Interface
{
    public interface IUserRepository
    {
        public Login_VM Login(Login_VM login_VM);
        public List<User_VM> GetList(int loginUserId, bool isSuperAdminLogin);
        public int Add(User_VM user_VM, int loginUserId, bool isSuperAdminLogin);
        public int Update(User_VM user_VM, int loginUserId, bool isSuperAdminLogin);
        public int Delete(int id, int loginUserId);
        public bool IsUniqueEmail(int id, string email);
        public bool IsUniqueUserName(int id, string username);
        public bool IsClientAssign(int id, int clientId);
        public User_VM Get(int id, bool isSuperAdminLogin);
        public Login_VM SuperAdminLogin(Login_VM login_VM);
    }
}
