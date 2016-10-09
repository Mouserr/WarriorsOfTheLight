using System;
using System.Collections.Generic;
using Assets.Scripts.Buildings;
using Assets.Scripts.Core;
using Assets.Scripts.Selection;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts
{
    public class MapController : Singleton<MapController>
    {
        private readonly Dictionary<int, List<UnitController>> units = new Dictionary<int, List<UnitController>>();

        public event Action<int> NoMoreUnitsAtMap;

        public FountainController Foutain { get; private set; }
        public LightTreeController LightTree { get; private set; }
        private List<BarrackController> barracks = new List<BarrackController>();

        protected override void init()
        {
            base.init();
            gameObject.layer = LayerMask.NameToLayer("Gameplay");
        }

        public void AddUnit(UnitController unit)
        {
            List<UnitController> playerUnits;
            if (!units.TryGetValue(unit.PlayerId, out playerUnits))
            {
                playerUnits = new List<UnitController>();
                units[unit.PlayerId] = playerUnits;
            }
            unit.IsVulnerable = true;
            playerUnits.Add(unit);
        }

        public void RemoveUnit(UnitController unit)
        {
            List<UnitController> playerUnits;
            if (!units.TryGetValue(unit.PlayerId, out playerUnits))
            {
                return;
            }

            playerUnits.Remove(unit);
            if (playerUnits.Count == 0)
            {
                if (NoMoreUnitsAtMap != null)
                    NoMoreUnitsAtMap(unit.PlayerId);
            }
        }

        public bool HasAvailableEnemy(UnitController askingUnit)
        {
            foreach (KeyValuePair<int, List<UnitController>> playerUnits in units)
            {
                if (playerUnits.Key == askingUnit.PlayerId) continue;
                if (playerUnits.Value.Count > 0) return true;
            }

            return false;
        }

        public IInteractableObject GetNearestEnemy(UnitController askingUnit)
        {
            float minSqrDistance = float.MaxValue;
            UnitController nearestUnit = null;
            Vector3 position = askingUnit.Position;
            foreach (KeyValuePair<int, List<UnitController>> playerUnits in units)
            {
                if (playerUnits.Key == askingUnit.PlayerId) continue;
                foreach (UnitController unit in playerUnits.Value)
                {
                    float sqrDistance = (unit.Position - position).sqrMagnitude;
                    if (sqrDistance < minSqrDistance)
                    {
                        minSqrDistance = sqrDistance;
                        nearestUnit = unit;
                    }
                }
            }

            return nearestUnit;
        }

        public List<Unit> GetEnemiesInArea(Vector3 position, float radius, int playerId)
        {
            float sqrRadius = radius * radius;
            List<Unit> unitsInArea = new List<Unit>();
            foreach (KeyValuePair<int, List<UnitController>> playerUnits in units)
            {
                if (playerUnits.Key == playerId) continue;
                foreach (UnitController unit in playerUnits.Value)
                {
                    float sqrDistance = (unit.Position - position).sqrMagnitude;
                    if (sqrDistance < sqrRadius)
                    {
                        unitsInArea.Add(unit.Unit);
                    }
                }
            }

            return unitsInArea;
        }

        public void RegisterFountain(FountainController fountainController)
        {
            Foutain = fountainController;
        }

        public void RegisterLightTree(LightTreeController lightTreeController)
        {
            LightTree = lightTreeController;
        }

        public void RegisterBarracks(BarrackController barrack)
        {
            barracks.Add(barrack);
        }

        public void StartSpawn()
        {
            for (int i = 0; i < barracks.Count; i++)
            {
                barracks[i].StartSpawn();
            }
        }

        public List<ISelectable> GetAllUnitsInBounds(Bounds bounds, int playerId)
        {
            List<ISelectable> selectables = new List<ISelectable>();

            if (units.ContainsKey(playerId))
            {
                for (int i = 0; i < units[playerId].Count; i++)
                {
                    if (bounds.Contains(units[playerId][i].Position))
                        selectables.Add(units[playerId][i]);
                }
            }

            return selectables;
        }

        public void Clear()
        {
            Foutain.RemoveAll();
            barracks.Clear();
            foreach (KeyValuePair<int, List<UnitController>> playerUnits in units)
            {
                foreach (UnitController unit in playerUnits.Value)
                {
                    UnitsPool.Instance.ReleaseUnit(unit);
                }
                playerUnits.Value.Clear();
            }
            units.Clear();
        }

    }
}