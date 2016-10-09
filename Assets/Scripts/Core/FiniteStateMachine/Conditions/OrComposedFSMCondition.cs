namespace Assets.Scripts.Core.FiniteStateMachine.Conditions
{
    public class OrComposedFSMCondition<T> : AndComposedFSMCondition<T>
    {
        public OrComposedFSMCondition(params IFSMCondition<T>[] conditions) :
            base(conditions)
        {
        }

        public override bool IsTriggered(T entity)
        {
            bool oneTriggered = false;
            for (int i = 0; i < internalConditions.Length; i++)
            {
                if (!internalConditions[i].IsTriggered(entity))
                {
                    oneTriggered = true;
                    break;
                }
            }
            return oneTriggered;
        }
    }
}