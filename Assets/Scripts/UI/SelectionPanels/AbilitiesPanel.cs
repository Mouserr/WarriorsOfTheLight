using System.Collections.Generic;
using Assets.Scripts.Selection;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.UI.SelectionPanels
{
    public class AbilitiesPanel : AbstractSelectionPanel<HeroController>
    {
        [SerializeField] 
        private SelectionController selectionController;

        private List<AbilityButton> buttons;
        private bool inited;

        private void init()
        {
            buttons = new List<AbilityButton>(GetComponentsInChildren<AbilityButton>(true));
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Init(selectionController);
            }

            inited = true;
        }

        protected override void updateData()
        {

            for (int i = 0; i < currentSelectable.Hero.Abilities.Count; i++)
            {
                if (buttons.Count > i)
                {
                    buttons[i].UpdateData(currentSelectable.Hero.Abilities[i], currentSelectable);
                }
            }
        }

        protected override void beforeUpdateData()
        {
            if (!inited) init();
        }
    }
}