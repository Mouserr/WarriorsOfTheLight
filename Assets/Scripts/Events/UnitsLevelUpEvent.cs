using Assets.Scripts.Core.Events;
using Assets.Scripts.Models;

namespace Assets.Events
{
    public class UnitsLevelUpEvent : IGameEvent
    {
        public string UnitType { get; private set; }
        public UnitLevel Level { get; private set; }

        public UnitsLevelUpEvent(string unitType, UnitLevel level)
        {
            UnitType = unitType;
            Level = level;
        }
    }
}