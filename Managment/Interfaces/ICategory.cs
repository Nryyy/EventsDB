using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment.Interfaces
{
    public interface ICategory : IDisposable
    {
        void Create(Category categoty);
        IEnumerable<Category> Get();
        Category? Get(int id);
        void Update(Category categoty);
        void Delete(int id);
        void SaveChanges();
    }
}
