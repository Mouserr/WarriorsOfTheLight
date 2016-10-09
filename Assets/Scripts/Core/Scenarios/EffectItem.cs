using System;
using UnityEngine;

namespace Assets.Scripts.Core.Scenarios
{
    public class EffectItem : IScenarioItem
    {
        private readonly ParticleSystem particleSystem;
        private IScenarioItem timerItem;

        public EffectItem(ParticleSystem particleSystem, float duration = -1)
        {
            this.particleSystem = particleSystem;
            if (duration > 0)
            {
                timerItem = new IterateItem(duration);
            }
        }

        public IScenarioItem Play()
        {
            particleSystem.Play(true);
            if (timerItem != null)
                timerItem.Play();
            return this;
        }

        public void Stop()
        {
            particleSystem.Stop(true);
            if (timerItem != null)
                timerItem.Stop();
        }

        public void Pause()
        {
            particleSystem.Pause(true);
            if (timerItem != null)
                timerItem.Pause();
        }

        public bool IsComplete
        {
            get { return timerItem == null ? particleSystem.IsAlive(true) : timerItem.IsComplete; }
        }
    }
}