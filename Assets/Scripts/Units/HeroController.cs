using System;
using System.Collections.Generic;
using Assets.Scripts.Configs;
using Assets.Scripts.Core.FiniteStateMachine;
using Assets.Scripts.Core.FiniteStateMachine.Conditions;
using Assets.Scripts.Models;
using Assets.Scripts.Units.Logic.Conditions;
using Assets.Scripts.Units.Logic.States;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class HeroController : UnitController
    {
        [SerializeField]
        private ParticleSystem levelUpEffect; 
        [SerializeField]
        private Transform castStartTransform;
        [SerializeField]
        private GameObject castRange;

        private Ability abilityToCast;
        private Ability selectedAbility;
        private CastContext currentCastContext;
        private bool shouldComeCloserToCast;


        public Hero Hero
        {
            get
            {
                return Unit as Hero;
            }
        }

        public override float Range
        {
            get
            {
                if (SelectedAbility != null)
                {
                    ICastingRangeProvider rangeProvider = SelectedAbility.Config as ICastingRangeProvider;
                    if (rangeProvider != null)
                        return rangeProvider.GetRange(SelectedAbility.Level);
                }
             
                return base.Range;
            }
        }

        public override string Name
        {
            get { return string.Format("{0} {1}", Unit.UnitType, Hero.Level + 1) ; }
        }

        protected override string defaultState
        {
            get { return UnitStates.Idle; }
        }

        public Ability SelectedAbility
        {
            get { return selectedAbility; }
            set
            {
                selectedAbility = value;
                shouldComeCloserToCast = false;

                if (selectedAbility != null)
                {
                    ICastingRangeProvider rangeProvider = selectedAbility.Config as ICastingRangeProvider;
                    if (rangeProvider != null)
                    {
                        float range = rangeProvider.GetRange(selectedAbility.Level);
                        castRange.transform.localScale = Vector3.one*range;
                        castRange.SetActive(true);
                        return;
                    }
                }
                
                castRange.SetActive(false);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            castRange.SetActive(SelectedAbility != null);   
        }

        public void OnCastReady()
        {
            abilityToCast.Apply(currentCastContext);
        }

        public override void CancelActionPrepare()
        {
            if (!shouldComeCloserToCast)
            {
                SelectedAbility = null;
            }
        }

        public override void SetSelected(bool selected)
        {
            if (!selected) SelectedAbility = null;
            base.SetSelected(selected);
        }

        public override bool StartAttack()
        {
            if (SelectedAbility == null)
                return base.StartAttack();

            InteractWithObject(Interactable, true);
            return true;
        }

        public override void InteractWithObject(IInteractableObject interactable, bool force = false)
        {
            if (shouldComeCloserToCast && interactable != Interactable)
            {
                SelectedAbility = null;
            }
            else
            {
                shouldComeCloserToCast = false;
            }

            if (!shouldComeCloserToCast && SelectedAbility != null && interactable is UnitController)
            {
                
                ICastingRangeProvider rangeProvider = SelectedAbility.Config as ICastingRangeProvider;
                if (rangeProvider != null)
                {
                    float range = rangeProvider.GetRange(SelectedAbility.Level);

                    if ((interactable.Position - Position).magnitude > range)
                    {
                        shouldComeCloserToCast = true;
                        base.InteractWithObject(interactable, force);
                        return;
                    }
                }

                CastContext context = new CastContext
                {
                    CasterPoint = castStartTransform.position,
                    MainTarget = (interactable as UnitController).Unit,
                    TargetPoint = interactable.Position,
                    TargetTransform = (interactable as UnitController).transform,
                };
                cast(context);
                
                base.InteractWithObject(this, true);
            }
            else
            {
                base.InteractWithObject(interactable, force);
            }

            SelectedAbility = null;
        }

        public override void InteractWithMapPoint(Vector3 point, bool force = false)
        {
            if (!shouldComeCloserToCast && SelectedAbility != null)
            {
                if (SelectedAbility.Config.TargetType == TargetType.Single) return;

                CastContext context = new CastContext
                {
                    CasterPoint = transform.position,
                    TargetPoint = point
                };

                cast(context);
                base.InteractWithObject(this, true);
            }
            else
            {
                base.InteractWithMapPoint(point, force);
            }

            SelectedAbility = null;
        }

        protected override List<IFSMState<UnitController>> getStates()
        {
            return new List<IFSMState<UnitController>>{
                new ActionState(), 
                new IdleState(), 
                new MoveToObjectState(), 
                new MoveToPointState()};
        }

        protected override List<FSMStateTransition<UnitController>> getTransitions()
        {
            return new List<FSMStateTransition<UnitController>>
            {
                new FSMStateTransition<UnitController>(null, UnitStates.MoveToPoint, new PointOrderCondition()),
                new FSMStateTransition<UnitController>(null, UnitStates.MoveToObject, new InteractOrderCondition()),
                new FSMStateTransition<UnitController>(UnitStates.MoveToPoint, UnitStates.Idle, new DestinationReachedCondition()),
                new FSMStateTransition<UnitController>(UnitStates.MoveToObject, UnitStates.Action, new InteractableIsNearCondition()),
                new FSMStateTransition<UnitController>(UnitStates.Action, UnitStates.MoveToObject, Conditions.Not(new InteractableIsNearCondition())),
                new FSMStateTransition<UnitController>(UnitStates.Action, UnitStates.Idle, new CanNotInteractCondition()),
                new FSMStateTransition<UnitController>(UnitStates.MoveToObject, UnitStates.Idle, new CanNotInteractCondition()),
                
            };
        }

        protected override void setUnit(Unit newUnit)
        {
            base.setUnit(newUnit);
            Hero.OnLevelUp += onLevelUp;
        }

        protected override void setDead()
        {
            MapController.Instance.RemoveUnit(this);
            MapController.Instance.Foutain.ResurrectHero(this);
        }

        private void cast(CastContext castContext)
        {
            if (!SelectedAbility.Config.CouldApply(castContext, SelectedAbility.Level, Hero)) return;
            currentCastContext = castContext;
            abilityToCast = SelectedAbility;
            animator.SetTrigger(AnimationVariables.Cast);
            transform.rotation = Quaternion.LookRotation(castContext.TargetPoint - transform.position);
        }

        private void onLevelUp()
        {
            levelUpEffect.Play();
        }
    }
}