using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers
{
    public class ScreenInputAxis : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
    {
        public static ScreenInputAxis Instance { get; private set; }

        [SerializeField] private Camera _camera;
        //[SerializeField] 
        
        private ScreenState _state = ScreenState.Free;

        private Vector3 _previousFingerPosition;

        private Vector3 _delta;

        public Vector3 Delta => _delta;
        
        public Vector2? Direction {

            get
            {
                if (_state != ScreenState.Captured)
                    return null;

                return transform.InverseTransformPoint(Input.mousePosition).normalized;
            }
        }

        private void Awake()
        {
            Instance = this;
            
            /*if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                
            }*/

            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _state = ScreenState.Captured;

            _previousFingerPosition = transform.InverseTransformPoint(Input.mousePosition).normalized;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            /*if (_state != ScreenState.Captured) 
                return;

            //Vector3 fingerPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            Vector3 currentFingerPosition = transform.InverseTransformPoint(Input.mousePosition).normalized;
            _delta = currentFingerPosition - _previousFingerPosition;
            _previousFingerPosition = currentFingerPosition;


            //Debug.Log("Pos: " + transform.InverseTransformPoint(Input.mousePosition).normalized);*/

            
            //Debug.Log("Pos: " + transform.InverseTransformPoint(Input.mousePosition));
        }

        private void Update()
        {
            if (_state != ScreenState.Captured) 
                return;

            //Vector3 fingerPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            Vector3 currentFingerPosition = transform.InverseTransformPoint(Input.mousePosition).normalized;
            _delta = currentFingerPosition - _previousFingerPosition;
            _previousFingerPosition = currentFingerPosition;


            //Debug.Log("Pos: " + transform.InverseTransformPoint(Input.mousePosition).normalized);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _delta = Vector3.zero;
            _state = ScreenState.Free;
        }
    }
}