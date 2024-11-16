using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment.Interfaces
{
    public interface IEvent : IDisposable
    {
        void Create(Event @event);
        IEnumerable<Event> Get();
        Event? Get(int id);
        void Update(Event categoty);
        void Delete(int id);
        void SaveChanges();
    }
}
