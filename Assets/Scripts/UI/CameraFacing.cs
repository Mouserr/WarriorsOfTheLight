using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class CameraFacing : MonoBehaviour
    {
        Camera mGameCam;
        
        void OnEnable()
        {
            mGameCam = Camera.main;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + mGameCam.transform.rotation * Vector3.forward,
                mGameCam.transform.up);
        }
    }
}