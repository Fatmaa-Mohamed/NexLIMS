using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<int> AddAsync(T entity);
    }
}
