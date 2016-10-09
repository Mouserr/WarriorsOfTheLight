using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Configs;
using Assets.Scripts.Core.Helpers;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class MobsSpawnController : MonoBehaviour
    {
        [SerializeField]
        private int playerId;
        [SerializeField]
        private MobsSpawnConfig spawnConfig;
        [SerializeField]
        private List<Transform> spawnPoints;
        [SerializeField]
        private float spawnCooldown; 

        private int currentIndex;
        private readonly Dictionary<string, MobLevel> mobLevels = new Dictionary<string, MobLevel>();

        public bool SpawnEnded { get; private set; }

        private void Awake()
        {
            foreach (MobConfig mobConfig in spawnConfig.MobConfigs)
            {
                UnitsPool.Instance.Register(mobConfig.UnitType, mobConfig.Prefab, 30);
                mobLevels[mobConfig.UnitType] = mobConfig.Level;
            }
        }

        public void StartSpawn()
        {
            StartCoroutine(spawnCoroutine());
        }
        
        private IEnumerator spawnCoroutine()
        {
            for (currentIndex = 0; currentIndex < spawnConfig.Waves.Count; currentIndex++)
            {
                yield return StartCoroutine(waveCoroutine());
            }

            SpawnEnded = true;
        }

        private IEnumerator waveCoroutine()
        {
            SpawnWave spawnWave = spawnConfig.Waves[currentIndex];
            yield return new WaitForSeconds(spawnWave.PrepareTime);
            int spawnIndex = 0;

            while (spawnIndex < spawnWave.Mobs.Count)
            {
                for (int j = 0; j < spawnPoints.Count; j++)
                {
                    spawn(spawnWave.Mobs[spawnIndex], spawnPoints[j].position);
                    spawnIndex++;
                    if (spawnIndex >= spawnWave.Mobs.Count) yield break;
                }    
                yield return new WaitForSeconds(spawnCooldown);
            }
        }

        private void spawn(string unitType, Vector3 position)
        {
            if (!mobLevels.ContainsKey(unitType)) return;
            UnitController unitController = UnitsPool.Instance.AddUnitToMap(unitType, mobLevels[unitType], playerId);
          
            unitController.gameObject.AppendTo(gameObject);
            unitController.gameObject.SetActive(true);
           
            unitController.transform.position = position;
        }

    }
}