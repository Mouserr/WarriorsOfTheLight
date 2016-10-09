using System;
using Assets.Scripts.Core.Scenarios;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GameOverPopup : MonoBehaviour
    {
        [SerializeField]
        private Text resultLabel;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private float fadeInDuration;

        private Action onRestart;

        public void Show(GameResult result, Action restartAction)
        {
            onRestart = restartAction;
            resultLabel.text = result == GameResult.Win ? "Victory!" : "Defeat... =(";
            canvasGroup.alpha = 0;
            gameObject.SetActive(true);

            new IterateItem(fadeInDuration,
                (leftTime) =>
                {
                    canvasGroup.alpha = 1 - leftTime/fadeInDuration;
                }).Play();
        }

        public void Restart()
        {
            if (onRestart != null)
                onRestart();
        }
    }
}