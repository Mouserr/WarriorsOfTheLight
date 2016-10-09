using System;
using Assets.Scripts.Core.ConstantsContainers;
using Assets.Scripts.Models;
using Assets.Scripts.Units;

namespace Assets.Scripts.Configs
{
    [Serializable]
    public class MobConfig
    {
        [ChooseFromList(typeof(UnitType))]
        public string UnitType;
        public UnitController Prefab;
        public MobLevel Level;
    }
}