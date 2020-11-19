using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository
{
    public class CustomersRespository : ICustomersRespository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public CustomersRespository(EMenuDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Cutomer list by client Id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public List<Set_Customers> GetCustomersByClient(int clientId)
        {
            var customers = _context.Set_Customers.Where(x => x.ClientId == clientId).Select(x => new Set_Customers
            {
                Id = x.Id,
                FirstName = x.FirstName + " " + x.LastName
            }).ToList();
            return customers;
        }
    }
}
