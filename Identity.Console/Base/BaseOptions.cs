using CommandLine;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Identity.Console.Base
{
    public abstract class BaseOptions : IRequest<int>
    {
        //Database options
        [Option("connectionString", Required = true)]
        public string ConnectionString { get; set; }

        [Option("sqlserver", 
            HelpText = "Configures the app to use a SQL Server database. If no database flag is specified, SQL Server will be used by default", 
            SetName = "sqlserver",
            Default = true)]
        public bool SqlServer { get; set; }
        
        [Option("sqlite", 
            HelpText = "Configures the app to use a SQLite database", 
            SetName = "sqlite",
            Default = false)]
        public bool Sqlite { get; set; }
        
        //Logging options
        [Option('v', "verbose")]
        public bool Verbose { get; set; }

        public LogLevel MinLogLevel => Verbose ? LogLevel.Trace : LogLevel.Information;
    }
}