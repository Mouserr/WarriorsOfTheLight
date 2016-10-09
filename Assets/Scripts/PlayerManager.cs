using System.Collections.Generic;
using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        private readonly Dictionary<int, Player> players = new Dictionary<int, Player>();

        public Player UserPlayer { get; private set; }

        public void AddPlayer(int playerId, bool userControl = false)
        {
            Player newPlayer = new Player(playerId);
            players.Add(playerId, newPlayer);
            if (userControl)
            {
                UserPlayer = newPlayer;
            }
        }

        public Player GetPlayer(int playerId)
        {
            Player player;
            if (!players.TryGetValue(playerId, out player))
            {
                Debug.LogError(string.Format("Can't find player {0}", playerId));
            }

            return player;
        }

        public void Clear()
        {
            players.Clear();
        }
    }
}