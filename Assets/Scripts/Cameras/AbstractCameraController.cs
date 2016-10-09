using System;
using UnityEngine;

namespace Assets.Scripts.Cameras
{
    public class AbstractCameraController : MonoBehaviour
    {
        [SerializeField] 
        protected Camera mainCamera;

        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }
        public bool IsActive { get; set; }

        protected void setPosition(Vector3 newCameraPosition)
        {
            newCameraPosition.x = Math.Min(newCameraPosition.x, Max.x);
            newCameraPosition.z = Math.Min(newCameraPosition.z, Max.z);
            newCameraPosition.x = Math.Max(newCameraPosition.x, Min.x);
            newCameraPosition.z = Math.Max(newCameraPosition.z, Min.z);

            mainCamera.transform.position = newCameraPosition;
        }
    }
}