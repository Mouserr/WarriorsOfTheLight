using Assets.Scripts.Selection;
using UnityEngine;

namespace Assets.Scripts.UI.SelectionPanels
{
    public abstract class AbstractSelectionPanel : MonoBehaviour
    {
        public abstract bool IsSuitableObject(ISelectable selectable);
        public abstract void UpdateData(ISelectable selectable);
    }

    public abstract class AbstractSelectionPanel<T> : AbstractSelectionPanel where T : ISelectable
    {
        protected T currentSelectable { get; private set; }

        public override bool IsSuitableObject(ISelectable selectable)
        {
            return selectable is T;
        }

        public override void UpdateData(ISelectable selectable)
        {
            beforeUpdateData();
            currentSelectable = (T)selectable;
            updateData();
        }

        protected abstract void updateData();
        protected abstract void beforeUpdateData();
    }
}