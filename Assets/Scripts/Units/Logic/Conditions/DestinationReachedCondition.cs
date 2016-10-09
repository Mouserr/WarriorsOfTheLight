using Assets.Scripts.Core.FiniteStateMachine;

namespace Assets.Scripts.Units.Logic.Conditions
{
    public class DestinationReachedCondition : IFSMCondition<UnitController>
    {
        public bool IsTriggered(UnitController entity)
        {
            return (entity.CurrentDestination - entity.Position).sqrMagnitude < entity.BoundingRadius * entity.BoundingRadius;
        }
    }
}