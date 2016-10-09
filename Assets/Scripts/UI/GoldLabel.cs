using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GoldLabel : MonoBehaviour
    {
        [SerializeField] private Text label;

        private void Awake()
        {
            PlayerManager.Instance.UserPlayer.OnGoldCountChange += updateValue;
            updateValue();
        }

        private void updateValue()
        {
            label.text = PlayerManager.Instance.UserPlayer.GoldCount.ToString();
        }
    }
}