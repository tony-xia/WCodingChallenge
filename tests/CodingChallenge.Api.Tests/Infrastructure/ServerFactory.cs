using CodingChallenge.Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace CodingChallenge.Api.Tests.Infrastructure
{
    public class ServerFactory : WebApplicationFactory<Startup>
    {
        private readonly DataSeed _dataSeed;

        public ServerFactory(DataSeed dataSeed)
        {
            _dataSeed = dataSeed;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<BuildingDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDB");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<BuildingDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<ServerFactory>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        db.Jobs.AddRange(_dataSeed.Jobs);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to seed the database.");
                    }
                }
            });
        }
    }

}
