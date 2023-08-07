﻿using System;
using Environment;
using UnityEngine;

namespace Controllers
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private Joint _joint;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private BlocksStack _blocksStack;

        public Joint Joint;
        public Rigidbody Rigidbody => _rigidbody;

        private void Awake()
        {
            Joint = _joint;
        }

        private void OnEnable()
        {
            _blocksStack.OnBlockAdded.AddListener(MoveUp);
        }

        private void MoveUp(Block upperBlock)
        {
            upperBlock.PutCharacter(this);
        }

        private void OnDisable()
        {
            _blocksStack.OnBlockAdded.RemoveListener(MoveUp);
        }
    }
}