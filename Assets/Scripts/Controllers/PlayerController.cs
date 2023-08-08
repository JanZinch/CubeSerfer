using System;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using Extensions;
using PathCreation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _forwardSpeed = 2.0f;
        [SerializeField] private float _maxHorizontalSpeed = 6.0f;
        [SerializeField] private float _leftMovementConstraint = -2.0f;
        [SerializeField] private float _rightMovementConstraint = 2.0f;
        
        [SerializeField] private Transform _controlledBody;

        [SerializeField] private PathCreator _pathCreator;
        
        private float _distancePassed;
        
        private void Start()
        {
            
            
        }

        private void Update()
        {
            //MoveByZAxis();
            MoveByXAxis();
        }

        private void MoveByXAxis()
        {
            _controlledBody.localPosition = _controlledBody.localPosition.WithX(Mathf.Clamp(_controlledBody.localPosition.x + ScreenInputAxis.Instance.Delta.x * _maxHorizontalSpeed,
                _leftMovementConstraint, _rightMovementConstraint));
        }

        private void MoveByZAxis()
        {
            _distancePassed += _forwardSpeed * Time.deltaTime;

            transform.position = _pathCreator.path.GetPointAtDistance(_distancePassed);
            transform.rotation = _pathCreator.path.GetRotationAtDistance(_distancePassed);
        }

    }
}