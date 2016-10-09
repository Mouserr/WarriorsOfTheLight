using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Configs;
using Assets.Scripts.Core.Scenarios;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    public class FountainController : MonoBehaviour, IInteractableObject
    {
        [SerializeField]
        private Transform resurrectionPoint;
        [SerializeField]
        private FountainConfig config;
        [SerializeField]
        private CapsuleCollider modelCollider;
        [SerializeField]
        private GameObject targetSign;

        public event Action OnTimeTillResurrectionChanged;


        public float TimeTillResurrection
        {
            get { return timeTillResurrection; }
            private set
            {
                timeTillResurrection = value;
                if (OnTimeTillResurrectionChanged != null) 
                    OnTimeTillResurrectionChanged();
            }
        }

        private float timeTillResurrection;
        private IScenarioItem resurrectionScenario;
        private List<UnitController> units = new List<UnitController>();

        public int PlayerId { get { return PlayerManager.Instance.UserPlayer.PlayerId; } }
        public bool IsAlive { get { return true; } }
        public bool IsVulnerable { get { return false; } }
        public Vector3 Position { get { return transform.position; } }
        public float BoundingRadius { get { return modelCollider.radius; } }
        public float Range { get { return 0; } }

        private void Awake()
        {
            MapController.Instance.RegisterFountain(this);
            targetSign.SetActive(false);
        }

        public void ResurrectHero(HeroController heroController)
        {
            heroController.gameObject.SetActive(false);
            heroController.transform.position = resurrectionPoint.position;
            TimeTillResurrection = (heroController.Unit as Hero).ResurrectionTime;
            resurrectionScenario = new Scenario(
                new IterateItem(TimeTillResurrection, (leftTime) => { TimeTillResurrection = leftTime; }),
                new ActionItem(() =>
                {
                    heroController.Unit.HP = heroController.Unit.MaxHP;
                    heroController.gameObject.SetActive(true);
                })
            ).Play();
        }

        public void AddToSaveZone(UnitController unitController)
        {
            if (unitController.PlayerId != PlayerId) return;
            unitController.IsVulnerable = false;
            units.Add(unitController);
            MapController.Instance.RemoveUnit(unitController);
        }

        public void RemoveFromSaveZone(UnitController unitController)
        {
            if (unitController.PlayerId != PlayerId) return;
            units.Remove(unitController);
            MapController.Instance.AddUnit(unitController);
        }

        public void RemoveAll()
        {
            while (units.Count > 0)
            {
                RemoveFromSaveZone(units[0]);
            }
        }

        private void Update()
        {
            for (int i = 0; i < units.Count; i++)
            {
                units[i].Unit.HP += (units[i].Unit is Hero ? config.HeroHealingSpeed : config.MinionsHealingSpeed) * Time.deltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            UnitController unitController = other.GetComponent<UnitController>();
            if (unitController)
            {
                AddToSaveZone(unitController);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            UnitController unitController = other.GetComponent<UnitController>();
            if (unitController)
            {
                RemoveFromSaveZone(unitController);
            }
        }

        public void SetTargeted(bool targeted)
        {
            targetSign.gameObject.SetActive(targeted);
        }
    }
}