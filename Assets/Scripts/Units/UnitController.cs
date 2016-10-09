using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Assets.Scripts.Core.FiniteStateMachine;
using Assets.Scripts.Core.FiniteStateMachine.Conditions;
using Assets.Scripts.Selection;
using Assets.Scripts.UI;
using Assets.Scripts.Units.Logic;
using Assets.Scripts.Units.Logic.Conditions;
using Assets.Scripts.Units.Logic.States;
using UnityEngine;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    public class UnitController : MonoBehaviour, ISelectable, IInteractableObject
    {
        [HideInInspector]
        public bool HasNewOrder;
        [HideInInspector]
        public Vector3 CurrentDestination;
        [HideInInspector]
        public CharacterController CharacterController;

        [SerializeField]
        private float rotateTime;
        [SerializeField]
        private ProgressBar hPBar;
        [SerializeField]
        private GameObject selection;
        [SerializeField]
        private GameObject targetSign;

        protected Animator animator;

        private Unit unit;
        private IEnumerator currentAction;
        private FSMachine<UnitController> stateMachine;
        private IInteractableObject interactable;
        private bool selected;

        public virtual IInteractableObject DefaultGoal
        {
            get { return MapController.Instance.Foutain; }
        }

        public IInteractableObject Interactable
        {
            get { return interactable; }
            set
            {
                if (selected)
                {
                    if (interactable != null)
                    {
                        interactable.SetTargeted(false);
                    }
                }

                interactable = value;

                if (selected)
                {
                    if (interactable != null && !ReferenceEquals(interactable, this))
                    {
                        interactable.SetTargeted(true);
                    }
                }
            }
        }

        public Unit Unit
        {
            get { return unit; }
            set
            {
                setUnit(value);
            }
        }

        public virtual float Range
        {
            get { return Unit.Range; }
        }

        public int PlayerId
        {
            get { return Unit.PlayerId; }
        }

        public bool IsAlive
        {
            get { return Unit.HP > 0; }
        }

        public bool IsVulnerable { get; set; }

        public Vector3 Position
        {
            get { return transform.position; }
        }

        public bool HasForcedOrder { get; protected set; }

        public float BoundingRadius { get { return CharacterController.radius; }
        }
        
        public virtual string Name
        {
            get { return Unit.UnitType; }
        }

        public IDestroyable Destroyable
        {
            get { return Unit; }
        }
        
        protected virtual string defaultState
        {
            get { return UnitStates.Analyse; }
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            CharacterController = GetComponent<CharacterController>();
        }

        protected virtual void OnEnable()
        {
            stateMachine = new FSMachine<UnitController>(this, 
                getStates(), 
                getTransitions(),
                defaultState);
            SetSelected(false);
            SetTargeted(false);
        }

        public void OnDisable()
        {
            stateMachine = null;
            Interactable = null;
            
        }



        protected virtual List<IFSMState<UnitController>> getStates()
        {
            return new List<IFSMState<UnitController>>{
                new ActionState(), 
                new AnalyseState(), 
                new MoveToObjectState(), 
                new MoveToPointState()};
        }

        protected virtual List<FSMStateTransition<UnitController>> getTransitions()
        {
            return new List<FSMStateTransition<UnitController>>
            {
                new FSMStateTransition<UnitController>(null, UnitStates.MoveToPoint, new PointOrderCondition()),
                new FSMStateTransition<UnitController>(null, UnitStates.MoveToObject, new InteractOrderCondition()),
                new FSMStateTransition<UnitController>(UnitStates.MoveToPoint, UnitStates.Analyse, new DestinationReachedCondition()),
                new FSMStateTransition<UnitController>(UnitStates.MoveToObject, UnitStates.Action, new InteractableIsNearCondition()),
                new FSMStateTransition<UnitController>(UnitStates.Action, UnitStates.MoveToObject, 
                    Conditions.Not(new InteractableIsNearCondition())),
                new FSMStateTransition<UnitController>(UnitStates.Action, UnitStates.Analyse, new CanNotInteractCondition()),
                new FSMStateTransition<UnitController>(UnitStates.MoveToObject, UnitStates.Analyse, new CanNotInteractCondition() ),

                new FSMStateTransition<UnitController>(UnitStates.MoveToPoint, UnitStates.Analyse, 
                    Conditions.And(new IsNotForcedCondition(), new NotInteractingWithEnemyCondition(), new HasEnemiesCondition()) ),
                new FSMStateTransition<UnitController>(UnitStates.MoveToObject, UnitStates.Analyse, 
                    Conditions.And(new IsNotForcedCondition(), new NotInteractingWithEnemyCondition(), new HasEnemiesCondition()) ),
                
                new FSMStateTransition<UnitController>(UnitStates.Action, UnitStates.Analyse, 
                    Conditions.And(new IsNotForcedCondition(), new NotInteractingWithEnemyCondition(), new HasEnemiesCondition()) ),
            };
        }

        protected virtual void setUnit(Unit newUnit)
        {
            if (unit != null)
            {
                newUnit.OnHPChange -= onHPChange;
            }

            unit = newUnit;
            onHPChange();
            unit.OnHPChange += onHPChange;
        }

        private void Update()
        {
            stateMachine.Update();
            if (Unit != null)
                Unit.Update(Time.deltaTime);
        }

        public void StartMovement()
        {
            animator.SetFloat(AnimationVariables.Speed, Unit.Speed);
        }

        public void StopMovement()
        {
            animator.SetFloat(AnimationVariables.Speed, 0);
        }

        public virtual void InteractWithMapPoint(Vector3 point, bool force = false)
        {
            CurrentDestination = point;
            Interactable = null;
            HasNewOrder = true;
            HasForcedOrder = force;
        }

        public virtual void InteractWithObject(IInteractableObject interactable, bool force = false)
        {
            Interactable = interactable;
            HasNewOrder = true;
            HasForcedOrder = force;
        }

        public virtual void CancelActionPrepare()
        {
        }

        public virtual void SetSelected(bool selected)
        {
            this.selected = selected;

            if (selection)
                selection.SetActive(selected);

            if (Interactable != null)
            {
                Interactable.SetTargeted(selected);
            }
        }

        public void SetTargeted(bool targeted)
        {
            if (targetSign)
            {
                targetSign.SetActive(targeted);
            }
        }

        public virtual bool StartAttack()
        {
            UnitController defender = Interactable as UnitController;
            if (defender != null)
            {
                startAttackAnimation();
                return true;
            }
            return false;
        }

        private void onHPChange()
        {
            hPBar.SetValue(unit.HP / (float)unit.MaxHP); 
            if (!IsAlive) RemoveFromField();
        }

        public void RemoveFromField()
        {
            if (Interactable != null)
            {
                Interactable.SetTargeted(false);
            }
            
            setDead();
        }

        protected virtual void setDead()
        {
            unit.OnHPChange -= onHPChange;
            MapController.Instance.RemoveUnit(this);
            UnitsPool.Instance.ReleaseUnit(this);
        }

        public void OnAttackDone()
        {
            onAttackDone();
        }

        protected void startAttackAnimation()
        {
            animator.SetTrigger(AnimationVariables.Attack);
        }

        protected virtual void onAttackDone()
        {
            UnitController defender = Interactable as UnitController;
            if (defender)
            {
                BattleManager.Instance.Attack(Unit, defender.Unit);
            }
        }

    }
}