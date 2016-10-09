using Assets.Scripts.Core.FiniteStateMachine;

namespace Assets.Scripts.Units.Logic.Conditions
{
    public class InteractOrderCondition : IFSMCondition<UnitController>
    {
        public bool IsTriggered(UnitController entity)
        {
            return entity.HasNewOrder && entity.Interactable != null && entity.Interactable.IsAlive;
        }
    }
}