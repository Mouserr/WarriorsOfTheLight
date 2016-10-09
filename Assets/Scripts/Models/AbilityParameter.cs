using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class FloatAbilityParameter : AbilityParameter<float>
    {
        
    }

    
    public abstract class AbilityParameter<T> : IAbilityParameter
    {
        public List<T> Levels;

        public int MaxLevel { get { return Levels.Count - 1; } }

        public T GetValue(int level)
        {
            if (Levels.Count == 0) return default(T);

            return Levels[Math.Min(level, MaxLevel)];
        }
    }

    public interface IAbilityParameter
    {
        int MaxLevel { get; }
    }
}