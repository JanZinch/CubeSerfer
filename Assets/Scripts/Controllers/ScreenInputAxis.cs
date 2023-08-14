using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Controllers
{
    public class ScreenInputAxis : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public static ScreenInputAxis Instance { get; private set; }
        
        [SerializeField] private UnityEvent _onFingerDown;
        [SerializeField] private UnityEvent _onFingerUp;
        
        private ScreenState _state = ScreenState.Free;
        private Vector3 _previousFingerPosition;
        private Vector3 _delta;

        public UnityEvent OnFingerDown => _onFingerDown;
        public UnityEvent OnFingerUp => _onFingerUp;
        public Vector3 Delta => _delta;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameStateMachine.Instance.OnWinning += () => { enabled = false; };
            GameStateMachine.Instance.OnLoss += () => { enabled = false; };
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _state = ScreenState.Captured;

            _previousFingerPosition = transform.InverseTransformPoint(Input.mousePosition).normalized;
            
            OnFingerDown?.Invoke();
        }
        
        private void Update()
        {
            if (_state != ScreenState.Captured) 
                return;
            
            Vector3 currentFingerPosition = transform.InverseTransformPoint(Input.mousePosition).normalized;
            _delta = currentFingerPosition - _previousFingerPosition;
            _previousFingerPosition = currentFingerPosition;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _delta = Vector3.zero;
            _state = ScreenState.Free;
            
            OnFingerUp?.Invoke();
        }
    }
}