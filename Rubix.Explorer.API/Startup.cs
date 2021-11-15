using System;
using Rubix.API.Shared.Interfaces;
using Rubix.API.Shared.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.SwaggerUI;
using Quartz;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Any;
using System.Linq;

namespace Rubix.Explorer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors();


            services.AddSingleton<IMongoClient>(c =>
            {
                var login = "admin";
                var password = Uri.EscapeDataString("IjzUmspU8yDwg5MW");
                var server = "cluster0.jeaxq.mongodb.net";
                return new MongoClient($"mongodb+srv://{login}:{password}@{server}/rubixDb?retryWrites=true&w=majority");
            });

            services.AddScoped(c =>
                c.GetService<IMongoClient>().StartSession());

            services.AddTransient<IRepositoryRubixUser, RepositoryRubixUser>();
            services.AddTransient<IRepositoryRubixToken, RepositoryRubixToken>();
            services.AddTransient<IRepositoryRubixTokenTransaction, RepositoryRubixTokenTransaction>();
            services.AddTransient<IRepositoryRubixTransaction, RepositoryRubixTransaction>();
            services.AddTransient<IRepositoryDashboard, RepositoryDashboard>();
            services.AddTransient<IRepositoryCardsDashboard,RepositoryCardsDashboard>();


            //services.AddQuartz(q =>
            //{
            //    q.UseMicrosoftDependencyInjectionScopedJobFactory();

            //    // Create a "key" for the job
            //    var jobKey = new JobKey("RubixDashboardJob");

            //    // Register the job with the DI container
            //    q.AddJob<RubixDashboardJob>(opts => opts.WithIdentity(jobKey));

            //    // Create a trigger for the job
            //    q.AddTrigger(opts => opts
            //        .ForJob(jobKey) // link to the HelloWorldJob
            //        .WithIdentity("RubixDashboardJob-trigger") // give the trigger a unique name
            //        .WithCronSchedule("0/5 * * * * ?")); // run every 5 seconds

            //});

            //services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Rubix Explorer API",
                        Version = "v1"
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true) // allow any origin
              .AllowCredentials()); // allow credentials

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                endpoints.MapControllers()
            );

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MongoDB POC");
                c.DocumentTitle = "Rubix Explorer API";
                c.DocExpansion(DocExpansion.List);
            });
        }
    }
}
  
