using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GemsUIDistributor : MonoBehaviour
    {
        public static GemsUIDistributor Instance { get; private set; }
        
        [SerializeField] private RectTransform _gemIconPrefab;
        [SerializeField] private Camera _camera;
        [SerializeField] private TextMeshProUGUI _gemsCounter;
        
        private int _fakeGemsCounter;

        private void Awake()
        {
            Instance = this;
        }

        private void AddGem()
        {
            _fakeGemsCounter++;
            _gemsCounter.SetText(_fakeGemsCounter.ToString());
        }

        public void LaunchGemToCounter(Vector3 sourceGemWorldPosition)
        {
            Vector2 gemViewportPoint = _camera.WorldToViewportPoint(sourceGemWorldPosition);
            
            RectTransform gemIcon = Instantiate(_gemIconPrefab, transform, false);
            gemIcon.anchorMin = gemIcon.anchorMax = gemViewportPoint;
        }



    }
}
