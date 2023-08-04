using System;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace Controllers
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float _forwardSpeed;
        [SerializeField] private float _maxHorizontalSpeed;
        [SerializeField] private Rigidbody _rigidbody;
        
        [SerializeField] private float _leftMovementConstraint = -2.0f;
        [SerializeField] private float _rightMovementConstraint = 2.0f;

        private MoverDirection _direction = MoverDirection.Forward;
        
        [EasyButtons.Button]
        private void RotateMe()
        {
            _rigidbody.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }

        [EasyButtons.Button]
        private void TurnLeft()
        {
            /*DOTween.To(() => _rigidbody.velocity, (newVelocity) => _rigidbody.velocity = newVelocity,
                Vector3.zero.WithX(-_forwardSpeed), 1.0f);*/

            _direction = MoverDirection.Left;
            
            //_rigidbody.velocity = Vector3.zero.WithX(-_forwardSpeed);

        }

        private void Update()
        {
            //transform.Translate(_screenInputAxis.Delta.x * _maxHorizontalSpeed, 0.0f, 0.0f);

            _rigidbody.MovePosition(_rigidbody.position.WithX(Mathf.Clamp(_rigidbody.position.x + ScreenInputAxis.Instance.Delta.x * _maxHorizontalSpeed,
                _leftMovementConstraint, _rightMovementConstraint)));
            
            /*_rigidbody.position += Vector3.right * Mathf.Clamp(_screenInputAxis.Delta.x * _maxHorizontalSpeed,
                _leftMovementConstraint, _rightMovementConstraint);*/
        }

        private void FixedUpdate()
        {
            //_rigidbody.velocity = _rigidbody.velocity.WithX(_screenInputAxis.Direction.x * _maxHorizontalSpeed);

            switch (_direction)
            {
                case MoverDirection.Forward:
                    _rigidbody.velocity = _rigidbody.velocity.WithX(0.0f).WithZ(_forwardSpeed);
                    break;
                
                case MoverDirection.Left:
                    _rigidbody.velocity = _rigidbody.velocity.WithX(-_forwardSpeed).WithZ(0.0f);
                    break;
                
                case MoverDirection.Right:
                    _rigidbody.velocity = _rigidbody.velocity.WithX(_forwardSpeed).WithZ(0.0f);
                    break;
            }
            
            //_rigidbody.velocity = _rigidbody.velocity.WithZ(_forwardSpeed);
            
            /*var locVel = transform.InverseTransformDirection(_rigidbody.velocity).WithZ(_forwardSpeed);
            _rigidbody.velocity = transform.TransformDirection(locVel);*/
            
            
            
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