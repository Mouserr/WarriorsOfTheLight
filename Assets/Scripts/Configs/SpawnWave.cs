using System;
using System.Collections.Generic;
using Assets.Scripts.Core.ConstantsContainers;
using Assets.Scripts.Units;

namespace Assets.Scripts.Configs
{
    [Serializable]
    public class SpawnWave
    {
        [ChooseFromList(typeof(UnitType))]
        public List<string> Mobs;

        public float PrepareTime;
    }
}