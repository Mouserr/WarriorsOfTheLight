namespace Assets.Scripts.Core.Events
{
    public interface IGameEventListener
    {
        void StartListening();

        void StopListening();
    }
}
