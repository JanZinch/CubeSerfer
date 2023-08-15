using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GemsCounter : MonoBehaviour
    {
        [SerializeField] private Image _icon; 
        [SerializeField] private TextMeshProUGUI _textMesh;

        private const float BounceDuration = 0.5f;
        private const float BounceScaleMultiplier = 1.25f;

        private Vector3 _sourceScale;
        private int _fakeGemsCounter = 0;
        
        public RectTransform FlyingGemsTarget => _icon.rectTransform;

        private void Awake()
        {
            _sourceScale = transform.localScale;
            _textMesh.SetText(_fakeGemsCounter.ToString());
        }

        public void AddGem()
        {
            _fakeGemsCounter++;
            _textMesh.SetText(_fakeGemsCounter.ToString());

            const float halfBounceDuration = BounceDuration / 2.0f;
            
            Sequence bounce = DOTween.Sequence();
            bounce.Append(transform.DOScale(_sourceScale * BounceScaleMultiplier, halfBounceDuration)
                .SetEase(Ease.InExpo));
            bounce.Append(transform.DOScale(_sourceScale, halfBounceDuration))
                .SetEase(Ease.OutExpo);
            bounce.SetLink(gameObject);
            
        }
    }
}