using CommandLine;
using Identity.Console.Base;

namespace Identity.Console.Features.AddRole
{
    [Verb("addRole", HelpText = "Adds a role in the database.")]
    public class AddRoleOptions : BaseOptions
    {
        [Option('n', "name", Required = true)]
        public string RoleName { get; set; }
    }
}