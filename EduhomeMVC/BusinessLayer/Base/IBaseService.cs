using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Base
{
    public interface IBaseService<TEntity>
    {
        Task<TEntity> Get(int? id);
        Task<List<TEntity>> GetAll();
        Task Create(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(int? id);
    }
}
