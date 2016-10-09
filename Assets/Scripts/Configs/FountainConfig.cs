using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "FountainConfig", menuName = "Configs/Fountain")]
    public class FountainConfig : ScriptableObject
    {
        public float MinionsHealingSpeed;
        public float HeroHealingSpeed;
    }
}