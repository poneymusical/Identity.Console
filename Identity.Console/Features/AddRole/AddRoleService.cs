using System.Threading;
using System.Threading.Tasks;
using Identity.Console.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Console.Features.AddRole
{
    public class AddRoleService : BaseService<AddRoleOptions>
    {
        private readonly ILogger<AddRoleService> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        protected override string ErrorMessagePrefix => "Error during role creation";

        public AddRoleService(ILogger<AddRoleService> logger,
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _roleManager = roleManager;
        }
        
        public override async Task<int> Handle(AddRoleOptions request, CancellationToken cancellationToken)
        {
            var role = new IdentityRole(request.RoleName);
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                _logger.LogInformation("Role successfully created");
                return 0;
            }

            _logger.LogError(BuildErrorMessage(result.Errors));
            return 1;
        }
    }
}