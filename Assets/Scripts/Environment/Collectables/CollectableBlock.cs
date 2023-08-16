using System;
using Environment.Obstacles;
using UnityEngine;
using UnityEngine.Events;

namespace Environment.Collectables
{
    public class CollectableBlock : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform _trailPivot;
        
        [HideInInspector] [SerializeField] private UnityEvent<CollectableBlock> _onCollidedWithObstacle;
        [HideInInspector] [SerializeField] private UnityEvent<CollectableBlock> _onGrounded;
        [HideInInspector] [SerializeField] private UnityEvent<CollectableBlock> _onUngrounded;
        
        public UnityEvent<CollectableBlock> OnCollidedWithObstacle => _onCollidedWithObstacle;
        public UnityEvent<CollectableBlock> OnGrounded => _onGrounded;
        public UnityEvent<CollectableBlock> OnUngrounded => _onUngrounded;

        public bool IsCollided { get; private set; }
        
        public Transform TrailPivot => _trailPivot;
        
        private const string TrackObjectTag = "Track";
        private GameObject _usedTrack;
        
        public void AttachTo(CollectableBlock other)
        {
            other.PutObject(transform);
        }

        public void PutObject(Transform objectTransform)
        {
            objectTransform.position = GetAttachingPosition();
            objectTransform.rotation = Quaternion.identity;
        }
        
        public void Lose()
        {
            transform.SetParent(null);
        }
        
        public static bool AreIntersects(CollectableBlock one, CollectableBlock two)
        {
            return Mathf.Abs(one.transform.position.y - two.transform.position.y) < 
                   one._meshRenderer.bounds.size.y / 2.0f;
        }
        
        private Vector3 GetAttachingPosition()
        {
            return transform.position + new Vector3(0.0f, _meshRenderer.bounds.size.y, 0.0f);
        }
        
        private void OnTriggerEnter(Collider other)
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
        }
        
        private void OnCollisionStay(Collision other)
        {
            bool alreadyGrounded = _usedTrack != null;

            GameObject otherGameObject = other.gameObject;
            
            if (otherGameObject != _usedTrack && otherGameObject.CompareTag(TrackObjectTag))
            {
                _usedTrack = otherGameObject;

                if (!alreadyGrounded)
                {
                    OnGrounded?.Invoke(this);
                }
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject == _usedTrack)
            {
                _usedTrack = null;
                OnUngrounded?.Invoke(this);
            }
        }
    }
}