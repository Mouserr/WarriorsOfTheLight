using Assets.Scripts.Core.FiniteStateMachine;
using UnityEngine;

namespace Assets.Scripts.Units.Logic.States
{
    public class MoveToObjectState : IFSMState<UnitController>
    {
        private Vector3 direction;

        public string Name
        {
            get { return UnitStates.MoveToObject; }
        }

        public void Enter(UnitController entity)
        {
            entity.HasNewOrder = false;
            entity.StartMovement();
        }

        public void Execute(UnitController entity)
        {
            if (entity.Interactable == null) return;
            direction = (entity.Interactable.Position - entity.Position).normalized;
            entity.transform.rotation = Quaternion.LookRotation(direction);
            entity.CharacterController.SimpleMove(direction*entity.Unit.Speed);
        }

        public void Exit(UnitController entity)
        {
            entity.StopMovement();
        }
    }
}