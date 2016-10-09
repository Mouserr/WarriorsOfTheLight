using System.Collections.Generic;
using Assets.Scripts.Configs;
using Assets.Scripts.Core.Scenarios;

namespace Assets.Scripts.Units
{
    public class Ability
    {
        public readonly ActiveAbilityConfig Config;
        private readonly Hero caster;
        private IScenarioItem cooldownScenario;
        private IScenarioItem castScenario;

        public float Cooldown { get; private set; }
        public int Level { get; private set; }

        public bool CouldCast
        {
            get { return Cooldown <= 0; }
        }

        public Ability(ActiveAbilityConfig config, Hero caster)
        {
            Config = config;
            this.caster = caster;
            Level = 0;
            Config.Register();
        }

        public void LevelUp()
        {
            if (caster.AbilityPoints <= 0 || Level >= Config.MaxLevel) return;

            Level++;
            caster.AbilityPoints--;
        }

        public void Apply(CastContext castContext)
        {
            if (!CouldCast || !Config.CouldApply(castContext, Level, caster)) return;
            castScenario = Config.Apply(castContext, Level, caster);
            Cooldown = Config.Cooldown.GetValue(Level);
            cooldownScenario = new IterateItem(Cooldown, (leftTime) => { Cooldown = leftTime; }).Play();
        }
    }
}