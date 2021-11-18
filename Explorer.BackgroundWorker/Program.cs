using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Quartz;
using Rubix.API.Shared.Interfaces;
using Rubix.API.Shared.Repositories;
using Rubix.Explorer.API;
using System;
using System.Threading.Tasks;

namespace Explorer.BackgroundWorker
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
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
                services.AddTransient<IRepositoryCardsDashboard, RepositoryCardsDashboard>();

                services.AddQuartz(q =>
                {
                    q.UseMicrosoftDependencyInjectionScopedJobFactory();

                    // Create a "key" for the job
                    var jobKey = new JobKey("RubixDashboardJob");

                    var jobCardKey = new JobKey("RubixCardDashboardJob");


                    // Register the job with the DI container
                    q.AddJob<RubixDashboardJob>(opts => opts.WithIdentity(jobKey));
                    q.AddJob<RubixCardDashboardJob>(opts => opts.WithIdentity(jobCardKey));

                    // Create a trigger for the job
                    q.AddTrigger(opts => opts
                        .ForJob(jobKey) // link to the HelloWorldJob
                        .WithIdentity("RubixDashboardJob-trigger") // give the trigger a unique name
                        .StartNow()
                        .WithSimpleSchedule(x => x.WithIntervalInMinutes(5).RepeatForever()));// run every 5 minitues

                    q.AddTrigger(opts => opts
                        .ForJob(jobCardKey) // link to the HelloWorldJob
                        .WithIdentity("RubixCardDashboardJob-trigger") // give the trigger a unique name
                        .StartNow()
                        .WithSimpleSchedule(x => x.WithIntervalInMinutes(10).RepeatForever())); // run every 5 minitues

                });


                services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            });
    }
}
