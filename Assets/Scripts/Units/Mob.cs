using Assets.Scripts.Models;

namespace Assets.Scripts.Units
{
    public class Mob : Unit
    {
        private MobLevel mobLevelInfo;

        public int AwardExp { get { return mobLevelInfo.AwardExp; } }
        public int AwardGold { get { return mobLevelInfo.AwardGold; } }


        public Mob(string unitType, MobLevel level, int playerId) 
            : base(unitType, level, playerId)
        {
            mobLevelInfo = level;
        }
    }
}