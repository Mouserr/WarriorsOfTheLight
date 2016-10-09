using Assets.Scripts.Core.FiniteStateMachine;

namespace Assets.Scripts.Units.Logic.Conditions
{
    public class IsNotForcedCondition : IFSMCondition<UnitController>
    {
        public bool IsTriggered(UnitController entity)
        {
            return !entity.HasForcedOrder;
        }
    }
}