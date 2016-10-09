using System;
using Assets.Scripts.Configs;
using Assets.Scripts.Selection;
using Assets.Scripts.UI;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    public class LightTreeController : MonoBehaviour, IInteractableObject, IDestroyable, ISelectable
    {
        [SerializeField] 
        private LightTreeConfig config;
        [SerializeField]
        private int playerId;
        [SerializeField]
        private CapsuleCollider capsuleCollider; 
        [SerializeField]
        private ProgressBar hPBar; 
        [SerializeField]
        private GameObject targetSign;

        private float currentHP;
        
        public event Action OnHPChange;

        public float HP
        {
            get { return currentHP; }
            set
            {
                currentHP = Mathf.Clamp(value, 0, MaxHP);
                hPBar.SetValue(currentHP / MaxHP);
                if (OnHPChange != null)
                    OnHPChange();
            }
        }

        public int MaxHP { get { return config.MaxHP; } }
        public float Armor { get { return 0; } }

        public int PlayerId
        {
            get { return playerId; }
        }

        public bool IsVulnerable { get { return true; } }
        public Vector3 Position { get { return transform.position; } }
        public float BoundingRadius { get { return capsuleCollider.radius; } }
        public float Range { get { return 0; } }

        public void SetTargeted(bool targeted)
        {
            targetSign.SetActive(targeted);
        }

        public bool IsAlive { get { return HP > 0; } }
        public string Name { get { return "LightTree"; } }
        public IDestroyable Destroyable { get { return this; } }

        private void Awake()
        {
            targetSign.SetActive(false);
            MapController.Instance.RegisterLightTree(this);
            HP = MaxHP;
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

        public void SetSelected(bool selected)
        {
        }
    }
}