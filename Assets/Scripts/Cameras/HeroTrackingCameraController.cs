using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Cameras
{
    public class HeroTrackingCameraController : AbstractCameraController
    {
        [SerializeField]
        private HeroController heroController;

        private Vector3 offset;
        private Vector3 lastHeroPosition;

        private void Start()
        {
            IsActive = true;
            offset = Vector3.Project(mainCamera.transform.position - heroController.transform.position, mainCamera.ViewportPointToRay(Vector3.one/2f).direction);
        }

        private void Update()
        {
            if (!IsActive) return;

            setPosition(heroController.transform.position + offset);
        }
    }
}