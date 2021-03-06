using droneproject.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using droneproject.Domain.Interface;
using AutoMapper;
using droneproject.Helpers;

namespace droneproject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //connection for containerize application (fix later)
            var server = Configuration["DBServer"] ?? "";
            var port = Configuration["DBPort"] ?? "";
            var user = Configuration["DBUser"] ?? "";
            var password = Configuration["DBPassword"] ?? "";
            var database = Configuration["Database"] ?? "";

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer($"Server={server},{port};Initial Catalog={database};User Id={user};Password={password}"));

            //services.AddDbContext<ApplicationDbContext>(options => 
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services
                .AddScoped<IDroneMaker, DroneMaker>()
                .AddScoped<IMedicationLoad, MedicationLoad>()
                .AddScoped<IMedicationLoadedItem, MedicationLoadedItem>()
                .AddScoped<IAvilableDrone, AvilableDrone>()
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelAttribute));
            });

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Drone", Version = "v1" });

                //c.OperationFilter<SwaggerFileOperationFilter>();

                var xmlFile = "Comments" + ".xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Drone v1"));
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepMigration.PrepPopulation(app);
        }
    }
}
