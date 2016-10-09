using Assets.Scripts.Core.FiniteStateMachine;

namespace Assets.Scripts.Units.Logic.Conditions
{
    public class PointOrderCondition : IFSMCondition<UnitController>
    {
        public bool IsTriggered(UnitController entity)
        {
            return entity.HasNewOrder && entity.Interactable == null;
        }
    }
}