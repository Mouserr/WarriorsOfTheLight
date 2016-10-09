using UnityEngine;

namespace Assets.Scripts.Cameras
{
    public class CameraModeController : MonoBehaviour
    {
        [SerializeField]
        private FreeCameraController freeCamera;
        [SerializeField]
        private HeroTrackingCameraController heroCamera;
        [SerializeField]
        private Transform minTransform;
        [SerializeField]
        private Transform maxTransform;

        private void Awake()
        {
            freeCamera.Min = heroCamera.Min = minTransform.position;
            freeCamera.Max = heroCamera.Max = maxTransform.position;
        }

        public void ToggleMode()
        {
            freeCamera.IsActive = !freeCamera.IsActive;
            heroCamera.IsActive = !heroCamera.IsActive;
        }
    }
}