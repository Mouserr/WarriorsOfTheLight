using System;
using System.Collections.Generic;
using Assets.Events;
using Assets.Scripts.Configs;
using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Helpers;
using Assets.Scripts.Core.Scenarios;
using Assets.Scripts.Selection;
using Assets.Scripts.UI;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    public class BarrackController : MonoBehaviour, ISelectable
    {
        [SerializeField]
        private BarrackConfig config;
        [SerializeField]
        private int playerId;
        [SerializeField]
        private Transform spawnPoint;
        [SerializeField]
        private GameObject selection;
        [SerializeField]
        private ProgressBar spawnBar;

        private IScenarioItem spawnScenario;

        public event Action OnUpgrade;

        public int Level { get; private set; }
        public int MaxLevel { get { return config.Levels.Count - 1; }}

        public string Name
        {
            get
            {
                return string.Format("{0} barracks {1}", config.UnitType, Level + 1);
            }
        }

        public IDestroyable Destroyable { get { return null; } }

        public int UpgradeCost
        {
            get { return Level < MaxLevel ? config.Levels[Level + 1].UpgradeCost : 0; }
        }

        public bool CouldUpgrade
        {
            get
            {
                return Level < MaxLevel &&
                       PlayerManager.Instance.GetPlayer(playerId).GoldCount >= config.Levels[Level + 1].UpgradeCost;
            }
        }

        private void Awake()
        {
            UnitsPool.Instance.Register(config.UnitType, config.MinionPrefab, 30);
            MapController.Instance.RegisterBarracks(this);
            SetSelected(false);
        }

        private void OnDestroy()
        {
            if (spawnScenario != null)
                spawnScenario.Stop();
        }

        public void StartSpawn()
        {
            spawnScenario = generateSpawnScenario().Play();
        }
        
        public void InteractWithMapPoint(Vector3 point, bool force = false)
        {
        }

        public void InteractWithObject(IInteractableObject interactable, bool force = false)
        {
        }

        public void CancelActionPrepare()
        {
        }

        public void Upgrade()
        {
            if (CouldUpgrade)
            {
                bool result = PlayerManager.Instance.GetPlayer(playerId).TrySpendCoins(config.Levels[Level + 1].UpgradeCost);
                if (!result) return;
                Level++;
                GameEventManager.Instance.Raise(new UnitsLevelUpEvent(config.UnitType, config.Levels[Level].SpawningMinionsLevel));

                if (OnUpgrade != null)
                    OnUpgrade();
            }
        }

        public void SetSelected(bool selected)
        {
            if (selection)
                selection.SetActive(selected);
        }

       

        private IScenarioItem generateSpawnScenario()
        {
            spawnBar.SetValue(0);
            return new Scenario(
                new IterateItem(config.Levels[Level].SpawnTime, updateProgress),
                new ActionItem(spawn),
                new ActionItem(() =>
                {
                    spawnScenario = generateSpawnScenario().Play();
                })
            );
        }

        private void updateProgress(float time)
        {
            spawnBar.SetValue(1 - time / config.Levels[Level].SpawnTime);
        }

        private void spawn()
        {
            UnitController unitController = UnitsPool.Instance.AddUnitToMap(config.UnitType, config.Levels[Level].SpawningMinionsLevel, playerId);
            unitController.gameObject.AppendTo(gameObject);
            unitController.gameObject.SetActive(true);
            unitController.transform.position = spawnPoint.position;
        }
    }
}