

namespace Assets.Scripts.Core.FiniteStateMachine
{
    public class FSMStateTransition<T>
    {
        public string StartState { get; private set; }
        public string NextState { get; private set; }
        public IFSMCondition<T> Condition { get; private set; }

        public FSMStateTransition(string startState, string nextState, IFSMCondition<T> condition)
        {
            StartState = startState;
            NextState = nextState;
            Condition = condition;
        }
    }
}

