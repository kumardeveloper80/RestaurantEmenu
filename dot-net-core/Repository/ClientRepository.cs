using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMenuApplication.Repository
{
    public class ClientRepository : IClientRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public ClientRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function for add client
        /// </summary>
        /// <param name="client_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(Client_VM client_VM, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var client = new Sec_Client();
                    client.FirstName = client_VM.FirstName;
                    client.LastName = client_VM.LastName;
                    client.CompanyName = client_VM.CompanyName;
                    client.Address1 = client_VM.Address1;
                    client.Address2 = client_VM.Address2;
                    client.EmailAddress = client_VM.EmailAddress;
                    client.PhoneNo = client_VM.PhoneNo;
                    client.Status = client_VM.Status;
                    client.CreatedOn = DateTime.Now;
                    client.CreatedBy = loginUserId;
                    _context.Sec_Client.Add(client);
                    _context.SaveChanges();
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
        /// Function for delete client
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var client = _context.Sec_Client.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
            if (client != null)
            {
                client.IsDeleted = true;
                client.DeletedOn = DateTime.Now;
                client.DeletedBy = loginUserId;
                return _context.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// Function for get client name by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public Client_VM Get(int id, int loginUserId)
        {
            var client = (from c in _context.Sec_Client
                          where c.IsDeleted == false && c.Id == id
                          select new Client_VM
                          {
                              Id = c.Id,
                              FirstName = c.FirstName,
                              LastName = c.LastName,
                              CompanyName = c.CompanyName,
                              Address1 = c.Address1,
                              Address2 = c.Address2,
                              EmailAddress = c.EmailAddress,
                              PhoneNo = c.PhoneNo,
                              Status = c.Status,
                          }).FirstOrDefault();
            return client;
        }

        /// <summary>
        /// Function for get client list
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public List<Client_VM> GetList(int loginUserId)
        {
            var client = (from c in _context.Sec_Client
                          where c.IsDeleted == false
                          select new Client_VM
                          {
                              Id = c.Id,
                              FirstName = c.FirstName,
                              LastName = c.LastName,
                              CompanyName = c.CompanyName,
                              Address1 = c.Address1,
                              Address2 = c.Address2 != null ? c.Address2 : "",
                              EmailAddress = c.EmailAddress,
                              PhoneNo = c.PhoneNo,
                              Status = c.Status,
                          }).ToList();
            return client;
        }

        /// <summary>
        /// Function for check unique company name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyName"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public bool IsUniqueCompanyName(int id, string companyName, int loginUserId)
        {
            var ret = false;
            var result = _context.Sec_Client.Where(x => x.IsDeleted != true && x.CompanyName == companyName).FirstOrDefault();
            if (result != null)
            {
                if (id > 0 && result.Id == id)
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
        /// Function for check unique email
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public bool IsUniqueUniqueEmail(int id, string email, int loginUserId)
        {
            var ret = false;
            var result = _context.Sec_Client.Where(x => x.IsDeleted != true && x.EmailAddress == email).FirstOrDefault();
            if (result != null)
            {
                if (id > 0 && result.Id == id)
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
        /// Fucntion for update client
        /// </summary>
        /// <param name="client_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(Client_VM client_VM, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var client = _context.Sec_Client.Where(x => x.Id == client_VM.Id && x.IsDeleted == false).FirstOrDefault();
                    if (client != null)
                    {
                        client.FirstName = client_VM.FirstName;
                        client.LastName = client_VM.LastName;
                        client.CompanyName = client_VM.CompanyName;
                        client.Address1 = client_VM.Address1;
                        client.Address2 = client_VM.Address2;
                        client.EmailAddress = client_VM.EmailAddress;
                        client.PhoneNo = client_VM.PhoneNo;
                        client.Status = client_VM.Status;
                        client.ModifiedBy = loginUserId;
                        client.ModifiedOn = DateTime.Now;
                        _context.SaveChanges();
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
    }
}
