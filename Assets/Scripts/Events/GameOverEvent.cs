using System.IO;
using Assets.Scripts;
using Assets.Scripts.Core.Events;

namespace Assets.Events
{
    public class GameOverEvent : IGameEvent
    {
        public GameResult Result { get; private set; }

        public GameOverEvent(GameResult result)
        {
            Result = result;
        }
    }
}