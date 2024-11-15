using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Managment.Interfaces
{
    public interface ILocation : IDisposable
    {
        void Create(Location location);
        IEnumerable<Location> Get();
        Location? Get(int id);
        void Update(Location location);
        void Delete(int id);
        void SaveChanges();
        
    }
}
