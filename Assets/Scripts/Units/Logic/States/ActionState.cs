using Assets.Scripts.Core.FiniteStateMachine;

namespace Assets.Scripts.Units.Logic.States
{
    public class ActionState : IFSMState<UnitController>
    {
        public string Name
        {
            get { return UnitStates.Action; }
        }

        public void Enter(UnitController entity)
        {
        }

        public void Execute(UnitController entity)
        {
            if (!entity.Unit.CanAttack) return;

            if (!entity.Interactable.IsAlive || !entity.Interactable.IsVulnerable || !entity.StartAttack())
            {
                entity.Interactable = null;
            }
        }

        public void Exit(UnitController entity)
        {
        }
    }
}