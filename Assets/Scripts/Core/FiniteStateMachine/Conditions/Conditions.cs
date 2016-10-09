namespace Assets.Scripts.Core.FiniteStateMachine.Conditions
{
    public static class Conditions
    {
        public static IFSMCondition<T> Not<T>(IFSMCondition<T> condition)
        {
            return new NotFSMCondition<T>(condition);
        }

        public static IFSMCondition<T> Or<T>(params IFSMCondition<T>[] conditions)
        {
            return new OrComposedFSMCondition<T>(conditions);
        }
        
        public static IFSMCondition<T> And<T>(params IFSMCondition<T>[] conditions)
        {
            return new AndComposedFSMCondition<T>(conditions);
        }
    }
}