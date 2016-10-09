using System;
using System.Collections.Generic;
using Assets.Scripts.Core.Scenarios;
using Assets.Scripts.Models;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    public abstract class AbilityConfig : ScriptableObject
    {
        public TargetType TargetType;
        public Sprite Icon;

        public abstract IScenarioItem Apply(CastContext castContext, int abilityLevel, Hero caster);
        protected abstract List<IAbilityParameter> getParameters();

        private int maxLevel;

        public int MaxLevel
        {
            get { return maxLevel; }
        }

        protected virtual void OnEnable()
        {
            List<IAbilityParameter> parameters = getParameters();
            maxLevel = 0;
            for (int i = 0; i < parameters.Count; i++)
            {
                maxLevel = Math.Max(maxLevel, parameters[i].MaxLevel);
            }
        }

        public abstract void Register();
    }

    public abstract class ActiveAbilityConfig : AbilityConfig
    {
        public FloatAbilityParameter Cooldown;

        public abstract bool CouldApply(CastContext castContext, int abilityLevel, Hero caster);
    }
    
    public interface ICastingAreaProvider
    {
        float GetRadius(int abilityLevel);
    }

    public interface ICastingRangeProvider
    {
        float GetRange(int abilityLevel);
    }
}