using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment.Interfaces
{
    public interface IStatus : IDisposable
    {
        void Create(Status status);
        IEnumerable<Status> Get();
        Status? Get(int id);
        void Update(Status status);
        void Delete(int id);
        void SaveChanges();
    }
}
