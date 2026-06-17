using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            return await _dbContext.SaveChangesAsync();
        }
    }
}