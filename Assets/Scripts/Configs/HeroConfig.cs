using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "HeroConfig", menuName = "Configs/Hero")]
    public class HeroConfig : ScriptableObject
    {
        public List<HeroLevel> Levels;

        public List<ActiveAbilityConfig> Abilities;
    }
}