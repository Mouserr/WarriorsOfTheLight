using Assets.Scripts.Core.FiniteStateMachine;

namespace Assets.Scripts.Units.Logic.Conditions
{
    public class NotInteractingWithEnemyCondition : IFSMCondition<UnitController>
    {
        public bool IsTriggered(UnitController entity)
        {
            return entity.Interactable == null 
                || !entity.Interactable.IsAlive 
                || !entity.Interactable.IsVulnerable 
                || !(entity.Interactable is UnitController);
        }
    }
}