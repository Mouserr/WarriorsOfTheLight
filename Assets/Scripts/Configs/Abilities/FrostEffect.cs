using System.Collections.Generic;
using Assets.Scripts.Core.Helpers;
using Assets.Scripts.Core.Scenarios;
using Assets.Scripts.Models;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Configs.Abilities
{
    [CreateAssetMenu(fileName = "FrostEffectConfig", menuName = "Configs/Ability/FrostEffect")]
    public class FrostEffect : AbilityConfig
    {
        public FloatAbilityParameter MovementSlow;
        public FloatAbilityParameter AttackSlow;
        public FloatAbilityParameter Duration;

        [SerializeField]
        private ParticleSystem frostEffectPrefab;
        [SerializeField]
        private Vector3 effectOffset;


        public override void Register()
        {
            EffectsPool.Instance.Register(name, frostEffectPrefab, 5);
        }

        public override IScenarioItem Apply(CastContext castContext, int abilityLevel, Hero caster)
        {
            if (castContext.MainTarget == null || castContext.MainTarget.HP <= 0) return null;

            ParticleSystem effect = EffectsPool.Instance.GetEffect(name);
            effect.gameObject.AppendTo(castContext.TargetTransform.gameObject);
            effect.transform.localPosition = Vector3.zero;
            effect.transform.position += effectOffset;
            effect.gameObject.SetActive(true);
            float duration = Duration.GetValue(abilityLevel);
            castContext.MainTarget.AddEffect(
                new StatModifier
                {
                    Speed = -castContext.MainTarget.Speed * MovementSlow.GetValue(abilityLevel),
                    AttackSpeed = -castContext.MainTarget.AttackSpeed * AttackSlow.GetValue(abilityLevel)
                }, 
                duration,
                new Scenario(
                    () => { EffectsPool.Instance.ReleaseEffect(name, effect); },
                    new EffectItem(effect, duration)
                )
            );

            return null;
        }

        protected override List<IAbilityParameter> getParameters()
        {
            return new List<IAbilityParameter>
            {
                AttackSlow,
                Duration,
                MovementSlow
            };
        }
    }
}