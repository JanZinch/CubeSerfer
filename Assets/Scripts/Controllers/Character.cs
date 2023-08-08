using System;
using Environment;
using UnityEngine;

namespace Controllers
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private Joint _joint;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private BlocksStack _blocksStack;

        private Block _baseBlock;
        public Joint Joint => _joint;
        public Rigidbody Rigidbody => _rigidbody;

        private void OnEnable()
        {
            _blocksStack.OnBlockAdded.AddListener(OnBlockAdded);
            //_blocksStack.OnBlockRemoved.AddListener(OnBlockRemoved);
        }
        
        private void Start()
        {
            _baseBlock = _blocksStack.Top;
        }

        private void OnBlockAdded(Block newBaseBlock)
        {
            newBaseBlock.PutCharacter(this);
        }

        public void OnAttachInject(Block baseBlock, Joint joint)
        {
            _baseBlock = baseBlock;
            _joint = joint;
        }

        private void OnBlockRemoved(Block block)
        {
            if (_baseBlock == block)
            {
                _blocksStack.Top.PutCharacter(this);
            }
        }

        private void OnDisable()
        {
            //_blocksStack.OnBlockRemoved.RemoveListener(OnBlockRemoved);
            _blocksStack.OnBlockAdded.RemoveListener(OnBlockAdded);
        }
    }
}