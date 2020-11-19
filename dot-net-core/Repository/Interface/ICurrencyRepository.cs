using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;

namespace EMenuApplication.Repository.Interface
{
    public interface ICurrencyRepository
    {
        List<Currency_VM> GetList(int loginUserId);
        Currency_VM Get(int id, int loginUserId);
        int Add(Currency_VM currency_VM, int loginUserId);
        int Update(Currency_VM currency_VM, int loginUserId);
        int Delete(int id, int loginUserId);
        bool IsUniqueCurrencyCode(int id, string currency, int loginUserId);
        List<Currency_VM> GetByClientId(int clientId);
    }
}
