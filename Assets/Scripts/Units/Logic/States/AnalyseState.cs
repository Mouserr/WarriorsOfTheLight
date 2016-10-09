using System;
using Assets.Scripts.Core.FiniteStateMachine;
using UnityEngine;

namespace Assets.Scripts.Units.Logic.States
{
    public class AnalyseState : IFSMState<UnitController>
    {
        public string Name
        {
            get { return UnitStates.Analyse; }
        }

        public void Enter(UnitController entity)
        {
        }

        public void Execute(UnitController entity)
        {
            IInteractableObject nextGoal = MapController.Instance.GetNearestEnemy(entity);

            if (nextGoal != null && nextGoal.IsAlive && nextGoal.IsVulnerable)
            {
                entity.InteractWithObject(nextGoal);
                return;
            }

            entity.InteractWithObject(entity.DefaultGoal);
        }

        public void Exit(UnitController entity)
        {
        }
    }
}