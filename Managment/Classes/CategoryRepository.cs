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
    public class CategoryRepository : ICategory
    {
        private readonly DataContext dataContext;
        private bool disposed = false;

        public CategoryRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Create(Category category)
        {
            if (!dataContext.Categories.Any(c => c.Name == category.Name))
            {
                dataContext.Categories.Add(category);
            }
            else
            {
                throw new InvalidOperationException("Category with the same name already exists.");
            }
        }

        public void Delete(int id)
        {
            dataContext.Categories.Remove(Get(id));
        }

        public IEnumerable<Category> Get()
        {
            return dataContext.Categories;
        }

        public Category? Get(int id)
        {
            return dataContext.Categories.Find(id);
        }

        public void Update(Category category)
        {
            dataContext.Entry(category).State = EntityState.Modified;
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
