

namespace Assets.Scripts.Core.FiniteStateMachine
{
    public interface IFSMState<T>
    {
        string Name { get; }
        void Enter(T entity);	
        void Execute(T entity);
        void Exit(T entity);
    }
}