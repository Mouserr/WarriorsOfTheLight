using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core.Scenarios
{
    public class IterateItem : IScenarioItem
    {
        private readonly float processingTime;
        private readonly Action<float> iterateAction;
        private bool isPaused;
        private IEnumerator timerCoroutine;

        public bool IsComplete { get; private set; }

        public IterateItem(float processingTime, Action<float> iterateAction = null)
        {
            this.processingTime = processingTime;
            this.iterateAction = iterateAction;
        }

        public IScenarioItem Play()
        {
            isPaused = false;
            IsComplete = false;

            if (timerCoroutine == null)
            {
                timerCoroutine = getTimerCoroutine();
                ScenariosRoot.Instance.StartCoroutine(timerCoroutine);
            }

            return this;
        }

        public void Stop()
        {
            if (timerCoroutine != null && ScenariosRoot.Instance != null)
            {
                ScenariosRoot.Instance.StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }

            IsComplete = true;
        }

        public void Pause()
        {
            isPaused = true;
        }

        private IEnumerator getTimerCoroutine()
        {
            float localTimeout = processingTime;

            while (localTimeout > 0)
            {
                do
                {
                    yield return null;    
                } while (isPaused);
                
	            localTimeout -= Time.deltaTime;
                if (iterateAction != null)
	                iterateAction(localTimeout);
            }
            Stop();
        }
    }
}