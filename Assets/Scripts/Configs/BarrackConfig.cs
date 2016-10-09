using System.Collections.Generic;
using Assets.Scripts.Core.ConstantsContainers;
using Assets.Scripts.Models;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "BarrackConfig", menuName = "Configs/Barracks")]
    public class BarrackConfig : ScriptableObject
    {
        [ChooseFromList(typeof(UnitType))]
        public string UnitType;
        public UnitController MinionPrefab;
        public List<BarracksLevel> Levels;
    }
}