namespace Assets.Scripts.Units
{
    public class StatModifier
    {
        public int MaxHP { get; set; }
        public float Armor { get; set; }
        public float Attack { get; set; }
        public float AttackSpeed { get; set; }
        public float Range { get; set; }
        public float Speed { get; set; } 

        public static StatModifier operator -(StatModifier x)
        {
            return new StatModifier
            {
                MaxHP = -x.MaxHP,
                Armor = -x.Armor,
                Attack = -x.Attack,
                AttackSpeed = -x.AttackSpeed,
                Range = -x.Range,
                Speed = -x.Speed,
            };
        }
    }
}