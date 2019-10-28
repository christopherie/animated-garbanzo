using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDemo.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> GetAll();
        T GetById(Guid id);
        bool Delete(Guid id);
        void Insert(T t);
        void Update(T t);
    }
}
