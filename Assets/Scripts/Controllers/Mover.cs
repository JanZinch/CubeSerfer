using Extensions;
using UnityEngine;

namespace Controllers
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float _forwardSpeed;
        [SerializeField] private Rigidbody _rigidbody;

        private void FixedUpdate()
        {
            _rigidbody.velocity = _rigidbody.velocity.WithZ(_forwardSpeed);
        }
    }
}