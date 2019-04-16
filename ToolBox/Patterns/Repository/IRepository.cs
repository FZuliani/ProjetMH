using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Patterns.Repository
{
    public interface IRepository<TKey, TEntity>
    {
        IEnumerable<TEntity> Get();
        TEntity Get(TKey key);
        TEntity Insert(TEntity entity);
        bool Update(TKey key, TEntity entity);
        bool Delete(TKey key);
    }
}
