using System.Collections.Generic;
using Assets.Scripts.Selection;
using UnityEngine;

namespace Assets.Scripts.UI.SelectionPanels
{
    public class SelectionPanelsManager : MonoBehaviour
    {
        private List<AbstractSelectionPanel> panels; 

        private void Awake()
        {
            panels = new List<AbstractSelectionPanel>(GetComponentsInChildren<AbstractSelectionPanel>(true));
        }

        public void Show(ISelectable selectable)
        {
            gameObject.SetActive(true);
            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i].IsSuitableObject(selectable))
                {
                    panels[i].UpdateData(selectable);
                    panels[i].gameObject.SetActive(true);
                }
                else
                {
                    panels[i].gameObject.SetActive(false);
                }
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

    }
}