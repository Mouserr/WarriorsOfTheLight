using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private Image foreground;
        [SerializeField]
        private Image backgound;

        private Vector2 backgroundSize;

        private float currentValue;

        private void Awake()
        {
            backgroundSize = backgound.rectTransform.sizeDelta;
            updateView();
        }

        public void SetValue(float value)
        {
            currentValue = value;
            updateView();
        }

        private void updateView()
        {
            foreground.rectTransform.sizeDelta = new Vector2(backgroundSize.x * currentValue, foreground.rectTransform.sizeDelta.y);
        }
    }
}