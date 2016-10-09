using Assets.Scripts.Buildings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.SelectionPanels
{
    public class BarrackActionsPanel : AbstractSelectionPanel<BarrackController>
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private Text costLabel;
        [SerializeField]
        private SelectableInfoPanel infoPanel;

        protected override void beforeUpdateData()
        {
            if (currentSelectable != null)
            {
                currentSelectable.OnUpgrade -= updateValues;
            }

        }

        protected override void updateData()
        {
            currentSelectable.OnUpgrade += updateValues;
            updateValues();
        }

        public void Upgrade()
        {
            currentSelectable.Upgrade();
        }

        private void updateValues()
        {
            button.gameObject.SetActive(currentSelectable.Level <= currentSelectable.MaxLevel);
            button.interactable = currentSelectable.CouldUpgrade;

            setLabel(costLabel, currentSelectable.UpgradeCost > 0 ? currentSelectable.UpgradeCost.ToString() : null);
            infoPanel.UpdateData(currentSelectable);
        }

        private void setLabel(Text label, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                label.gameObject.SetActive(true);
                label.text = value;
            }
            else
            {
                label.gameObject.SetActive(false);
            }
        }
    }
}