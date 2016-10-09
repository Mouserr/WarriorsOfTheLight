using System;

namespace Assets.Scripts.Units
{
    public interface IDestroyable
    {
        int PlayerId { get; }
        float HP { get; set; }
        int MaxHP { get; }
        /// <summary>
        /// Value from 0 to 1
        /// </summary>
        float Armor { get; }

        event Action OnHPChange;
    }
}