using UnityEngine;

namespace CoreModel
{
    public class CameraTarget : MonoBehaviour
    {
        [SerializeField] private float _fixedHeight = 4.0f;
        [SerializeField] private float _fixedHorizontalPoint = 0.0f;

        private Vector3 _cachedPosition;
        
        private void Update()
        {
            Transform cachedTransform = transform;
            
            _cachedPosition.x = _fixedHorizontalPoint;
            _cachedPosition.y = _fixedHeight;
            _cachedPosition.z = cachedTransform.position.z;
            
            cachedTransform.position = _cachedPosition;
        }
    }
}