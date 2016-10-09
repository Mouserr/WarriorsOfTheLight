using System;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class UnitLevel
    {
        public int MaxHP;
        [Range(0,1)]
        public float Armor;
        public float Attack;
        public float AttackSpeed;
        public float Speed;
        public float Range;
    }
}