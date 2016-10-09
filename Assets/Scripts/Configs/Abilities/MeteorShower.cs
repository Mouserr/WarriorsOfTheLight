using System.Collections.Generic;
using Assets.Scripts.Core.Helpers;
using Assets.Scripts.Core.Scenarios;
using Assets.Scripts.Models;
using Assets.Scripts.Units;
using DigitalRuby.PyroParticles;
using UnityEngine;

namespace Assets.Scripts.Configs.Abilities
{
    [CreateAssetMenu(fileName = "MeteorShowerConfig", menuName = "Configs/Ability/MeteorShower")]
    public class MeteorShower : ActiveAbilityConfig, ICastingAreaProvider
    {
        public FloatAbilityParameter Radius;
        public FloatAbilityParameter Damage;

        [SerializeField]
        private MeteorSwarmScript MeteorsEffectPrefab;


        public float GetRadius(int abilityLevel)
        {
            return Radius.GetValue(abilityLevel);
        }

        public override void Register()
        {
        }

        public override bool CouldApply(CastContext castContext, int abilityLevel, Hero caster)
        {
            return true;
        }

        public override IScenarioItem Apply(CastContext castContext, int abilityLevel, Hero caster)
        {
            MeteorSwarmScript meteors = PrefabHelper.Intantiate(MeteorsEffectPrefab, MapController.Instance.gameObject);
            meteors.transform.position = castContext.TargetPoint;
            meteors.Source = castContext.TargetPoint + Vector3.up * 50;
            meteors.DestinationRadius = Radius.GetValue(abilityLevel);
            meteors.SourceRadius = Radius.GetValue(abilityLevel);
            meteors.CollisionDelegate += (MeteorSwarmScript script, GameObject meteor) =>
            {
                castContext.Targets = MapController.Instance.GetEnemiesInArea(castContext.TargetPoint,
                    Radius.GetValue(abilityLevel), caster.PlayerId);

                for (int i = 0; i < castContext.Targets.Count; i++)
                {
                    BattleManager.Instance.ApplyDamage(caster, 
                        Damage.GetValue(abilityLevel) / meteors.MeteorsPerSecondRange.Maximum, 
                        castContext.Targets[i]);
                }
            };

          
            return null;
        }


        protected override List<IAbilityParameter> getParameters()
        {
            return new List<IAbilityParameter>
            {
                Radius,
                Cooldown,
                Damage
            };
        }

    }
}