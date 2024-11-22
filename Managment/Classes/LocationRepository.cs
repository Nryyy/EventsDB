﻿using Managment.Interfaces;
using Managment.ApplicationContext;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managment.Classes
{
    public class LocationRepository : ILocation
    {
        private readonly DataContext dataContext;
        private bool disposed = false;
        public LocationRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Create(Location location)
        {
            if (!dataContext.Locations.Any(l => l.Name == location.Name))
            {
                dataContext.Locations.Add(location);
            }
            else
            {
                throw new InvalidOperationException("Location with the same name already exists.");
            }
        }

        public void Delete(int id)
        {
            dataContext.Locations.Remove(Get(id));
        }

        public IEnumerable<Location> Get()
        {
            return dataContext.Locations;
        }

        public Location? Get(int id)
        {
            return dataContext.Locations.Find(id);
        }
        public void Update(Location location)
        {
            dataContext.Entry(location).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
