using System;
using System.Collections.Generic;
using System.Linq;
using Managment.ApplicationContext;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace Managment.Classes
{
    public class EventRepository
    {
        private readonly DataContext dataContext;
        private bool disposed = false;

        public EventRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Create(Event @event)
        {
            if (!dataContext.Events.Any(e => e.Name == @event.Name))
            {
                dataContext.Events.Add(@event);
            }
            else
            {
                throw new InvalidOperationException("Event with the same name already exists.");
            }
        }

        public void Update(Event @event)
        {
            dataContext.Entry(@event).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            dataContext.Events.Remove(Get(id));
        }

        public IEnumerable<Event> Get()
        {
            return dataContext.Events;
        }

        public Event? Get(int id)
        {
            return dataContext.Events.Find(id);
        }

        public void SaveChanges()
        {
            dataContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    dataContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
