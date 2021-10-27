using DSCC._8392.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DSCC._8392.Domain.Contracts
{
    //implementation is in DAL, but contract should be defined in domain layer
    public interface IRepository<T> where T : BaseEntity
    {
        Task InsertItemAsync(T item);
        Task UpdateItemAsync(T item);
        Task DeleteItemAsync(T item);
        Task<T> GetItemByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetItemsAsync(params Expression<Func<T, object>>[] includes);
        bool IfExists(int id);
    }
}
