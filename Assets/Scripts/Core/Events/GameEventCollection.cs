using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Events
{
    public class GameEventCollection
    {
        public delegate void EventDelegate<T>(T e) where T : IGameEvent;
        public delegate void EventDelegate(IGameEvent e);
        public bool IsLocked;

        private readonly Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();
        private readonly Dictionary<string, EventDelegate> delegateLookup = new Dictionary<string, EventDelegate>();


        public void AddListener<T>(EventDelegate<T> del) where T : IGameEvent
        {
            Type gameEventType = typeof(T);
            var key = getKey(del, gameEventType);
            if (delegateLookup.ContainsKey(key))
            {
                return;
            }

            // Create a new non-generic delegate which calls our generic one.
            // This is the delegate we actually invoke.
            EventDelegate internalDelegate = (e) => del((T)e);
		
            delegateLookup[key] = internalDelegate;

            EventDelegate tempDel;
	   
            if (delegates.TryGetValue(gameEventType, out tempDel))
            {
                delegates[gameEventType] = tempDel += internalDelegate;
            }
            else
            {
                delegates[gameEventType] = internalDelegate;
            }
        }

        public bool RemoveListener(Type gameEventType, Delegate del)
        {
            EventDelegate internalDelegate;
            string key = getKey(del, gameEventType);
            if (delegateLookup.TryGetValue(key, out internalDelegate))
            {
                EventDelegate tempDel;
                Type gameEvenType = gameEventType;
                if (delegates.TryGetValue(gameEvenType, out tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                    {
                        delegates.Remove(gameEvenType);
                    }
                    else
                    {
                        delegates[gameEvenType] = tempDel;
                    }
                }
                delegateLookup.Remove(key);
				return true;
            }

			return false;
        }

        public bool RemoveListener<T>(EventDelegate<T> del) where T : IGameEvent
        {
            return RemoveListener(typeof(T), del);
        }

        public void Raise(IGameEvent e)
        {
            if (!IsLocked)
            {
                EventDelegate del;
                if (delegates.TryGetValue(e.GetType(), out del))
                {
                    del.Invoke(e);
                }
            }
        }

        private static string getKey(Delegate del, Type gameEventType)
        {
            return del.GetHashCode().ToString() + gameEventType;
        }
    }
}