using Assets.Scripts.Core.FiniteStateMachine;

namespace Assets.Scripts.Units.Logic.Conditions
{
    public class HasEnemiesCondition : IFSMCondition<UnitController>
    {
        public bool IsTriggered(UnitController entity)
        {
            return MapController.Instance.HasAvailableEnemy(entity);
        }
    }
}