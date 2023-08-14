using DG.Tweening;
using UI;
using UnityEngine;

namespace Environment.Collectables
{
    public class Gem : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 30.0f;

        private void Start()
        {
            transform.DOLocalRotate(new Vector3(0.0f, 360.0f, 0.0f), _rotationSpeed, RotateMode.FastBeyond360)
                .SetSpeedBased().SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetLink(gameObject);
        }

        public void Collect()
        {
            UIDistributor.Instance.LaunchGemToCounter(transform.position);
            SelfDestroy();
        }

        private void SelfDestroy()
        {
            Destroy(gameObject);
        }
    }
} 