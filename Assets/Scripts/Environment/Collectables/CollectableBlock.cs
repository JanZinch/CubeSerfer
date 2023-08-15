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

        public Transform TrailPivot => _trailPivot;
        
        public bool IsCollided { get; private set; }
        
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
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Track"))
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
                _usedTrack = null;
                OnUngrounded?.Invoke(this);
            }
        }
    }
}