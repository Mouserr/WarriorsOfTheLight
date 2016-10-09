using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ButtonShortcut : MonoBehaviour
    {
        [SerializeField]
        private KeyCode shortcut;
        [SerializeField]
        private Text keyLabel;

        private void Start()
        {
            keyLabel.text = shortcut.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyUp(shortcut))
            {
                var pointer = new PointerEventData(EventSystem.current); // pointer event for Execute
                ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.submitHandler);
            }
        }
    }
}