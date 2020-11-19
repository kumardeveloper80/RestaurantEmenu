using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMenuApplication.Repository
{
    public class StoresRespository : IStoresRespository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;


        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public StoresRespository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function for Get store list
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public List<Store_VM> GetList(int loginUserId)
        {
            var clientList = _context.Sec_Client.Where(x => x.IsDeleted != true && x.Status == true).AsEnumerable();
            var storesConceptsList = _context.Set_StoresConcepts.AsEnumerable();
            var regionlist = _context.Set_Region.Where(x => x.IsDeleted != true && x.Status == true).AsEnumerable();
            var countrylist = _context.Set_CountryCodes.Where(x => x.IsDeleted != true && x.Status == true).AsEnumerable();

            var stores = (from store in _context.Set_Stores
                          join client in clientList on store.ClientId equals client.Id
                          join region in regionlist on store.RegionId equals region.Id
                          join country in countrylist on store.CountryCode equals country.CountryID
                          where store.CreatedBy == loginUserId && store.IsDeleted != true
                          select new Store_VM()
                          {
                              Id = store.Id,
                              StoreName = store.StoreName,
                              Status = (bool)store.Status,
                              StoreCode = store.StoreCode != null ? store.StoreCode : string.Empty,
                              RegionId = region.Id,
                              RegionName = region.Region,
                              CountryName = country.Name,
                              ClientName = client.CompanyName,
                              StoreConcepts = (from storesConcepts in _context.Set_StoresConcepts
                                               join concept in _context.Set_Concepts on storesConcepts.ConceptId equals concept.Id
                                               where storesConcepts.StoreId == store.Id
                                               select new Concepts_VM
                                               {
                                                   Id = concept.Id,
                                                   ConceptName = concept.ConceptName
                                               }).ToList(),
                          }).ToList();
            return stores;
        }

        /// <summary>
        /// Function for Get Stores By User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Store_VM> GetStoresByUserId(int userId)
        {
            var stores = (from userStores in _context.Sec_UserStores
                          join store in _context.Set_Stores on userStores.StoreId equals store.Id
                          where userStores.UserId == userId && store.Status == true && store.IsDeleted != true
                          && userStores.IsDeleted != true && userStores.Status == true
                          select new Store_VM
                          {
                              Id = store.Id,
                              StoreGuid = store.StoreGuid,
                              StoreName = store.StoreName
                          }).Distinct().ToList();

            return stores;
        }

        /// <summary>
        /// Function for delete store
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var store = _context.Set_Stores.Where(x => x.Id == id && x.IsDeleted == false && x.CreatedBy == loginUserId).FirstOrDefault();
                    if (store != null)
                    {
                        var userStores = _context.Sec_UserStores.Where(x => x.StoreId == store.Id && x.IsDeleted == false && x.CreatedBy == loginUserId).ToList();
                        if (userStores.Any())
                        {
                            foreach (var userStore in userStores)
                            {
                                userStore.IsDeleted = true;
                                userStore.DeletedBy = loginUserId;
                                userStore.CreatedOn = DateTime.Now;
                                _context.SaveChanges();
                            }
                        }

                        store.IsDeleted = true;
                        store.DeletedOn = DateTime.Now;
                        store.DeletedBy = loginUserId;
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

        /// <summary>
        /// Function for check Unique Store Code
        /// </summary>
        /// <param name="id"></param>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public bool IsUniqueStoreCode(int id, string storeCode, int clientId, int loginUserId)
        {
            var ret = false;
            var store = _context.Set_Stores.Where(x => x.StoreCode == storeCode && x.IsDeleted != true && x.CreatedBy == loginUserId && x.ClientId == clientId).FirstOrDefault();
            if (store != null)
            {
                // For Edit Mode
                if (id > 0 && store.Id == id)
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
        /// Function for check Unique Store Name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public bool IsUniqueStoreName(int id, string storeName, int clientId, int loginUserId)
        {
            var ret = false;
            var store = _context.Set_Stores.Where(x => x.StoreName == storeName && x.IsDeleted != true && x.CreatedBy == loginUserId && x.ClientId == clientId).FirstOrDefault();
            if (store != null)
            {
                // For Edit Mode
                if (id > 0 && store.Id == id)
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
        /// Function for add store
        /// </summary>
        /// <param name="store_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(Store_VM store_VM, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var store = new Set_Stores();
                    store.StoreGuid = Guid.NewGuid();
                    store.StoreCode = store_VM.StoreCode;
                    store.StoreName = store_VM.StoreName;
                    store.Status = store_VM.Status;
                    store.RegionId = store_VM.RegionId;
                    store.CountryCode = store_VM.CountryCode;
                    store.CreatedOn = DateTime.Now;
                    store.CreatedBy = loginUserId;
                    store.DeletedBy = 0;
                    store.ClientId = store_VM.ClientId;
                    _context.Set_Stores.Add(store);
                    _context.SaveChanges();

                    // Add Stores Concepts
                    var storesConcepts = new List<Set_StoresConcepts>();
                    foreach (var concept in store_VM.ConceptIds)
                    {
                        var obj = new Set_StoresConcepts();
                        obj.StoreId = store.Id;
                        obj.ClientId = store.ClientId;
                        obj.ConceptId = concept;
                        storesConcepts.Add(obj);

                    }
                    _context.Set_StoresConcepts.AddRange(storesConcepts);
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
        /// Function for update store
        /// </summary>
        /// <param name="store_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(Store_VM store_VM, int loginUserId)
        {
            var ret = 1;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var store = _context.Set_Stores.Where(x => x.Id == store_VM.Id && x.IsDeleted == false).FirstOrDefault();
                    if (store != null)
                    {
                        store.StoreCode = store_VM.StoreCode;
                        store.StoreName = store_VM.StoreName;
                        store.Status = store_VM.Status;
                        store.RegionId = store_VM.RegionId;
                        store.CountryCode = store_VM.CountryCode;
                        store.ModifiedBy = loginUserId;
                        store.ModifiedOn = DateTime.Now;
                        store.ClientId = store_VM.ClientId;
                        _context.SaveChanges();

                        //Remove old stores concepts
                        var oldStoresConcepts = _context.Set_StoresConcepts.Where(x => x.StoreId == store.Id).ToList();
                        if (oldStoresConcepts.Any())
                        {
                            _context.Set_StoresConcepts.RemoveRange(oldStoresConcepts);
                            _context.SaveChanges();
                        }

                        // Add Stores Concepts
                        var storesConcepts = new List<Set_StoresConcepts>();
                        foreach (var concept in store_VM.ConceptIds)
                        {
                            var obj = new Set_StoresConcepts();
                            obj.StoreId = store.Id;
                            obj.ClientId = store.ClientId;
                            obj.ConceptId = concept;
                            storesConcepts.Add(obj);

                        }
                        _context.Set_StoresConcepts.AddRange(storesConcepts);
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

        /// <summary>
        /// Function for Get Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Store_VM Get(int id, int loginUserId)
        {
            var clientList = _context.Sec_Client.Where(x => x.IsDeleted != true && x.Status == true).AsEnumerable();
            var stores = (from store in _context.Set_Stores
                          join client in clientList on store.ClientId equals client.Id
                          where store.CreatedBy == loginUserId && store.IsDeleted != true && store.Id == id
                          select new Store_VM()
                          {
                              Id = store.Id,
                              StoreName = store.StoreName,
                              Status = (bool)store.Status,
                              StoreCode = store.StoreCode != null ? store.StoreCode : string.Empty,
                              RegionId = store.RegionId,
                              CountryCode = store.CountryCode,
                              ClientId = store.ClientId,
                              ConceptIds = _context.Set_StoresConcepts.Where(x => x.StoreId == id).Select(x => x.ConceptId).ToList()
                          }).FirstOrDefault();
            return stores;
        }
    }
}
