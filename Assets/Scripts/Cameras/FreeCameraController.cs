using UnityEngine;

namespace Assets.Scripts.Cameras
{
    public class FreeCameraController : AbstractCameraController
    {
        [SerializeField]
        private Vector2 motionBorder;
        [SerializeField]
        private Vector2 motionSpeed;

        private void Update()
        {
            if (!IsActive) return;

            Vector2 mousePosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);
            Vector3 delta = Vector3.zero;
            if (mousePosition.x < motionBorder.x || Input.GetKey(KeyCode.A))
            {
                delta.z = -motionSpeed.x;
            }
            if (mousePosition.x > 1 - motionBorder.x || Input.GetKey(KeyCode.D))
            {
                delta.z = motionSpeed.x;
            }
            if (mousePosition.y < motionBorder.y || Input.GetKey(KeyCode.S))
            {
                delta.x = motionSpeed.y;
            }
            if (mousePosition.y > 1 - motionBorder.y || Input.GetKey(KeyCode.W))
            {
                delta.x = -motionSpeed.y;
            }

            setPosition(mainCamera.transform.position + delta);
        }
    }
}