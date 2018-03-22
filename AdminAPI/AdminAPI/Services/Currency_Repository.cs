using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.Models;
using AdminAPI.ServiceContract;
using Microsoft.EntityFrameworkCore;

namespace AdminAPI.Services
{
    public class Currency_Repository : ICurrency_Repository, IDisposable
    {
        private readonly AdminDemoContext _context;
        public Currency_Repository(AdminDemoContext context)
        {
            _context = context;
        }
        public Currency_Repository()
        {
            _context = new AdminDemoContext();

        }
        public async Task<List<CurrencyMaster>> GetAll()
        {
            try
            {
                return await _context.CurrencyMaster.Where(c => c.IsActive == true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CurrencyMaster>> GetAllForBase()
        {
            try
            {
                return await _context.CurrencyMaster.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Insert(CurrencyMaster currencyMaster)
        {
            try
            {
                await _context.CurrencyMaster.AddAsync(currencyMaster);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(CurrencyMaster currencyMaster)
        {
            try
            {
                _context.Entry(currencyMaster).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var record = await _context.CurrencyMaster.SingleOrDefaultAsync(m => m.CurrencyId == id);
                if (record == null)
                    return false;
                _context.Remove(record);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CurrencyMaster> GetById(int Id)
        {
            try
            {
                return await _context.CurrencyMaster.SingleOrDefaultAsync(x => x.CurrencyId == Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
