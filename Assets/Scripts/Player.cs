using System;

namespace Assets.Scripts
{
    public class Player
    {
        public event Action OnGoldCountChange;
        
        public int PlayerId { get; private set; }
        private int goldCount;
        public int GoldCount
        {
            get { return goldCount; }
            set
            {
                goldCount = value;
                if (OnGoldCountChange != null)
                    OnGoldCountChange();
            }
        }

        public Player(int id)
        {
            PlayerId = id;
        }

        public bool TrySpendCoins(int price)
        {
            if (GoldCount < price)
            {
                return false;
            }

            GoldCount -= price;
            return true;
        }

        public void ProcessAward(int gold)
        {
            GoldCount += gold;
        }
    }
}