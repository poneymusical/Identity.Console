Identity.Console
==================
# What is it? 
A humble CLI tool to handle admin tasks on asp.net core Identity compliant databases.
You can:
* Add a role
* Add a user

Other features will be added.
# How to use it
No prerequisites is needed as the .net runtime is embedded in the exe.

Everything is controlled through command-line arguments.

The tool must be called like this: `identityutil <verb> <options>`, where the verb tells what operation is performed.

## Common options
* `--help`: prints the help.
* `--version`: prints the version.
* `--verbose`: if not used, only warning and above logs will be printed to the console. if used, all logs will be printed.
* `--sqlserver`\\`--sqlite`: enables the use of a SQL Server/SQLite database. SQL Server is the default option.
* `--connectionString`: **required**. The connection string to the database.

## `addRole` verb options
* `--name`: **required**. The name of the role that will be added.

## `addUser` verb options
* `--email`: **required**. The user's email address. The address will automatically be confirmed.
* `--userName`: the user's login. If not used, the email address will be used as the login.
* `--password`: the user's password, in plain text
* `--phoneNumber`: the user's phone number. If used, the phone number will automatically be confirmed.
* `--twoFactor`: enables two-factor authentication for the added 

# How to build it
You'll need .net core >= 3.1
* Clone the repo
* In the repo directory, use `dotnet build`

# How to extend it
If you want to add more features, there's 3 things to do:

1) Create a *Options class that inherits from `BaseOptions`. The `[Verb]` and `[Option]` attributes come from the [Command Line Parser](https://github.com/commandlineparser/commandline) library.
```csharp
using CommandLine;
using Identity.Console.Base;

namespace Identity.Console.Features.DoStuff
{
    [Verb("doStuff", HelpText = "Does some stuff.")]
    public class DoStuffOptions : BaseOptions
    {
        [Option("myOption", Required = true)]
        public string MyOption { get; set; }
    }
}
```
2) Create a *Service class that implements `IService<T>`. You can also inherit from `BaseService<T>`, which will give you access to common features for error handling.
```csharp
using System.Threading;
using System.Threading.Tasks;
using Identity.Console.Base;

namespace Identity.Console.Features.DoStuff
{
    public class DoStuffService : BaseService<DoStuffOptions>
    {
        protected override string ErrorMessagePrefix => "Error while doing stuff";

        public override async Task<int> Handle(DoStuffOptions request, CancellationToken cancellationToken)
        {
            //Do stuff
            return 0; //Service should return 1 when an error is encountered, 0 when no error was encountered
        }
    }
}
```
3) Modify the Main Method to add your new Options class in the Parser call:
```csharp
internal static async Task<int> Main(string[] args)
{
    return await Parser.Default
        .ParseArguments<AddRoleOptions, AddUserOptions, DoStuffOptions>(args)
        .MapResult(
            (AddRoleOptions options) => Process(options),
            (AddUserOptions options) => Process(options),
            (DoStuffOptions options) => Process(options),
            errors => Task.FromResult(1));
}
```
**Remarks**: should you need extra stuff besides an ILogger or Identity manager classes, you can add anything you need in the DI wiring in `Program.cs`.