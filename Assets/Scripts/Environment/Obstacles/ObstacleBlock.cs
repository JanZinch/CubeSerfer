using UnityEngine;
using UnityEngine.Events;

namespace Environment.Obstacles
{
    public class ObstacleBlock : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onCollided;

        public UnityEvent OnCollided => _onCollided;
        
        public bool IsCollided { get; private set; }

        public void Collide()
        {
            IsCollided = true;
            OnCollided?.Invoke();
        }
        
    }
}