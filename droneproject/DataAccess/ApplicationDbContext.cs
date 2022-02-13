using droneproject.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Drone> Drones { get; set; }

        public DbSet<Mediation> Mediations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public ApplicationDbContext()
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

    }
}
