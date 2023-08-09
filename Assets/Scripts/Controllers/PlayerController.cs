using Dreamteck.Splines;
using Extensions;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _forwardSpeed = 5.0f;
        [SerializeField] private float _maxHorizontalSpeed = 6.0f;
        [SerializeField] private float _leftMovementConstraint = -2.0f;
        [SerializeField] private float _rightMovementConstraint = 2.0f;
        
        [SerializeField] private SplineFollower _splineFollower;
        [SerializeField] private Transform _controlledBody;
        
        private void Start()
        {
            _splineFollower.followSpeed = 0.0f;
            ScreenInputAxis.Instance.OnFingerDown.AddListener(StartMoving);
        }

        private void StartMoving()
        {
            _splineFollower.followSpeed = _forwardSpeed;
            ScreenInputAxis.Instance.OnFingerDown.RemoveListener(StartMoving);
        }

        private void Update()
        {
            MoveByXAxis();
        }

        private void MoveByXAxis()
        {
            Vector3 cachedLocalPosition = _controlledBody.localPosition;
            float x = Mathf.Clamp(cachedLocalPosition.x + ScreenInputAxis.Instance.Delta.x * _maxHorizontalSpeed,
                _leftMovementConstraint, _rightMovementConstraint);
            
            _controlledBody.localPosition = cachedLocalPosition.WithX(x);
        }
    }
}