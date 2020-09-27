using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Identity.Console.Base
{
    public abstract class BaseService<T> : IService<T> 
        where T : BaseOptions
    {
        protected abstract string ErrorMessagePrefix { get; } 
        public abstract Task<int> Handle(T request, CancellationToken cancellationToken);

        protected string BuildErrorMessage(IEnumerable<IdentityError> identityErrors)
        {
            var errors = new List<string>{ $"{ErrorMessagePrefix}: "};
            errors.AddRange(identityErrors.Select(e => $"{e.Code}: {e.Description}"));
            return string.Join("\r\n\t- ", errors);
        }
    }
}