using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment.Interfaces
{
    public interface IOrganizers : IDisposable
    {
        void Create(Organizers organizers);
        IEnumerable<Organizers> Get();
        Organizers? Get(int id);
        void Update(Organizers organizers);
        void Delete(int id);
        void SaveChanges();
    }
}
