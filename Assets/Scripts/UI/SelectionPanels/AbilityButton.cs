using System;
using Assets.Scripts.Selection;
using Assets.Scripts.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.SelectionPanels
{
    public class AbilityButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private Button levelUpButton;
        [SerializeField]
        private Text shortcut;
        [SerializeField]
        private KeyCode shortcutKey;
        [SerializeField]
        private Text level;
        [SerializeField]
        private Text cooldown;

        private Ability ability;
        private HeroController heroController;
        private SelectionController selectionController;

        public void Init(SelectionController selectionController)
        {
            this.selectionController = selectionController;
        }

        public void UpdateData(Ability ability, HeroController hero)
        {
            if (heroController != null)
            {
                heroController.Hero.OnAbilityPointsChange -= updateLevelWidgets;
            }

            this.ability = ability;
            heroController = hero;
            button.image.sprite = ability.Config.Icon;
            updateLevelWidgets();

            heroController.Hero.OnAbilityPointsChange += updateLevelWidgets;
        }

        private void updateLevelWidgets()
        {
            level.text = (ability.Level + 1).ToString();
            levelUpButton.gameObject.SetActive(CouldLevelUp);
            button.interactable = ability.CouldCast && !CouldLevelUp;
        }

        public void LevelUp()
        {
            if (CouldLevelUp)
            {
                ability.LevelUp();
            }
        }

        public void StartCast()
        {
            if (heroController.SelectedAbility != null || !ability.CouldCast) return;
            selectionController.SetCastingAbility(ability);
            heroController.SelectedAbility = ability;
        }

        private void Update()
        {
            if (ability.CouldCast)
            {
                button.interactable = true;
                cooldown.gameObject.SetActive(false);
                return;
            }

            cooldown.gameObject.SetActive(true);
            button.interactable = false;
            cooldown.text = Mathf.CeilToInt(ability.Cooldown).ToString();
        }

        private bool CouldLevelUp
        {
            get { return heroController.Hero.AbilityPoints > 0 && ability.Level < ability.Config.MaxLevel; }
        }
    }
}