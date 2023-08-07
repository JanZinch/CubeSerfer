﻿using System;
using Configs;
using Controllers;
using DG.Tweening;
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

        private Rigidbody ConnectedBody
        {
            get =>_joint.connectedBody;
            set => _joint.connectedBody = value;
        }

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
      
            _joint = gameObject.AddComponent<ConfigurableJoint>();
            //PresetType TYPE = _jointPreset.GetPresetType()
            _jointPreset.ApplyTo(_joint); 
            other.ConnectedBody = _rigidbody;
            
        }

        public void Lose()
        {
            //SetMovable(false);
            Destroy(_joint);
        }
        
        public void PutCharacter(Character character)
        {
            character.transform.position = GetAttachingPosition();
            character.transform.rotation = Quaternion.identity;
            
            _joint.connectedBody = character.Rigidbody;
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