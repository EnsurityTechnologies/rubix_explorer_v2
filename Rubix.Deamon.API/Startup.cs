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

namespace Rubix.Deamon.API
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


            services.AddCors(options =>
            {
                options.AddPolicy(
                  "CorsPolicy",
                  builder => builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials());
            });


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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Rubix Deamon API",
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

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                endpoints.MapControllers()
            );

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MongoDB POC");
                c.DocumentTitle = "Rubix Deamon API";
                c.DocExpansion(DocExpansion.List);
            });
        }
    }
}
