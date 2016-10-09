namespace Assets.Scripts.Core.FiniteStateMachine.Conditions
{
    public class AndComposedFSMCondition<T> : IFSMCondition<T>
    {
        protected IFSMCondition<T>[] internalConditions;

        public AndComposedFSMCondition(params IFSMCondition<T>[] conditions)
        {
            internalConditions = conditions;
        }

        public virtual bool IsTriggered(T entity)
        {
            bool allTriggered = true;
            for (int i = 0; i < internalConditions.Length; i++)
            {
                if (!internalConditions[i].IsTriggered(entity))
                {
                    allTriggered = false;
                    break;
                }
            }
            return allTriggered;
        }
    }
}