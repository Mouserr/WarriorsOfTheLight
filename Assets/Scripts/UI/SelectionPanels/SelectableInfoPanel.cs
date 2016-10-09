using Assets.Scripts.Selection;
using Assets.Scripts.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.SelectionPanels
{
    public class SelectableInfoPanel : AbstractSelectionPanel<ISelectable>
    {
        [SerializeField] private Text nameLabel;
        [SerializeField] private ProgressBar hpBar;
        [SerializeField] private Text hpLabel;

        private IDestroyable currentDestroyable;

        protected override void updateData()
        {
            currentDestroyable = currentSelectable.Destroyable;
            nameLabel.text = currentSelectable.Name;

            if (currentDestroyable != null)
            {
                hpBar.gameObject.SetActive(true);
                currentDestroyable.OnHPChange += updateHP;
                updateHP();
            }
            else
            {
                hpBar.gameObject.SetActive(false);
            }
        }

        protected override void beforeUpdateData()
        {
            if (currentDestroyable != null)
            {
                currentDestroyable.OnHPChange -= updateHP;
            }
        }

        private void updateHP()
        {
            hpLabel.text = string.Format("{0} / {1}", Mathf.FloorToInt(currentDestroyable.HP), currentDestroyable.MaxHP);
            hpBar.SetValue(currentDestroyable.HP / (float)currentDestroyable.MaxHP);
        }
    }
}