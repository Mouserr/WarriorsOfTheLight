using System;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class HeroLevel : UnitLevel
    {
        public int ExpToLevelUp;
        public float ResurrectionTime;
    }
}