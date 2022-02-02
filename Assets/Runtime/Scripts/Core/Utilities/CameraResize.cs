using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Woska.Core
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CameraResize : MonoBehaviour
    {
        [SerializeField] private bool autoAdjust = true;
        // Set this to the in-world distance between the left & right edges of your scene.
        [SerializeField] float _sceneWidth = 10;

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }
        void Start() 
        {
            AdjustCameraSize();
        }

        private void Update()
        {
            AdjustCameraSize();
            // choose the margin randomly

        }

        private void AdjustCameraSize()
        {
            if(!autoAdjust) return;
            float unitsPerPixel = _sceneWidth / Screen.width;

            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

            _camera.orthographicSize = desiredHalfHeight;
        }
    }
}