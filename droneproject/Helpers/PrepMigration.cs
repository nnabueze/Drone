using droneproject.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.Helpers
{
    public static class PrepMigration
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                Populate(serviceScope.ServiceProvider.GetService<ApplicationDbContext>());
            }
        }

        public static void Populate(ApplicationDbContext context)
        {
            Console.WriteLine("Applying Migration");

            context.Database.Migrate();

            Console.WriteLine("Migration completed");
        }
    }
}
