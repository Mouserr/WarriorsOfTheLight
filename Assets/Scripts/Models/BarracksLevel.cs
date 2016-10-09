using System;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class BarracksLevel
    {
        public UnitLevel SpawningMinionsLevel;
        public float SpawnTime;
        public int UpgradeCost;
    }
}