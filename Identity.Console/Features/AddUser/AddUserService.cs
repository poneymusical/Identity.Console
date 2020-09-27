using System.Threading;
using System.Threading.Tasks;
using Identity.Console.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Console.Features.AddUser
{
    public class AddUserService : BaseService<AddUserOptions>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AddUserService> _logger;

        protected override string ErrorMessagePrefix => "Error during user creation";
        
        public AddUserService(UserManager<IdentityUser> userManager,
            ILogger<AddUserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public override async Task<int> Handle(AddUserOptions request, CancellationToken cancellationToken)
        {
            var identityUser = BuildUserFromRequest(request);
            var result = await _userManager.CreateAsync(identityUser, request.Password);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("User successfully created");
                return 0;
            }

            _logger.LogError(BuildErrorMessage(result.Errors));
            return 1;
        }

        private IdentityUser BuildUserFromRequest(AddUserOptions options)
        {
            var identityUser = new IdentityUser
            {
                Email = options.Email, 
                EmailConfirmed = true, 
                UserName = options.UserName ?? options.Email,
                TwoFactorEnabled = options.TwoFactorEnabled
            };

            if (!string.IsNullOrWhiteSpace(options.PhoneNumber))
            {
                identityUser.PhoneNumber = options.PhoneNumber;
                identityUser.PhoneNumberConfirmed = true;
            }
            
            return identityUser;
        }
    }
}