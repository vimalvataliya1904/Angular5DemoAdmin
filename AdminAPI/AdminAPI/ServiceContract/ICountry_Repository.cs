using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdminAPI.Models;

namespace AdminAPI.ServiceContract
{
    interface ICountry_Repository : IDisposable
    {
        Task<List<CountryMaster>> GetAll();
        Task<CountryMaster> GetById(int Id);
        Task Insert(CountryMaster countryMaster);
        Task Update(CountryMaster countryMaster);
        Task<bool> Delete(int id);
    }
}
