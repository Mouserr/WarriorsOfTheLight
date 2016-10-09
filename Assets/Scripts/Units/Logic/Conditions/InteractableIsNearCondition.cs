using Assets.Scripts.Core.FiniteStateMachine;

namespace Assets.Scripts.Units.Logic.Conditions
{
    public class InteractableIsNearCondition : IFSMCondition<UnitController>
    {
        public bool IsTriggered(UnitController entity)
        {
            if (entity.Interactable == null) return false;
            float radius = entity.Interactable.BoundingRadius + entity.Range;
            return (entity.Interactable.Position - entity.Position).sqrMagnitude < radius * radius;
        }
    }
}