using AdminAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAPI.ServiceContract
{
    interface ICurrency_Repository: IDisposable
    {
        Task<List<CurrencyMaster>> GetAll();
        Task<CurrencyMaster> GetById(int Id);
        Task<List<CurrencyMaster>> GetAllForBase();
        Task Insert(CurrencyMaster currencyMaster);
        Task Update(CurrencyMaster currencyMaster);
        Task<bool> Delete(int id);
    }
}
