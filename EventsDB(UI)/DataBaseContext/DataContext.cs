using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace UI.DataBaseContext
{
    public class DataContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=EventsDB;User=sa;Password=Admin123!;TrustServerCertificate=True;");
        }
    }
}
