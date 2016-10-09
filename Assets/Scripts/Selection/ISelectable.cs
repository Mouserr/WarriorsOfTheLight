using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Selection
{
    public interface ISelectable
    {
        string Name { get; }

        IDestroyable Destroyable { get; }

        void InteractWithMapPoint(Vector3 point, bool force = false);

        void InteractWithObject(IInteractableObject interactable, bool force = false);

        void CancelActionPrepare();

        void SetSelected(bool selected);
    }
}