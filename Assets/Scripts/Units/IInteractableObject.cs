using UnityEngine;

namespace Assets.Scripts.Units
{
    public interface IInteractableObject
    {
        int PlayerId { get; }
        bool IsAlive { get; }
        bool IsVulnerable { get; }
        
        Vector3 Position { get; }
        float BoundingRadius { get; }
        float Range { get; }

        void SetTargeted(bool targeted);
    }
}