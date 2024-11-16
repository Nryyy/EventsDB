using Managment.Interfaces;
using Managment.ApplicationContext;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment.Classes
{
    public class OrganizersRepository : IOrganizers
    {
        private readonly DataContext dataContext;
        private bool disposed = false;

        public OrganizersRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Create(Organizers organizer)
        {
            if (!dataContext.Organizers.Any(o => o.Name == organizer.Name))
            {
                dataContext.Organizers.Add(organizer);
            }
            else
            {
                throw new InvalidOperationException("Organizer with the same name already exists.");
            }
        }

        public void Delete(int id)
        {
            dataContext.Organizers.Remove(Get(id));
        }

        public IEnumerable<Organizers> Get()
        {
            return dataContext.Organizers;
        }

        public Organizers? Get(int id)
        {
            return dataContext.Organizers.Find(id);
        }

        public void Update(Organizers organizer)
        {
            dataContext.Entry(organizer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
