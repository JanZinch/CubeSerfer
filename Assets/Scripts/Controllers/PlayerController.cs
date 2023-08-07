using System;
using Extensions;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _forwardSpeed = 2.0f;
        [SerializeField] private float _maxHorizontalSpeed = 6.0f;
        [SerializeField] private float _leftMovementConstraint = -2.0f;
        [SerializeField] private float _rightMovementConstraint = 2.0f;
        
        [SerializeField] private Transform _controlledBody;
        
        private void Update()
        {
            MoveByZAxis();
            MoveByXAxis();
        }

        private void MoveByXAxis()
        {
            _controlledBody.position = _controlledBody.position.WithX(Mathf.Clamp(_controlledBody.position.x + ScreenInputAxis.Instance.Delta.x * _maxHorizontalSpeed,
                _leftMovementConstraint, _rightMovementConstraint));
        }

        private void MoveByZAxis()
        {
            transform.Translate(Vector3.forward * (_forwardSpeed * Time.deltaTime));
        }

    }
}