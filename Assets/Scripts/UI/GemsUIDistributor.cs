using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class GemsUIDistributor : MonoBehaviour
    {
        public static GemsUIDistributor Instance { get; private set; }
        
        [SerializeField] private Camera _camera;
        [SerializeField] private RectTransform _gemIconPrefab;
        [SerializeField] private GemsCounter _gemsCounter;

        [Space]
        [SerializeField] private float _gemsFlyingDuration = 1.0f;
        [SerializeField] private Ease _gemsFlyingEase = Ease.OutCubic;
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void LaunchGemToCounter(Vector3 sourceGemWorldPosition, Action onCounterReached = null)
        {
            Vector2 gemViewportPoint = _camera.WorldToViewportPoint(sourceGemWorldPosition);
            
            RectTransform gemIcon = Instantiate(_gemIconPrefab, transform, false);
            gemIcon.anchorMin = gemIcon.anchorMax = gemViewportPoint;
            
            gemIcon.DOMove(_gemsCounter.FlyingGemsTarget.position, _gemsFlyingDuration).SetEase(_gemsFlyingEase)
                .OnComplete(() =>
                {
                    _gemsCounter.AddGem();
                    Destroy(gemIcon.gameObject);
                    onCounterReached?.Invoke();
                })
                .SetLink(gameObject);
        }
        
    }
}
