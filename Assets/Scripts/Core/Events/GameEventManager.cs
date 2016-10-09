using System;

namespace Assets.Scripts.Core.Events
{
    public class GameEventManager
    {
        private static GameEventManager instance = null;
        public static GameEventManager Instance
        {
            get { return instance ?? (instance = new GameEventManager()); }
        }

        private readonly GameEventCollection gameEventCollection = new GameEventCollection();
        
        private GameEventManager()
        {
        }

        public void AddListener<T>(GameEventCollection.EventDelegate<T> del) where T : IGameEvent
        {
            gameEventCollection.AddListener<T>(del);
        }

        public bool RemoveListener<T>(GameEventCollection.EventDelegate<T> del) where T : IGameEvent
        {
			return gameEventCollection.RemoveListener<T>(del);
        }

        public bool RemoveListener(Type gameEventType, Delegate del)
        {
            return gameEventCollection.RemoveListener(gameEventType, del);
        }

        public void Raise(IGameEvent e)
        {
            gameEventCollection.Raise(e);
        }
    }
}

