using System;
using System.Collections.Generic;
using Assets.Events;
using Assets.Scripts.Configs;
using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Scenarios;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Unit : IDestroyable, IAttacker
    {
        public event Action OnHPChange;
        
        private UnitLevel levelInfo;
        private float currentHP;
        private float cooldownTime;
        private IGameEventListener levelUpListener;
        private List<IScenarioItem> currentEffects;
        
        public string UnitType { get; private set; }
        public int PlayerId { get; private set; }

        public float HP
        {
            get { return currentHP; }
            set
            {
                currentHP = Mathf.Clamp(value, -0.001f, MaxHP);
                if (OnHPChange != null)
                    OnHPChange();
            }
        }

        public int MaxHP { get; set; }
        public float Armor { get; set; }
        public float Attack { get; set;  }
        public float AttackSpeed { get; set; }
        public float Range { get; set; }
        public float Speed { get; set; }

        
        public bool CanAttack
        {
            get { return cooldownTime <= 0; }
        }

        public Unit(string unitType, UnitLevel level, int playerId)
        {
            UnitType = unitType;
            PlayerId = playerId;
            SetLevel(level);
            HP = MaxHP;
            levelUpListener = new GameEventListener<UnitsLevelUpEvent>(onLevelUp);
            levelUpListener.StartListening();
            currentEffects = new List<IScenarioItem>();
        }

        public void Clear()
        {
            levelUpListener.StopListening();
            foreach (IScenarioItem currentEffect in currentEffects)
            {
                currentEffect.Stop();
            }
        }

        private void onLevelUp(UnitsLevelUpEvent e)
        {
            if (e.UnitType == UnitType)
            {
                SetLevel(e.Level);
            }
        }

        public void SetLevel(UnitLevel level)
        {
            levelInfo = level;
            MaxHP = levelInfo.MaxHP;
            Armor = levelInfo.Armor;
            Attack = levelInfo.Attack;
            AttackSpeed = levelInfo.AttackSpeed;
            Range = levelInfo.Range;
            Speed = levelInfo.Speed;
        }

        public void ProduceAttack()
        {
            cooldownTime = AttackSpeed;
        }

        public virtual void OnKill(IDestroyable defender)
        {
        }

        public void Update(float deltaTime)
        {
            if (cooldownTime > 0)
                cooldownTime -= deltaTime;
        }

        public void ApplyModifier(StatModifier statModifier)
        {
            MaxHP += statModifier.MaxHP;
            Attack += statModifier.Attack;
            AttackSpeed = Mathf.Max(0, AttackSpeed + statModifier.AttackSpeed);
            Armor = Mathf.Clamp01(Armor + statModifier.Armor);
            Speed += statModifier.Speed;
            Range += statModifier.Range;
        }

        public void AddEffect(StatModifier statModifier, float duration, IScenarioItem effectScenario)
        {
            ApplyModifier(statModifier);
            currentEffects.Add(new Scenario(
                new CompositeItem
                (
                    new IterateItem(duration),
                    effectScenario
                ),
                new ActionItem(() => ApplyModifier(-statModifier))
                ).Play());
        }
        
    }
}