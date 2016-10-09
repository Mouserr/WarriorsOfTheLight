using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.Core.Helpers;
using Assets.Scripts.Core.Pools;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class UnitsPool : Singleton<UnitsPool>
    {
        private readonly Dictionary<string, GameObjectPool<UnitController>> pools = new Dictionary<string, GameObjectPool<UnitController>>();

        public void Register(string unitType, UnitController prefab, int startCount)
        {
            if (!pools.ContainsKey(unitType))
            {
                pools[unitType] = new GameObjectPool<UnitController>(gameObject.AddChild(unitType), prefab, startCount);
            }
        }

        public UnitController AddUnitToMap(string unitType, UnitLevel level, int playerId)
        {
            UnitController unitController = GetUnitPrefab(unitType);
            Unit spawnUnit = BattleManager.Instance.SpawnUnit(unitType, level, playerId);
            unitController.Unit = spawnUnit;
            MapController.Instance.AddUnit(unitController);
            
            return unitController;
        }

        public UnitController GetUnitPrefab(string unitType)
        {
            GameObjectPool<UnitController> unitPool;
            if (!pools.TryGetValue(unitType, out unitPool))
            {
                Debug.LogError(string.Format("Can't find pool for {0}", unitType));
                return null;
            }

            return unitPool.GetObject();
        }

        public void ReleaseUnit(UnitController unitController)
        {
            GameObjectPool<UnitController> unitPool;
            if (!pools.TryGetValue(unitController.Unit.UnitType, out unitPool))
            {
                Debug.LogError(string.Format("Can't find pool for {0}", unitController.Unit.UnitType));
                return;
            }
            unitController.Unit.Clear();
            unitPool.ReleaseObject(unitController);
        }

        public void Clear()
        {
            foreach (KeyValuePair<string, GameObjectPool<UnitController>> gameObjectPool in pools)
            {
                gameObjectPool.Value.ClearPull();
            }
            pools.Clear();
        }
    }
}