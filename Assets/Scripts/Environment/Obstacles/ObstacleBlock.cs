using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Environment.Obstacles
{
    public class ObstacleBlock : MonoBehaviour
    {
        [SerializeField] private Collider _auxiliaryCollider;
        [HideInInspector] [SerializeField] private UnityEvent _onCollided;
        
        public UnityEvent OnCollided => _onCollided;
        
        public bool IsCollided { get; private set; }

        public void Collide()
        {
            IsCollided = true;
            _auxiliaryCollider.enabled = false;
            OnCollided?.Invoke();
        }
        
    }
}