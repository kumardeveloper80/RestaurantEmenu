using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMenuApplication.Repository
{
    public class CurrencyRepository : ICurrencyRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public CurrencyRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function for add currency
        /// </summary>
        /// <param name="currency_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(Currency_VM currency_VM, int loginUserId)
        {
            var obj = new Emenu_Currency();
            obj.Currency = currency_VM.Currency;
            obj.Symbol = currency_VM.Symbol;
            obj.SymbolAR = currency_VM.SymbolAR;
            obj.CreatedOn = DateTime.Now;
            obj.CreatedBy = loginUserId;
            obj.ClientId = currency_VM.ClientId;
            _context.Emenu_Currency.Add(obj);
            return _context.SaveChanges();
        }

        /// <summary>
        /// Function for delete curremcy by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var isCurrencyRef = _context.Emenu_MenuItems.Where(x => x.IsDeleted != true && x.CurrencyId == id).FirstOrDefault();
            if (isCurrencyRef == null)
            {
                var currency = _context.Emenu_Currency.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
                if (currency != null)
                {
                    currency.IsDeleted = true;
                    currency.DeletedOn = DateTime.Now;
                    currency.DeletedBy = loginUserId;
                    return _context.SaveChanges();
                }
                return 0;
            }
            else
            {
                return Helper.refernce_error_code;
            }
        }

        /// <summary>
        /// Function for get currency by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Currency_VM Get(int id, int loginUserId)
        {
            


            var currency = _context.Emenu_Currency.Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => new Currency_VM()
                {
                    Id = x.Id,
                    Currency = x.Currency,
                    Symbol = x.Symbol,
                    SymbolAR = x.SymbolAR
                }).FirstOrDefault();
            return currency;
        }

        /// <summary>
        /// Function for Get Currcency List
        /// </summary>
        /// <returns></returns>
        public List<Currency_VM> GetList(int loginUserId)
        {

            var clientid = _context.Sec_Users.Where(c => c.Id == loginUserId).Select(c => c.ClientId).FirstOrDefault();

            var list = _context.Emenu_Currency.Where(x => !x.IsDeleted && x.ClientId == clientid)
                .Select(x => new Currency_VM()
                {
                    Id = x.Id,
                    Currency = x.Currency,
                    Symbol = x.Symbol,
                    SymbolAR = x.SymbolAR
                }).ToList();

            return list;
        }

        /// <summary>
        /// Function for check unique currency code
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public bool IsUniqueCurrencyCode(int id, string currency, int loginUserId)
        {
            var ret = false;
            var obj = _context.Emenu_Currency.Where(x => x.Currency == currency && !x.IsDeleted && x.CreatedBy == loginUserId).FirstOrDefault();
            if (obj != null)
            {
                // For edit mode
                if (id > 0 && obj.Id == id)
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
        /// Function for Update currency
        /// </summary>
        /// <param name="currency_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(Currency_VM currency_VM, int loginUserId)
        {
            var currency = _context.Emenu_Currency.Where(x => x.Id == currency_VM.Id && x.IsDeleted == false).FirstOrDefault();
            if (currency != null)
            {
                currency.Currency = currency_VM.Currency;
                currency.Symbol = currency_VM.Symbol;
                currency.SymbolAR = currency_VM.SymbolAR;
                currency.ModifiedBy = loginUserId;
                currency.ModifiedOn = DateTime.Now;
                currency.ClientId = currency_VM.ClientId;
                return _context.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// Get Currency By ClientId
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public List<Currency_VM> GetByClientId(int clientId)
        {
            var list = _context.Emenu_Currency.Where(x => !x.IsDeleted && x.ClientId == clientId)
                .Select(x => new Currency_VM()
                {
                    Id = x.Id,
                    Currency = x.Currency,
                    Symbol = x.Symbol,
                    SymbolAR = x.SymbolAR
                }).ToList();

            return list;
        }
    }
}
