using Assets.Scripts.Buildings;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class MobController : UnitController
    {
        public override IInteractableObject DefaultGoal
        {
            get { return MapController.Instance.LightTree; }
        }

        public override bool StartAttack()
        {
            if (base.StartAttack()) return true;

            LightTreeController lightTree = Interactable as LightTreeController;
            if (lightTree != null)
            {
                startAttackAnimation();
                return true;
            }

            return false;
        }

        protected override void onAttackDone()
        {
            LightTreeController lightTree = Interactable as LightTreeController;
            if (lightTree != null)
            {
                BattleManager.Instance.Attack(Unit, lightTree);
            }
            else
            {
                base.onAttackDone();
            }
        }
    }
}