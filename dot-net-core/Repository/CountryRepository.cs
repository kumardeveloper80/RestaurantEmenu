using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository
{
    public class CountryRepository : ICountryRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public CountryRepository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Function for get country list
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public List<Country_VM> GetList()
        {
            var country = (from c in _context.Set_CountryCodes
                           where c.IsDeleted != true
                           select new Country_VM()
                           {
                               CountryID = c.CountryID,
                               Name = c.Name != null ? c.Name : string.Empty,
                               Country = c.Country != null ? c.Country : string.Empty,
                               Code = c.Code != null ? c.Code : string.Empty,
                               Status = c.Status,
                               minDigits = c.minDigits != null ? c.minDigits.Value : 0,
                               Digits = c.Digits != null ? c.Digits.Value : 0,
                               timezone = c.timezone != null ? c.timezone : string.Empty
                           }).ToList();
            return country;
        }

        /// <summary>
        /// Function for get country by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Country_VM Get(int id)
        {
            var country = (from c in _context.Set_CountryCodes
                           where c.IsDeleted != true && c.CountryID == id
                           select new Country_VM()
                           {
                               CountryID = c.CountryID,
                               Name = c.Name,
                               Country = c.Country,
                               Code = c.Code,
                               Status = c.Status,
                               minDigits = c.minDigits,
                               Digits = c.Digits,
                               timezone = c.timezone
                           }).FirstOrDefault();
            return country;
        }

        /// <summary>
        /// Function for check unique country name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="countryName"></param>
        /// <returns></returns>
        public bool IsUniqueCountryName(int id, string countryName)
        {
            var ret = false;
            var result = _context.Set_CountryCodes.Where(x => x.IsDeleted != true && x.Name == countryName).FirstOrDefault();
            if (result != null)
            {
                if (id > 0 && result.CountryID == id)
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
        /// Function for delete country
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Delete(int id, int loginUserId)
        {
            var country = _context.Set_CountryCodes.Where(x => x.CountryID == id && x.IsDeleted == false).FirstOrDefault();
            if (country != null)
            {
                country.IsDeleted = true;
                country.DeletedOn = DateTime.Now;
                country.DeletedBy = loginUserId;
                return _context.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// Fucntion for save country
        /// </summary>
        /// <param name="country_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Add(Country_VM country_VM, int loginUserId)
        {
            var obj = new Set_CountryCodes();
            obj.Name = country_VM.Name;
            obj.Country = country_VM.Country;
            obj.Code = country_VM.Code;
            obj.minDigits = country_VM.minDigits;
            obj.Digits = country_VM.Digits;
            obj.timezone = country_VM.timezone;
            obj.Status = country_VM.Status;
            obj.CreatedBy = loginUserId;
            obj.CreatedOn = DateTime.Now;
            _context.Set_CountryCodes.Add(obj);
            return _context.SaveChanges();
        }

        /// <summary>
        /// Fucntion for update country
        /// </summary>
        /// <param name="country_VM"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public int Update(Country_VM country_VM, int loginUserId)
        {
            var obj = _context.Set_CountryCodes.Where(x => x.CountryID == country_VM.CountryID && x.IsDeleted == false).FirstOrDefault();
            if (obj != null)
            {
                obj.Name = country_VM.Name;
                obj.Country = country_VM.Country;
                obj.Code = country_VM.Code;
                obj.minDigits = country_VM.minDigits;
                obj.Digits = country_VM.Digits;
                obj.timezone = country_VM.timezone;
                obj.Status = country_VM.Status;
                obj.ModifiedBy = loginUserId;
                obj.ModifiedOn = DateTime.Now;
                return _context.SaveChanges();
            }
            return 0;
        }
    }
}
