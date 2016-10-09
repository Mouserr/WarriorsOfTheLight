using System;
using Assets.Scripts.Selection;
using Assets.Scripts.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HeroWidget : MonoBehaviour
    {
        [SerializeField]
        private ProgressBar HPbar;
        [SerializeField]
        private ProgressBar ExpBar;
        [SerializeField]
        private Image FaceImage;
        [SerializeField]
        private Sprite AliveTexture;
        [SerializeField]
        private Sprite DeadTexture;
        [SerializeField]
        private Text ResurrectionTimer;
        [SerializeField]
        private HeroController heroController;
        [SerializeField]
        private SelectionController selectionController;

        private bool isDead;

        private void Start()
        {
            heroController.Hero.OnHPChange += OnHPChange;
            heroController.Hero.OnExpChange += OnExpChange;
            MapController.Instance.Foutain.OnTimeTillResurrectionChanged += OnTimeTillResurrectionChanged;
            ResurrectionTimer.gameObject.SetActive(false);
            OnExpChange();
            OnHPChange();
        }

        public void OnDestroy()
        {
            if (MapController.Instance != null && MapController.Instance.Foutain != null)
                MapController.Instance.Foutain.OnTimeTillResurrectionChanged -= OnTimeTillResurrectionChanged;
        }

        private void OnTimeTillResurrectionChanged()
        {
            ResurrectionTimer.text = Mathf.CeilToInt(MapController.Instance.Foutain.TimeTillResurrection).ToString();
        }

        public void SelectHero()
        {
            selectionController.Select(heroController);
        }

        private void OnExpChange()
        {
            ExpBar.SetValue(heroController.Hero.Experience / (float)heroController.Hero.ExpToLevelUp);
        }

        private void OnHPChange()
        {
            HPbar.SetValue(heroController.Unit.HP / heroController.Unit.MaxHP);

            if (!isDead && heroController.Unit.HP <= 0)
            {
                FaceImage.sprite = DeadTexture;
                ResurrectionTimer.gameObject.SetActive(true);
            }
            else if (isDead && heroController.Unit.HP > 0)
            {
                FaceImage.sprite = AliveTexture;
                ResurrectionTimer.gameObject.SetActive(false);
            }

            isDead = heroController.Unit.HP <= 0;
        }
    }
}