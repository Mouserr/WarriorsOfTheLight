using Assets.Scripts.Core.FiniteStateMachine;
using UnityEngine;

namespace Assets.Scripts.Units.Logic.States
{
    public class MoveToPointState : IFSMState<UnitController>
    {
        private Vector3 direction;
        
        public string Name
        {
            get { return UnitStates.MoveToPoint; }
        }

        public void Enter(UnitController entity)
        {
            entity.HasNewOrder = false;
            entity.StartMovement();
        }

        public void Execute(UnitController entity)
        {
            direction = (entity.CurrentDestination - entity.Position).normalized;
            entity.transform.rotation = Quaternion.LookRotation(direction);
            entity.CharacterController.SimpleMove(direction * entity.Unit.Speed);
        }

        public void Exit(UnitController entity)
        {
            entity.StopMovement();
        }
    }
}