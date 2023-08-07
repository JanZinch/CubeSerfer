using System;
using Configs;
using Controllers;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Environment
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private Preset _jointPreset;
        [SerializeField] private Joint _joint;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private bool _isMovable;
        //[SerializeField] private MoverConfig _moverConfig;
        
        [HideInInspector] [SerializeField] private UnityEvent<Block> _onCollidedWithObstacle;

        public UnityEvent<Block> OnCollidedWithObstacle => _onCollidedWithObstacle;

        //private Mover _mover = null;
        
        //public bool IsMovable => _mover != null;

        public bool IsCollided { get; private set; }

        public Rigidbody Body => _rigidbody;

        /*public void SetMovable(bool movable)
        {
            if (IsMovable == movable)
                return;
            
            if (IsMovable)
            {
                Destroy(_mover);
            }
            else
            {
                _mover = gameObject.AddComponent<Mover>().Launch(_moverConfig, _rigidbody);
            }
        }*/

        /*[EasyButtons.Button]
        public void Launch()
        {
            SetMovable(true);
        }

        private void Awake()
        {
            SetMovable(_isMovable);
        }*/

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
                IsCollided = true;
                OnCollidedWithObstacle?.Invoke(this);
            }
        }

        public void AttachTo(Block other)
        {
            
            Destroy(_joint);
       
            transform.position = other.GetAttachingPosition();

            //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = transform.position;
            
            _joint = gameObject.AddComponent<ConfigurableJoint>();
            //PresetType TYPE = _jointPreset.GetPresetType()
            _jointPreset.ApplyTo(_joint);
            _joint.connectedBody = other._rigidbody;
            _joint.connectedAnchor = Vector3.up * 1.5f;
            //other.ConnectedBody = _rigidbody;
            
        }

        public void Detach()
        {
            Destroy(_joint);
        }

        public void Lose()
        {
            //SetMovable(false);
            Destroy(_joint);
            transform.SetParent(null);
        }
        
        public void PutCharacter(Character character)
        {
            Destroy(character.Joint);
            
            character.transform.position = GetAttachingPosition();
            character.transform.rotation = Quaternion.identity;

            character.Joint = character.AddComponent<ConfigurableJoint>();
            _jointPreset.ApplyTo(character.Joint);
            character.Joint.connectedBody = _rigidbody;
            character.Joint.connectedAnchor = Vector3.up * 1.5f;
        }

        private Vector3 GetAttachingPosition()
        {
            return transform.position + new Vector3(0.0f, _meshRenderer.bounds.size.y, 0.0f);
        }

        private static void ConfigureJoint(ConfigurableJoint joint)
        {
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = new Vector3(0.0f, 0.5f, 0.0f);
            //joint.connectedAnchor

        }

    }
}