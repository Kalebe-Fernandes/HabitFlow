using HabitFlow.Aplicacao.Common.Models;
using MediatR;

namespace HabitFlow.Aplicacao.Common.Interfaces
{
    public interface ICommand : IRequest<Result> { }
    public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
}
