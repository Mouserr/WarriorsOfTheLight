using System;
using System.Collections;

namespace Assets.Scripts.Core.Scenarios
{
    public class Scenario : IScenarioItem
    {
        private readonly Action onStopAction;
        private readonly IScenarioItem[] scenarioItems;
        private int currentIndex;
        private IEnumerator currentCoroutine;
        private bool isStarted;
        private bool isPaused;

        public Scenario(params IScenarioItem[] scenarioItems)
            : this(null, scenarioItems)
        {
        }
        
        public Scenario(Action onStopAction, params IScenarioItem[] scenarioItems)
        {
            this.onStopAction = onStopAction;
            this.scenarioItems = scenarioItems;
        }

        public IScenarioItem Play()
        {
            isPaused = false;
            if (isStarted)
            {
                scenarioItems[currentIndex].Play();
                return this;
            }

            isStarted = true;
            currentCoroutine = getSequenceCoroutine();
            ScenariosRoot.Instance.StartCoroutine(currentCoroutine);

            return this;
        }

        public void Stop()
        {
            if (!isStarted) return;

            if (currentIndex < scenarioItems.Length)
                scenarioItems[currentIndex].Stop();

            if (currentCoroutine != null && ScenariosRoot.Instance != null)
            {
                ScenariosRoot.Instance.StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }

            if (onStopAction != null)
            {
                onStopAction();
            }

            isStarted = false;
        }

        public void Pause()
        {
            if (!isStarted)
                return;

            isPaused = true;
            scenarioItems[currentIndex].Pause();
        }

        public bool IsComplete
        {
            get { return !isStarted; }
        }

        private IEnumerator getSequenceCoroutine()
        {
            for (currentIndex = 0; currentIndex < scenarioItems.Length; currentIndex++)
            {
                if (scenarioItems[currentIndex] != null)
                {
                    scenarioItems[currentIndex].Play();
                    while (!scenarioItems[currentIndex].IsComplete)
                    {
                        yield return null;
                    }
                }

                while (isPaused)
                    yield return null;

                if (IsComplete) break;
            }

            Stop();
        }
    }
}