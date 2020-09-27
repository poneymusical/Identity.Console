using System.Collections.Generic;
using CommandLine;
using Identity.Console.Base;

namespace Identity.Console.Features.AddUser
{
    [Verb("addUser", HelpText = "Adds an user in the database.")]
    public class AddUserOptions : BaseOptions
    {
        [Option("email", HelpText = "The user's email address", Required = true)]
        public string Email { get; set; }
        
        [Option("password", HelpText = "The user's password, in plain text", Required = true)]
        public string Password { get; set; }
        
        [Option("userName", HelpText = "The user's login. If not specified, the email address value will be used", Default = null)]
        public string UserName { get; set; }
        
        [Option("phone", HelpText = "The user's phone number", Default = null)]
        public string PhoneNumber { get; set; }
        
        [Option("twoFactor", HelpText = "If set, enables two-factor authentication for the added user.", Default = false)]
        public bool TwoFactorEnabled { get; set; }
    }
}