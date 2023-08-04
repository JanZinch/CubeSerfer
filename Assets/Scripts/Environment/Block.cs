using UnityEngine;

namespace Environment
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private Joint _joint;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        private Rigidbody ConnectedBody
        {
            get =>_joint.connectedBody;
            set => _joint.connectedBody = value;
        }
        
        public void AttachTo(Block other)
        {
            other.ConnectedBody = _rigidbody;
            transform.position = other.GetAttachingPosition();
            transform.rotation = Quaternion.identity;
        }

        private Vector3 GetAttachingPosition()
        {
            return transform.position + new Vector3(0.0f, _meshRenderer.bounds.size.y, 0.0f);
        }

    }
}