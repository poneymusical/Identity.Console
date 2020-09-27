using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommandLine;
using Identity.Console.Base;
using Identity.Console.Features.AddRole;
using Identity.Console.Features.AddUser;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Identity.Console
{
    internal static class Program
    {
        private static IServiceProvider _serviceProvider;

        static Program()
        {
        }

        internal static async Task<int> Main(string[] args)
        {
            return await Parser.Default
                .ParseArguments<AddRoleOptions, AddUserOptions>(args)
                .MapResult(
                    (AddRoleOptions options) => Process(options),
                    (AddUserOptions options) => Process(options),
                    errors => Task.FromResult(1));
        }

        private static async Task<int> Process(BaseOptions options)
        {
            ConfigureServices(options);
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            return await mediator.Send(options);
        }

        private static void ConfigureServices(BaseOptions options)
        {
            var services = new ServiceCollection();
            
            services.AddDbContext<IdentityDbContext<IdentityUser>>(dbContextOptions =>
            {
                if (options.Sqlite)
                    dbContextOptions.UseSqlite(options.ConnectionString);
                else if (options.SqlServer)
                    dbContextOptions.UseSqlServer(options.ConnectionString);
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext<IdentityUser>>()
                .AddDefaultTokenProviders();

            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            services.AddLogging(logging =>
            {
                logging
                    // .AddFilter("Microsoft", LogLevel.Warning)
                    // .AddFilter("Identity.Console", options.MinLogLevel)
                    .SetMinimumLevel(LogLevel.Warning)
                    .AddConsole();
            });
            
            _serviceProvider = services.BuildServiceProvider();
        }
    }
}