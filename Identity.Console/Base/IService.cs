using MediatR;

namespace Identity.Console.Base
{
    public interface IService<in TOptions> : IRequestHandler<TOptions, int> 
        where TOptions : BaseOptions { }
}