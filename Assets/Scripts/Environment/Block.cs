using System;
using Controllers;
using Environment.Obstacles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Environment
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        [HideInInspector] [SerializeField] private UnityEvent<Block> _onCollidedWithObstacle;
        [HideInInspector] [SerializeField] private UnityEvent<Block> _onGrounded;
        [HideInInspector] [SerializeField] private UnityEvent<Block> _onUngrounded;
        
        public UnityEvent<Block> OnCollidedWithObstacle => _onCollidedWithObstacle;
        public UnityEvent<Block> OnGrounded => _onGrounded;
        public UnityEvent<Block> OnUngrounded => _onUngrounded;
        
        public bool IsCollided { get; private set; }
        
        private Joint _joint;
        private GameObject _usedTrack;
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<ObstacleBlock>(out ObstacleBlock obstacleBlock) 
                && !obstacleBlock.IsCollided)
            {
                IsCollided = true;
                obstacleBlock.Collide();
                OnCollidedWithObstacle?.Invoke(this);
            }
            else if (other.gameObject.TryGetComponent<Deep>(out Deep deep))
            {
                IsCollided = true;
                OnCollidedWithObstacle?.Invoke(this);
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("Track"))
            {
                bool alreadyGrounded = _usedTrack != null;
                _usedTrack = other.gameObject;
                
                if (!alreadyGrounded)
                {
                    OnGrounded?.Invoke(this);
                }
            }
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Track") && other.gameObject == _usedTrack)
            {
                Debug.Log("Ungrounded");
                
                _usedTrack = null;
                OnUngrounded?.Invoke(this);
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
            
            character.OnAttachInject(this, AddConfiguredJoint(character.gameObject, _rigidbody));
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
                positionDamper = 100.0f,
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