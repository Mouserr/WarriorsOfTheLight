namespace Assets.Scripts.Units
{
    public interface IAttacker
    {
        int PlayerId { get; }
        float Attack { get; }
        bool CanAttack { get; }
        void ProduceAttack();
        void OnKill(IDestroyable defender);
    }
}