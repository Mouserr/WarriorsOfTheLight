using System;
using System.Collections.Generic;
using Assets.Scripts.Configs;
using Assets.Scripts.Models;

namespace Assets.Scripts.Units
{
    public class Hero : Unit
    {
        public event Action OnLevelUp;
        public event Action OnExpChange;
        public event Action OnAbilityPointsChange;

        public readonly List<Ability> Abilities;

        private readonly List<HeroLevel> heroLevels;
        private int currenExp;
        private int abilityPoints;

        public int ExpToLevelUp { get { return Level < MaxLevel ? heroLevels[Level + 1].ExpToLevelUp : int.MaxValue; } }
        public float ResurrectionTime { get { return heroLevels[Level].ResurrectionTime; } }
        public int Level { get; private set; }
        public int MaxLevel { get { return heroLevels.Count - 1; } }
       
        public int Experience
        {
            get
            {
                return currenExp;
            }
            set
            {
                if (Level >= MaxLevel) return;
                currenExp = value;

                if (currenExp > ExpToLevelUp)
                {
                    currenExp = currenExp - ExpToLevelUp;
                    Level++;
                    AbilityPoints++;
                    SetLevel(heroLevels[Level]);
                    HP = MaxHP;
                    
                    if (OnLevelUp != null)
                        OnLevelUp();
                }

                if (OnExpChange != null)
                    OnExpChange();
            }
        }

        public int AbilityPoints
        {
            get
            {
                return this.abilityPoints;
            }
            set
            {
                this.abilityPoints = value;

                if (OnAbilityPointsChange != null);
                    OnAbilityPointsChange();
            }
        }

        public Hero(string unitType, HeroConfig heroConfig, int playerId)
            : base(unitType, heroConfig.Levels[0], playerId)
        {
            heroLevels = heroConfig.Levels;
            Abilities = new List<Ability>();
            for (int i = 0; i < heroConfig.Abilities.Count; i++)
            {
                Abilities.Add(new Ability(heroConfig.Abilities[i], this));
            }
        }


        public override void OnKill(IDestroyable defender)
        {
            base.OnKill(defender);
            Mob mob = defender as Mob;
            if (mob != null)
            {
                Experience += mob.AwardExp;
            }
        }
    }
}