using Managment.Interfaces;
using Managment.ApplicationContext;
using Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment.Classes
{
    public class StatusRepository : IStatus
    {
        private readonly DataContext dataContext;
        private bool disposed = false;

        public StatusRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Create(Status status)
        {
            if (!dataContext.Status.Any(s => s.Name == status.Name))
            {
                dataContext.Status.Add(status);
            }
            else
            {
                throw new InvalidOperationException("Status with the same name already exists.");
            }
        }

        public void Delete(int id)
        {
            dataContext.Status.Remove(Get(id));
        }

        public IEnumerable<Status> Get()
        {
            return dataContext.Status;
        }

        public Status? Get(int id)
        {
            return dataContext.Status.Find(id);
        }

        public void Update(Status status)
        {
            dataContext.Entry(status).State = EntityState.Modified;
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
