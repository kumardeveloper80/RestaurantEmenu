using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMenuApplication.Repository
{
    public class UserRepository : IUserRepository
    {

        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function for check login details
        /// </summary>
        /// <param name="login_VM"></param>
        /// <returns></returns>
        public Login_VM Login(Login_VM login_VM)
        {
            var result = _context.Sec_Users.Where(x => x.Email == login_VM.Email && x.IsDeleted.Value != true && x.Active.Value).FirstOrDefault();
            if (result != null)
            {
                var encryptPwd = Helper.Encrypt(login_VM.Password);
                if (encryptPwd == result.Password)
                {
                    // check client
                    var client = _context.Sec_Client.Where(x => x.Id == result.ClientId && x.IsDeleted != true && x.Status == true).FirstOrDefault();
                    if (client != null)
                    {
                        login_VM.IsAdmin = result.IsAdmin != null ? result.IsAdmin.Value : false;
                        login_VM.UserId = result.Id;
                        login_VM.FullName = result.FullName;
                        login_VM.ProfilePic = result.ProfilePicture != null ? result.ProfilePicture : "";
                        login_VM.ClientId = result.ClientId;
                        login_VM.IsAllowVoucherApprovalPermission = login_VM.IsAdmin ? true : result.IsAllowVoucherApprovalPermission;
                        login_VM.IsAllowVoucherIssuancePermission = login_VM.IsAdmin ? true : result.IsAllowVoucherIssuancePermission;
                        return login_VM;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Function for get users list
        /// </summary>
        /// <returns></returns>
        public List<User_VM> GetList(int loginUserId, bool isSuperAdminLogin)
        {
            var clientList = _context.Sec_Client.Where(x => x.IsDeleted != true).AsEnumerable();

            var users = (from user in _context.Sec_Users
                         join client in clientList on user.ClientId equals client.Id into c
                         from client in c.DefaultIfEmpty()
                         where user.IsDeleted.Value != true
                         select new User_VM
                         {
                             Id = user.Id,
                             UserName = user.Username != null ? user.Username : "",
                             Email = user.Email != null ? user.Email : "",
                             FullName = user.FullName != null ? user.FullName : "",
                             Phone = user.Phone != null ? user.Phone : "",
                             Active = user.Active != null ? user.Active.Value : false,
                             IsAdmin = user.IsAdmin != null ? user.IsAdmin.Value : false,
                             ClientId = user.ClientId,
                             ClientName = client != null ? client.CompanyName : "",
                             CreatedBy = (int)user.CreatedBy,
                             IsAllowVoucherApprovalPermission = user.IsAllowVoucherApprovalPermission,
                             IsAllowVoucherIssuancePermission = user.IsAllowVoucherIssuancePermission,
                         }).ToList();

            if (users.Any() && isSuperAdminLogin)
            {
                users = users.Where(x => x.IsAdmin).ToList();
            }
            else
            {
                users = users.Where(x => x.IsAdmin != true && x.CreatedBy == loginUserId).ToList();
            }
            return users;
        }

        /// <summary>
        /// Function for delete the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var user = _context.Sec_Users.Where(x => x.Id == id && x.IsDeleted.Value != true).FirstOrDefault();
            if (user != null)
            {
                user.IsDeleted = true;
                user.DeletedOn = DateTime.Now;
                user.DeletedBy = loginUserId;
                return _context.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// Function for check email exists
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsUniqueEmail(int id, string email)
        {
            var ret = false;
            var user = _context.Sec_Users.Where(x => x.Email == email && x.IsDeleted.Value != true).FirstOrDefault();
            if (user != null)
            {
                if (user.IsAdmin.Value == true && user.Id != id)
                {
                    ret = false;
                }
                else if (user != null && id > 0 && user.Id == id) //edit mode
                {
                    ret = true;
                }
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Function for check username exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsUniqueUserName(int id, string username)
        {
            var ret = false;
            var user = _context.Sec_Users.Where(x => x.Username == username && x.IsDeleted.Value != true).FirstOrDefault();
            if (user != null)
            {
                if (user.IsAdmin.Value == true && user.Id != id)
                {
                    ret = false;
                }
                else if (user != null && id > 0 && user.Id == id) //edit mode
                {
                    ret = true;
                }
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Function for add the user
        /// </summary>
        /// <param name="user_VM"></param>
        /// <returns></returns>
        public int Add(User_VM user_VM, int loginUserId, bool isSuperAdminLogin)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var user = new Sec_Users();
                    user.Username = user_VM.UserName;
                    user.Password = Helper.Encrypt(user_VM.Password);
                    user.Email = user_VM.Email;
                    user.FullName = user_VM.FullName;
                    user.Phone = user_VM.Phone;
                    user.Lock = true;
                    user.DefaultModule = 0;
                    user.DefaultPage = 0;
                    user.LockProfile = true;
                    user.UnlockProfile = true;
                    user.CreatedOn = DateTime.Now;
                    user.CreatedBy = loginUserId;
                    user.Active = user_VM.Active;
                    user.CanExportProfiles = true;
                    user.IsAdmin = isSuperAdminLogin;
                    user.ClientId = user_VM.ClientId;
                    user.IsAllowVoucherApprovalPermission = user_VM.IsAllowVoucherApprovalPermission;
                    user.IsAllowVoucherIssuancePermission = user_VM.IsAllowVoucherIssuancePermission;
                    _context.Sec_Users.Add(user);
                    _context.SaveChanges();

                    // Add Stores Persmission

                    // Add Stores Persmission for Admin
                    if (isSuperAdminLogin)
                    {
                        var storesList = _context.Set_Stores.Where(x => x.IsDeleted != true && x.ClientId == user.ClientId && x.CreatedBy == loginUserId).ToList();
                        if (storesList.Any())
                        {
                            var userStores = new List<Sec_UserStores>();
                            foreach (var store in storesList)
                            {
                                var userStore = new Sec_UserStores();
                                userStore.UserId = user.Id;
                                userStore.Status = store.Status;
                                userStore.StoreId = store.Id;
                                userStore.CreatedBy = loginUserId;
                                userStore.CreatedOn = DateTime.Now;
                                userStore.ClientId = user.ClientId;
                                userStores.Add(userStore);
                            }
                            _context.Sec_UserStores.AddRange(userStores);
                            _context.SaveChanges();
                        }
                    }

                    // Add Stores Persmission for User
                    if (!isSuperAdminLogin)
                    {
                        var userStores = new List<Sec_UserStores>();
                        foreach (var store in user_VM.StoreIds)
                        {
                            var userStore = new Sec_UserStores();
                            userStore.UserId = user.Id;
                            userStore.Status = true;
                            userStore.StoreId = store;
                            userStore.CreatedBy = loginUserId;
                            userStore.CreatedOn = DateTime.Now;
                            userStore.ClientId = user.ClientId;
                            userStores.Add(userStore);
                        }
                        _context.Sec_UserStores.AddRange(userStores);
                        _context.SaveChanges();
                    }

                    dbcxtransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbcxtransaction.Rollback();
                    ret = 0;
                }
            }
            return ret;
        }

        /// <summary>
        /// Function for update the user
        /// </summary>
        /// <param name="user_VM"></param>
        /// <returns></returns>
        public int Update(User_VM user_VM, int loginUserId, bool isSuperAdminLogin)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var user = _context.Sec_Users.Where(x => x.Id == user_VM.Id && x.IsDeleted.Value != true).FirstOrDefault();
                    if (user != null)
                    {
                        user.Username = user_VM.UserName;
                        user.Password = Helper.Encrypt(user_VM.Password);
                        user.Email = user_VM.Email;
                        user.FullName = user_VM.FullName;
                        user.Phone = user_VM.Phone;
                        user.ProfilePicture = user_VM.FileName;
                        user.ModifiedBy = loginUserId;
                        user.ModifiedOn = DateTime.Now;
                        user.Active = user_VM.Active;
                        user.IsAdmin = isSuperAdminLogin ? isSuperAdminLogin : ((bool)user.IsAdmin ? true : false);
                        user.ClientId = user_VM.ClientId;
                        user.IsAllowVoucherApprovalPermission = user_VM.IsAllowVoucherApprovalPermission;
                        user.IsAllowVoucherIssuancePermission = user_VM.IsAllowVoucherIssuancePermission;
                        _context.SaveChanges();

                        // Update Stores Persmission

                        // Add Stores Persmission for Admin
                        if (isSuperAdminLogin)
                        {
                            // remove old store permission
                            var oldUserStores = _context.Sec_UserStores.Where(x => x.UserId == user.Id && x.CreatedBy == loginUserId).ToList();
                            if (oldUserStores.Any())
                            {
                                _context.Sec_UserStores.RemoveRange(oldUserStores);
                                _context.SaveChanges();
                            }

                            // Add Stores Persmission
                            var storesList = _context.Set_Stores.Where(x => x.IsDeleted != true && x.ClientId == user.ClientId && x.CreatedBy == loginUserId).ToList();
                            if (storesList.Any())
                            {
                                var userStores = new List<Sec_UserStores>();
                                foreach (var store in storesList)
                                {
                                    var userStore = new Sec_UserStores();
                                    userStore.UserId = user.Id;
                                    userStore.Status = store.Status;
                                    userStore.StoreId = store.Id;
                                    userStore.CreatedBy = loginUserId;
                                    userStore.CreatedOn = DateTime.Now;
                                    userStore.ClientId = user.ClientId;
                                    userStores.Add(userStore);
                                }
                                _context.Sec_UserStores.AddRange(userStores);
                                _context.SaveChanges();
                            }
                        }

                        // Add Stores Persmission for User
                        if (!isSuperAdminLogin)
                        {
                            // remove old store permission
                            var oldUserStores = _context.Sec_UserStores.Where(x => x.UserId == user.Id && x.CreatedBy == loginUserId).ToList();
                            if (oldUserStores.Any())
                            {
                                _context.Sec_UserStores.RemoveRange(oldUserStores);
                                _context.SaveChanges();
                            }

                            var userStores = new List<Sec_UserStores>();
                            foreach (var store in user_VM.StoreIds)
                            {
                                var userStore = new Sec_UserStores();
                                userStore.UserId = user.Id;
                                userStore.Status = true;
                                userStore.StoreId = store;
                                userStore.CreatedBy = loginUserId;
                                userStore.CreatedOn = DateTime.Now;
                                userStore.ClientId = user.ClientId;
                                userStores.Add(userStore);
                            }
                            _context.Sec_UserStores.AddRange(userStores);
                            _context.SaveChanges();
                        }
                        dbcxtransaction.Commit();
                    }
                    else
                    {
                        ret = 0;
                    }
                }
                catch (Exception ex)
                {
                    dbcxtransaction.Rollback();
                    ret = 0;
                }
            }

            return ret;
        }

        /// <summary>
        /// Function for get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User_VM Get(int id, bool isSuperAdminLogin)
        {
            var user = (from u in _context.Sec_Users
                        where u.IsDeleted.Value != true && u.Id == id
                        select new User_VM
                        {
                            Id = u.Id,
                            UserName = u.Username,
                            Email = u.Email,
                            FullName = u.FullName,
                            Phone = u.Phone,
                            Password = Helper.Decrypt(u.Password),
                            FileName = u.ProfilePicture,
                            Active = u.Active != null ? u.Active.Value : false,
                            IsAdmin = (bool)u.IsAdmin,
                            ClientId = u.ClientId,
                            StoreIds = _context.Sec_UserStores.Where(x => x.UserId == u.Id && x.IsDeleted != true).Select(x => x.StoreId).ToList(),
                            IsAllowVoucherApprovalPermission = u.IsAllowVoucherApprovalPermission,
                            IsAllowVoucherIssuancePermission = u.IsAllowVoucherIssuancePermission,
                        }).FirstOrDefault();

            if (user != null && isSuperAdminLogin)
            {
                user = user.IsAdmin ? user : null;
            }
            else if (user != null && isSuperAdminLogin != true)
            {
                user = user.IsAdmin != true ? user : null;
            }
            return user;
        }

        /// <summary>
        /// Function for Super Admin Login
        /// </summary>
        /// <param name="login_VM"></param>
        /// <returns></returns>
        public Login_VM SuperAdminLogin(Login_VM login_VM)
        {
            var result = _context.Sec_SuperUser.Where(x => x.Email == login_VM.Email && x.IsDeleted != true && x.Status).FirstOrDefault();
            if (result != null)
            {
                var encryptPwd = Helper.Encrypt(login_VM.Password);
                if (encryptPwd == result.Password)
                {
                    login_VM.UserId = result.Id;
                    login_VM.FullName = result.UserName;
                    login_VM.ProfilePic = "";
                    return login_VM;
                }
            }
            return null;
        }

        /// <summary>
        /// Function to check client already assign
        /// </summary>
        /// <param name="id"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public bool IsClientAssign(int id, int clientId)
        {
            var ret = false;
            var user = _context.Sec_Users.Where(x => x.ClientId == clientId && x.IsDeleted.Value != true && x.IsAdmin.Value == true).FirstOrDefault();
            if (user != null)
            {
                if (user != null && id > 0 && user.Id == id)
                {
                    ret = true;
                }
            }
            else
            {
                ret = true;
            }
            return ret;
        }
    }
}
