using System;
using Extensions;
using UnityEngine;

namespace Controllers
{
    public class CameraTarget : MonoBehaviour
    {
        [SerializeField] private float _fixedHeight = 4.0f;
        [SerializeField] private float _fixedHorizontalPoint = 0.0f;
        private void Update()
        {
            transform.position = new Vector3(_fixedHorizontalPoint, _fixedHeight, transform.position.z);
        }
    }
}