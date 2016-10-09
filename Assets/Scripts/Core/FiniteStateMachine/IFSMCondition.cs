namespace Assets.Scripts.Core.FiniteStateMachine
{
    public interface IFSMCondition<T>
    {
        bool IsTriggered(T entity);
    }
}