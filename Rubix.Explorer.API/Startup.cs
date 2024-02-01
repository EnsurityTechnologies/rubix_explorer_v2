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
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            services.AddControllersWithViews();

            services.AddCors();


            //Live

            services.AddSingleton<IMongoClient>(c =>
            {
                var login = "admin";
                var password = Uri.EscapeDataString("IjzUmspU8yDwg5MW");
                var server = "cluster0.jeaxq.mongodb.net";
                var rubix_dbName = "rubixDb";
                return new MongoClient($"mongodb+srv://{login}:{password}@{server}/{rubix_dbName}?retryWrites=true&w=majority");
            });

            // Test

            //services.AddSingleton<IMongoClient>(c =>
            //{
            //    var login = "admin";
            //    var password = Uri.EscapeDataString("tqlTXQEh5ex7jt2Q");
            //    var server = "cluster0.peyce.mongodb.net";
            //    var rubix_dbName = "rubixDb";
            //    return new MongoClient($"mongodb+srv://{login}:{password}@{server}/{rubix_dbName}?retryWrites=true&w=majority");
            //});

            //services.AddSingleton<IMongoClient>(new MongoClient());

            services.AddScoped(c =>
                c.GetService<IMongoClient>().StartSession());

            services.AddTransient<UrlShortener>();

            services.AddTransient<IRepositoryShortURL,RepositoryShortURL>();
            services.AddTransient<IRepositoryRubixUser, RepositoryRubixUser>();
            services.AddTransient<IRepositoryRubixToken, RepositoryRubixToken>();
            services.AddTransient<IRepositoryRubixTokenTransaction, RepositoryRubixTokenTransaction>();
            services.AddTransient<IRepositoryRubixTransaction, RepositoryRubixTransaction>();
            services.AddTransient<IRepositoryDashboard, RepositoryDashboard>();
            services.AddTransient<IRepositoryCardsDashboard,RepositoryCardsDashboard>();
            services.AddTransient<ILevelBasedTokenRepository, LevelBasedTokenRepository>();
            services.AddTransient<IRepositoryRubixTransactionQuorum, RepositoryRubixTransactionQuorum>();
            services.AddTransient<IRepositoryNFTTokenInfo, RepositoryNFTTokenInfo>();
            services.AddTransient<IDIDMapperRepository, DIDMapperRepository>();
            services.AddTransient<IRepositoryRubixDataToken, RepositoryRubixDataToken>();
            services.AddTransient<IRepositoryRubixNFTTransaction, RepositoryRubixNFTTransaction>();



            services.AddMemoryCache();


            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                var jobDashboardKey = new JobKey("RubixDashboardJob");
                var jobCardKey = new JobKey("RubixCardDashboardJob");
                var levelBasedTokensCardKey = new JobKey("LevelBasedTokenJob");

                // Register the job with the DI container
                q.AddJob<RubixCardDashboardJob>(opts => opts.WithIdentity(jobCardKey));
                q.AddJob<LevelBasedTokenJob>(opts => opts.WithIdentity(levelBasedTokensCardKey));
                q.AddJob<RubixDashboardJob>(opts => opts.WithIdentity(jobDashboardKey));

                // Create a trigger for the job

                q.AddTrigger(opts => opts
                    .ForJob(jobCardKey) // link to the HelloWorldJob
                    .WithIdentity("RubixCardDashboardJob-trigger") // give the trigger a unique name
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInMinutes(10).RepeatForever())); // run every 5 minitues


                q.AddTrigger(opts => opts
                    .ForJob(levelBasedTokensCardKey) // link to the HelloWorldJob
                    .WithIdentity("LevelBasedTokenJob-trigger") // give the trigger a unique name
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInMinutes(2).RepeatForever())); // run every 5 minitues

                q.AddTrigger(opts => opts
                   .ForJob(jobDashboardKey) // link to the HelloWorldJob
                   .WithIdentity("jobDashboardKey-trigger") // give the trigger a unique name
                   .StartNow()
                   .WithSimpleSchedule(x => x.WithIntervalInMinutes(4).RepeatForever())); // run every 5 minitues

            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MongoDB POC");
                c.DocumentTitle = "Rubix Explorer API";
                c.DocExpansion(DocExpansion.List);
            });
            app.UseCors(x => x
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true) // allow any origin
              .AllowAnyOrigin()); // allow credentials

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Short}/{action=Index}/{id?}");
            });

        }
    }
}
  
