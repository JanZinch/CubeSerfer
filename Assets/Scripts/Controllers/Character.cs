using System;
using Environment;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Controllers
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private Joint _joint;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private BlocksStack _blocksStack;

        [SerializeField] private UnityEvent _onWon;
        [SerializeField] private UnityEvent _onLost; 
        
        public UnityEvent OnWon => _onWon;
        public UnityEvent OnLost => _onLost;
        
        public BlocksStack CollectedBlocksStack => _blocksStack;
        public Joint Joint => _joint;
     
        private Block _baseBlock;
        
        private void OnEnable()
        {
            _blocksStack.OnBlockAdded.AddListener(OnBlockAdded);
            _blocksStack.OnBlockRemoved.AddListener(OnBlockRemoved);
        }
        
        private void Start()
        {
            _baseBlock = _blocksStack.Top;
        }

        private void OnBlockAdded(Block newBaseBlock)
        {
            newBaseBlock.PutCharacter(this);
        }

        private void Win()
        {
            _onWon?.Invoke();
        }

        private void Lose()
        {
            Destroy(_joint);
            _onLost?.Invoke();
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
                if (_blocksStack.Top != null)
                {
                    _blocksStack.Top.PutCharacter(this);
                }
                else
                {
                    Lose();
                }
            }
        }

        private void OnDisable()
        {
            _blocksStack.OnBlockRemoved.RemoveListener(OnBlockRemoved);
            _blocksStack.OnBlockAdded.RemoveListener(OnBlockAdded);
        }
    }
}