namespace Assets.Scripts.Core.FiniteStateMachine.Conditions
{
    public class NotFSMCondition<T> : IFSMCondition<T>
    {
        private readonly IFSMCondition<T> internalCondition;

        public NotFSMCondition(IFSMCondition<T> condition)
        {
            internalCondition = condition;
        }

        public bool IsTriggered(T entity)
        {
            return !internalCondition.IsTriggered(entity);
        }
    }
}