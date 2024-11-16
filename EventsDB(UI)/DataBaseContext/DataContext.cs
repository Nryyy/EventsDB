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
        public DbSet<Organizers> Organizers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=EventsDB;User=sa;Password=Admin123!;TrustServerCertificate=True;");
        }
    }
}
