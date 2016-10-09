using Assets.Scripts.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.SelectionPanels
{
    public class StatsPanel : AbstractSelectionPanel<UnitController>
    {
        [SerializeField] private Text attackLabel;
        [SerializeField] private Text armorLabel;
        [SerializeField] private Text attackSpeedLabel;
        [SerializeField] private Text speedLabel;
        [SerializeField] private Text rangeLabel;

        protected override void updateData()
        {
            Unit unit = currentSelectable.Unit;
            attackLabel.text = unit.Attack.ToString();
            armorLabel.text = string.Format("{0} %", unit.Armor * 100);
            attackSpeedLabel.text = unit.AttackSpeed.ToString();
            speedLabel.text = unit.Speed.ToString();
            rangeLabel.text = unit.Range.ToString();
        }

        protected override void beforeUpdateData()
        {
        }
    }
}