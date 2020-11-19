using EMenuApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Repository.Interface
{
    public interface ICustomersRespository
    {
        public List<Set_Customers> GetCustomersByClient(int clientId);
    }
}
