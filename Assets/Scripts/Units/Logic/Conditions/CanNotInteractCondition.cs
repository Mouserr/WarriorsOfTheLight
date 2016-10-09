using Assets.Scripts.Core.FiniteStateMachine;

namespace Assets.Scripts.Units.Logic.Conditions
{
    public class CanNotInteractCondition : IFSMCondition<UnitController>
    {
        public bool IsTriggered(UnitController entity)
        {
            return entity.Interactable == null || !entity.Interactable.IsAlive || ReferenceEquals(entity.Interactable, entity);
        }
    }
}