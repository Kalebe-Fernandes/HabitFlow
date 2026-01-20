using HabitFlow.Aplicacao.Common.Models;
using MediatR;

namespace HabitFlow.Aplicacao.Common.Interfaces
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
}
