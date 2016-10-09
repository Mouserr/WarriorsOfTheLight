using System;
using System.Collections.Generic;
using Assets.Scripts.Core.Helpers;
using Assets.Scripts.Core.Scenarios;
using Assets.Scripts.Models;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Configs.Abilities
{
    [CreateAssetMenu(fileName = "FrostArrowConfig", menuName = "Configs/Ability/FrostArrow")]
    public class FrostArrow : ActiveAbilityConfig, ICastingRangeProvider
    {
        public FloatAbilityParameter Range;
        public FloatAbilityParameter Damage;
        public AbilityConfig FrostEffect;

        [SerializeField] 
        private float flightSpeed;
        [SerializeField]
        private ParticleSystem flyingObjectPrefab;
        [SerializeField] 
        private ParticleSystem explosionEffectPrefab;

        public override void Register()
        {
            FrostEffect.Register();
            EffectsPool.Instance.Register(flyingObjectPrefab.name, flyingObjectPrefab, 5);
            EffectsPool.Instance.Register(explosionEffectPrefab.name, explosionEffectPrefab, 5);
        }

        public override bool CouldApply(CastContext castContext, int abilityLevel, Hero caster)
        {
            if (castContext.MainTarget == null || castContext.TargetTransform == null)
                return false;

            if (caster.PlayerId == castContext.MainTarget.PlayerId)
                return false;

            if ((castContext.CasterPoint - castContext.TargetTransform.position).magnitude > Range.GetValue(abilityLevel))
                return false;

            return true;
        }

        public override IScenarioItem Apply(CastContext castContext, int abilityLevel, Hero caster)
        {
            
            ParticleSystem flyingObject =  EffectsPool.Instance.GetEffect(flyingObjectPrefab.name);
            flyingObject.transform.position = castContext.CasterPoint;
            flyingObject.gameObject.layer = castContext.TargetTransform.gameObject.layer;
            flyingObject.gameObject.SetActive(true);
           
            return new Scenario(
                new MoveToTransformItem(flyingObject.transform, castContext.TargetTransform, flightSpeed),
                new ActionItem(() =>
                {
                    if (castContext.MainTarget.HP > 0)
                    {
                        BattleManager.Instance.ApplyDamage(caster, Damage.GetValue(abilityLevel), castContext.MainTarget);
                        FrostEffect.Apply(castContext, abilityLevel, caster);
                    }

                    EffectsPool.Instance.ReleaseEffect(flyingObjectPrefab.name, flyingObject);
                    ParticleSystem explosion = EffectsPool.Instance.GetEffect(explosionEffectPrefab.name);
                    explosion.transform.position = flyingObject.transform.position;
                    explosion.gameObject.layer = castContext.TargetTransform.gameObject.layer;
                    explosion.gameObject.SetActive(true);
                    new Scenario(
                        new EffectItem(explosion), 
                        new ActionItem(() =>
                        {
                            EffectsPool.Instance.ReleaseEffect(explosionEffectPrefab.name, explosion);
                        })
                    ).Play();
                })
            ).Play();
        }

        public float GetRange(int abilityLevel)
        {
            return Range.GetValue(abilityLevel);
        }

        protected override List<IAbilityParameter> getParameters()
        {
            return new List<IAbilityParameter>
            {
                Damage,
                Range,
                Cooldown
            };
        }

    }
}