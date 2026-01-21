using HabitFlow.Aplicacao.Features.Gamification.Commands.AwardXP;
using HabitFlow.Domain.Habits.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HabitFlow.Infrastructure.EventHandlers
{
    /// <summary>
    /// Handler para evento de conclusão de hábito.
    /// Concede XP ao usuário quando um hábito é completado.
    /// </summary>
    public sealed class HabitCompletedEventHandler(IMediator mediator, ILogger<HabitCompletedEventHandler> logger) : INotificationHandler<HabitCompletedEvent>
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<HabitCompletedEventHandler> _logger = logger;

        public async Task Handle(HabitCompletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Processando HabitCompletedEvent: HabitId={HabitId}, UserId={UserId}",
                notification.HabitId,
                notification.UserId);

            // Concede XP padrão por conclusão
            var command = new AwardXPCommand(
                notification.UserId,
                10, // XP padrão - TODO: Buscar XP configurado no hábito
                $"Habit completed: {notification.HabitId}",
                notification.HabitId);

            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogWarning(
                    "Falha ao conceder XP: {Error}",
                    result.Error);
            }
            else
            {
                _logger.LogInformation(
                    "XP concedido: UserId={UserId}, Amount=10, TotalXP={TotalXP}, LeveledUp={LeveledUp}",
                    notification.UserId,
                    result.Value.TotalXP,
                    result.Value.LeveledUp);
            }
        }
    }
}
