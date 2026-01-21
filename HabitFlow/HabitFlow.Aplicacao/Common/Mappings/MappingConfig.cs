using HabitFlow.Aplicacao.Features.Goals.Queries.GetGoals;
using HabitFlow.Aplicacao.Features.Habits.Queries.GetHabits;
using HabitFlow.Domain.Goals;
using HabitFlow.Domain.Habits;
using Mapster;

namespace HabitFlow.Aplicacao.Common.Mappings
{
    /// <summary>
    /// Central configuration for Mapster object mapping.
    /// </summary>
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Habit → HabitDto (do código já gerado)
            TypeAdapterConfig<Habit, HabitDto>
                .NewConfig()
                .Map(dest => dest.FrequencyType, src => src.Frequency.Type.ToString())
                .Map(dest => dest.TargetType, src => src.Target.Type.ToString())
                .Map(dest => dest.TargetValue, src => src.Target.Value)
                .Map(dest => dest.TargetUnit, src => src.Target.Unit)
                .Map(dest => dest.Status, src => src.Status.ToString());

            // Goal → GoalDto (do código já gerado)
            TypeAdapterConfig<Goal, GoalDto>
                .NewConfig()
                .Map(dest => dest.Status, src => src.Status.ToString())
                .Map(dest => dest.ProgressPercentage, src =>
                    src.TargetValue > 0 ? src.CurrentValue / src.TargetValue * 100 : 0);

            // REMOVIDO: UserProfileDto (não existe)
            // REMOVIDO: UserLevelDto (não existe)
        }
    }
}
