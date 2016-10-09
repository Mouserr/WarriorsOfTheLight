using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.FiniteStateMachine
{
    public class FSMachine<T>
    {
        private readonly T owner;
        private IFSMState<T> currentState;
        private readonly Dictionary<string, IFSMState<T>> states;
        private readonly List<FSMStateTransition<T>> transitions;
        public event Action<string> OnStateChange;

        public FSMachine(T owner, List<IFSMState<T>> statesList, List<FSMStateTransition<T>> transitions, string initialState)
        {
            this.owner = owner;
            states = new Dictionary<string, IFSMState<T>>();
            for (int i = 0; i < statesList.Count; i++)
            {
                states[statesList[i].Name] = statesList[i];
            }
            this.transitions = transitions;
            changeState(initialState);
        }

        public void Update()
        {
            if (currentState == null) return;
            
            for (int i = 0; i < transitions.Count; i++)
            {
                if ((string.IsNullOrEmpty(transitions[i].StartState) 
                        || transitions[i].StartState == currentState.Name) 
                    && transitions[i].Condition.IsTriggered(owner))
                {
                    changeState(transitions[i].NextState);
                    break;
                }
            }
            currentState.Execute(owner);
        }

        private void changeState(string newStateName)
        {
            if (currentState != null)
                currentState.Exit(owner);
            currentState = states[newStateName];
            currentState.Enter(owner);

            if (OnStateChange != null)
                OnStateChange(currentState.Name);
        }
    }
}
