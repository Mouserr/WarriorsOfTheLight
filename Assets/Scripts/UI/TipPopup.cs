using System;
using Assets.Scripts.Core.Scenarios;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class TipPopup : MonoBehaviour
    {
        [SerializeField]
        private Text label;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private float fadeInDuration;
        [SerializeField]
        private float waitTime;
        [SerializeField]
        private float fadeOutDuration;

        private IScenarioItem currentScenarioItem;

        public void Show(string tipText)
        {
            if (currentScenarioItem != null && !currentScenarioItem.IsComplete)
                currentScenarioItem.Stop();

            label.text = tipText;
            canvasGroup.alpha = 0;
            gameObject.SetActive(true);

            currentScenarioItem = new Scenario(
                new IterateItem(fadeInDuration,
                    (leftTime) =>
                    {
                        canvasGroup.alpha = 1 - leftTime/fadeInDuration;
                    }),
                new IterateItem(waitTime),
                new IterateItem(fadeOutDuration,
                    (leftTime) =>
                    {
                        canvasGroup.alpha = leftTime/fadeInDuration;
                    })
            ).Play();
        }
    }
}