using Assets.Scripts.Core.FiniteStateMachine;

namespace Assets.Scripts.Units.Logic.States
{
    public class IdleState : IFSMState<UnitController>
    {
        public string Name
        {
            get { return UnitStates.Idle; }
        }

        public void Enter(UnitController entity)
        {
        }

        public void Execute(UnitController entity)
        {
        }

        public void Exit(UnitController entity)
        {
        }
    }
}