namespace Assets.Scripts.Core.Events
{
    public class GameEventListener<T> : IGameEventListener where T : IGameEvent
    {
        private readonly GameEventCollection.EventDelegate<T> eventDelegate;
    
        public GameEventListener(GameEventCollection.EventDelegate<T> eventDelegate)
        {
            this.eventDelegate = eventDelegate;
        }

        public void StartListening()
        {
            GameEventManager.Instance.AddListener<T>(eventDelegate);
        }

        public void StopListening()
        {
            GameEventManager.Instance.RemoveListener<T>(eventDelegate);
        }
    }
}
