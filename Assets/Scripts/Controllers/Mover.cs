using System;
using Extensions;
using UnityEngine;

namespace Controllers
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float _forwardSpeed;
        [SerializeField] private float _maxHorizontalSpeed;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ScreenInputAxis _screenInputAxis;

        [SerializeField] private float _leftMovementConstraint = -2.0f;
        [SerializeField] private float _rightMovementConstraint = 2.0f;

        private void Update()
        {
            //transform.Translate(_screenInputAxis.Delta.x * _maxHorizontalSpeed, 0.0f, 0.0f);

            _rigidbody.position += Vector3.right * (_screenInputAxis.Delta.x * _maxHorizontalSpeed);
        }

        private void FixedUpdate()
        {
            //_rigidbody.velocity = _rigidbody.velocity.WithX(_screenInputAxis.Direction.x * _maxHorizontalSpeed);
            _rigidbody.velocity = _rigidbody.velocity.WithZ(_forwardSpeed);

            
            
            /*if (_screenInputAxis.Direction != null)
            {
                Vector2 direction = _screenInputAxis.Direction.Value;
                
                
                if (direction.x > 0.0f)
                {
                    transform.position = transform.position.WithX(Mathf.Lerp(0.0f, _rightMovementConstraint,
                        direction.x));
                }
                else
                {
                    transform.position = transform.position.WithX(Mathf.Lerp(0.0f, _leftMovementConstraint,
                        -1.0f * direction.x));
                }
                
            }*/

            




            
        }
    }
}