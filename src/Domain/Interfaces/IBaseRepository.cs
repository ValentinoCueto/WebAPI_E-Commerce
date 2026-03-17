using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        List<T> GetAll();
        T? GetById(int id);
        T Create(T entity);
        void Delete(T entity);
        void Update(T entity);

    }
}
