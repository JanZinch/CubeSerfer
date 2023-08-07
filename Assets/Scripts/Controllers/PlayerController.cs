using System;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using Extensions;
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

        [SerializeField] private Transform _pathPointsParent;
        
        
        
        private void Start()
        {
            Vector3[] waypoints = new Vector3[_pathPointsParent.childCount];

            int i = 0;
            
            foreach (Transform point in _pathPointsParent)
            {
                waypoints[i] = point.position;
                i++;
            }

            transform.DOLocalPath(waypoints, _forwardSpeed, PathType.Linear, PathMode.Full3D, 10, Color.red).SetEase(Ease.Linear).SetSpeedBased().OnComplete(
                () =>
                {
                    Debug.Log("Success!");
                });
            
            
        }

        private void Update()
        {
            _controlledBody.localPosition = _controlledBody.localPosition.WithX(Mathf.Clamp(_controlledBody.position.x + ScreenInputAxis.Instance.Delta.x * _maxHorizontalSpeed,
                _leftMovementConstraint, _rightMovementConstraint));
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