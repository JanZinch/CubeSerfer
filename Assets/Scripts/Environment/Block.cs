using Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private Joint _joint;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        [HideInInspector] [SerializeField] private UnityEvent<Block> _onCollidedWithObstacle;

        public UnityEvent<Block> OnCollidedWithObstacle => _onCollidedWithObstacle;
        
        public bool IsCollided { get; private set; }

        public Rigidbody Body => _rigidbody;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<WallLevel>(out WallLevel wallLevel) && !wallLevel.IsCollided)
            {
                IsCollided = true;
                wallLevel.IsCollided = true;
                OnCollidedWithObstacle?.Invoke(this);
            }
            else if (other.gameObject.TryGetComponent<Deep>(out Deep deep))
            {
                IsCollided = true;
                OnCollidedWithObstacle?.Invoke(this);
                Destroy(gameObject);
            }
        }

        public void AttachTo(Block other)
        {
            Destroy(_joint);
            transform.position = other.GetAttachingPosition();
            _joint = AddConfiguredJoint(gameObject, other._rigidbody);
        }

        public void Detach()
        {
            Destroy(_joint);
        }

        public void Lose()
        {
            Destroy(_joint);
            transform.SetParent(null);
        }
        
        public void PutCharacter(Character character)
        {
            Destroy(character.Joint);
            
            character.transform.position = GetAttachingPosition();
            character.transform.rotation = Quaternion.identity;
            
            character.Joint = AddConfiguredJoint(character.gameObject, _rigidbody);
        }

        private Vector3 GetAttachingPosition()
        {
            return transform.position + new Vector3(0.0f, _meshRenderer.bounds.size.y, 0.0f);
        }

        private static Joint AddConfiguredJoint(GameObject gameObject, Rigidbody connectableBody)
        {
            ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();
            
            joint.autoConfigureConnectedAnchor = false;
            
            joint.anchor = new Vector3(0.0f, 0.5f, 0.0f);
            joint.connectedBody = connectableBody;
            joint.connectedAnchor = new Vector3(0.0f, 1.5f, 0.0f);

            joint.yMotion = ConfigurableJointMotion.Free;
            joint.yDrive = new JointDrive()
            {
                positionSpring = 1000.0f,
                maximumForce = float.MaxValue,
            };
            
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;

            return joint;
        }

    }
}