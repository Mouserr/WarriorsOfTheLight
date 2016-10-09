using System.Collections.Generic;
using Assets.Scripts.Configs;
using Assets.Scripts.Models;
using Assets.Scripts.UI.SelectionPanels;
using Assets.Scripts.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Selection
{
    public class SelectionController : MonoBehaviour
    {
        [SerializeField]
        private Camera selectionCamera;
        [SerializeField]
        private TerrainCollider terrainCollider;
        [SerializeField]
        private int playerId;
        [SerializeField]
        private Texture2D crossTexture;
        [SerializeField]
        private Texture2D castCrossTexture;
        [SerializeField]
        private SpriteRenderer areaCastCursor;
        [SerializeField]
        private Vector2 crossOffset = new Vector2(16, 16);
        [SerializeField]
        private float minDeltaForRect = 10;
        [SerializeField]
        private Image selectionRect;
        [SerializeField]
        private SelectionPanelsManager selectionPanelsManager;

        private List<ISelectable> selection = new List<ISelectable>();
        private RaycastHit lastCursorHitInfo;

        private Vector2 startSelectionPoint;
        private List<RaycastResult> objectsHit = new List<RaycastResult>();

        private Ability castingAbility;

        private void Awake()
        {
            areaCastCursor.gameObject.SetActive(false);
            selectionRect.gameObject.SetActive(false);
            selectionPanelsManager.Hide();
        }

        public void Select(ISelectable selectable)
        {
            clearSelection();
            addToSelection(selectable);
        }

        public void SetCastingAbility(Ability ability)
        {
            castingAbility = ability;

            if (castingAbility != null)
            {

                ICastingAreaProvider areaProvider = castingAbility.Config as ICastingAreaProvider;
                if (areaProvider != null)
                {
                    areaCastCursor.transform.localScale = Vector2.one * areaProvider.GetRadius(castingAbility.Level);
                    areaCastCursor.gameObject.SetActive(true);
                }
             
                Cursor.SetCursor(castCrossTexture, crossOffset, CursorMode.Auto);
            }
            else
            {
                areaCastCursor.gameObject.SetActive(false);
                foreach (ISelectable selectable in selection)
                {
                    selectable.CancelActionPrepare();
                }
            }
        }

        private void Update()
        {
            if (isCursorOverUI())
            {
                areaCastCursor.gameObject.SetActive(false);
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                return;
            }

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                SetCastingAbility(null);
            }

            updateCursorView();

            if (updateMultipleSelectingArea()) return;

            Ray crossRay = selectionCamera.ScreenPointToRay(Input.mousePosition);

            updateCastArea(crossRay);

            if (Input.GetMouseButtonUp(0))
            {
                if (castingAbility != null)
                {
                    sendNewOrder(crossRay);
                }
                else
                {
                    clearSelection();
                    selectionRect.gameObject.SetActive(false);
                    if (shouldMultipleSelect())
                    {
                        areaSelect(crossRay);
                        return;
                    }

                    singleSelect(crossRay);
                }
               
            }
            else if (Input.GetMouseButtonUp(1))
            {
                if (castingAbility != null)
                {
                    SetCastingAbility(null);
                }
                else
                {
                    sendNewOrder(crossRay);
                }
            }
        }

        private void sendNewOrder(Ray crossRay)
        {
            if (Physics.Raycast(crossRay, out lastCursorHitInfo, 10000))
            {
                IInteractableObject interactable = lastCursorHitInfo.collider.gameObject.GetComponent<IInteractableObject>();
                foreach (var selectable in selection)
                {
                    if (interactable != null && (castingAbility == null || castingAbility.Config.TargetType == TargetType.Single))
                    {
                        selectable.InteractWithObject(interactable, true);
                    }
                    else
                    {
                        if (castingAbility != null && castingAbility.Config.TargetType == TargetType.Single)
                            return;

                        selectable.InteractWithMapPoint(lastCursorHitInfo.point, true);
                    }
                }
                SetCastingAbility(null);
            }
        }

        private void singleSelect(Ray crossRay)
        {
            if (Physics.Raycast(crossRay, out lastCursorHitInfo, 10000, selectionCamera.cullingMask,
                QueryTriggerInteraction.Ignore))
            {
                ISelectable selectable = lastCursorHitInfo.collider.gameObject.GetComponent<ISelectable>();
                if (selectable != null)
                {
                    Cursor.SetCursor(crossTexture, crossOffset, CursorMode.Auto);

                    addToSelection(selectable);
                }
                else
                {
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                }
            }
        }

        private void areaSelect(Ray crossRay)
        {
            RaycastHit startHitInfo;
            terrainCollider.Raycast(crossRay, out lastCursorHitInfo, 10000);
            terrainCollider.Raycast(selectionCamera.ScreenPointToRay(startSelectionPoint), out startHitInfo, 10000);
            Vector3 delta = lastCursorHitInfo.point - startHitInfo.point;
            List<ISelectable> selectables =
                MapController.Instance.GetAllUnitsInBounds(
                    new Bounds(startHitInfo.point + delta/2, new Vector3(Mathf.Abs(delta.x), 100, Mathf.Abs(delta.z))), playerId);
            for (int i = 0; i < selectables.Count; i++)
            {
                addToSelection(selectables[i]);
            }
        }

        private bool shouldMultipleSelect()
        {
            return selectionRect.rectTransform.sizeDelta.magnitude > minDeltaForRect;
        }

        private void updateCastArea(Ray crossRay)
        {
            if (castingAbility != null && (castingAbility.Config is ICastingAreaProvider))
            {
                areaCastCursor.gameObject.SetActive(true);
                RaycastHit currentTerranCast;
                terrainCollider.Raycast(crossRay, out currentTerranCast, 10000);
                areaCastCursor.transform.position = currentTerranCast.point + Vector3.up*2;
            }
        }

        private bool updateMultipleSelectingArea()
        {
            if (castingAbility != null) return false;

            if (Input.GetMouseButtonDown(0))
            {
                startSelectionPoint = Input.mousePosition;
                selectionRect.rectTransform.position = startSelectionPoint;
                return true;
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 currentPoint = Input.mousePosition;

                Vector2 delta = currentPoint - startSelectionPoint;

                if (delta.x > 0)
                    if (delta.y > 0)
                    {
                        selectionRect.rectTransform.pivot = Vector2.zero;
                    }
                    else
                    {
                        selectionRect.rectTransform.pivot = Vector2.up;
                    }
                else if (delta.y > 0)
                {
                    selectionRect.rectTransform.pivot = Vector2.right;
                }
                else
                {
                    selectionRect.rectTransform.pivot = Vector2.one;
                }

                selectionRect.rectTransform.sizeDelta = new Vector2(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
                selectionRect.gameObject.SetActive(true);
                return true;
            }
            return false;
        }

        private void updateCursorView()
        {
            if (selection.Count > 0)
            {
                if (castingAbility != null)
                {
                    Cursor.SetCursor(castCrossTexture, crossOffset, CursorMode.Auto);
                }
                else
                {
                    areaCastCursor.gameObject.SetActive(false);
                    Cursor.SetCursor(crossTexture, crossOffset, CursorMode.Auto);
                }
            }
        }

        private bool isCursorOverUI()
        {
            PointerEventData cursor = new PointerEventData(EventSystem.current);
            cursor.position = Input.mousePosition;
            EventSystem.current.RaycastAll(cursor, objectsHit);
            var b = objectsHit.Count > 0;
            return b;
        }

        private void addToSelection(ISelectable selectable)
        {
            selectable.SetSelected(true);
            selection.Add(selectable);
            if (selection.Count == 1)
            {
                selectionPanelsManager.Show(selection[0]);
            }
            else
            {
                selectionPanelsManager.Hide();
            }
        }

        private void clearSelection()
        {
            for (int i = 0; i < selection.Count; i++)
            {
                selection[i].SetSelected(false);
            }
            selectionPanelsManager.Hide();
            selection.Clear();
            SetCastingAbility(null);
        }
    }
}