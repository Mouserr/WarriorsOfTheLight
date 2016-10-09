using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "MobsSpawnConfig", menuName = "Configs/MobsSpawn")]
    public class MobsSpawnConfig : ScriptableObject
    {
        public List<MobConfig> MobConfigs;
     
        public List<SpawnWave> Waves;
    }
}