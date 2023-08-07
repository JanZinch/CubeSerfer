using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    [RequireComponent(typeof(Collider))]
    public class BlocksStack : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Block> _onBlockAdded;

        [SerializeField] private List<Block> _initialBlocks;
        
        private LinkedList<Block> _blocks;

        public UnityEvent<Block> OnBlockAdded => _onBlockAdded;

        private Tween _allCollisionsWait;
        private Tween _afterCollisionDelay;
        
        private void Awake()
        {
            _blocks = new LinkedList<Block>(_initialBlocks);
            
            
            int i = 0;
            
            foreach (var block in _blocks)
            {
                Debug.Log("i: " + (i++) + " name: " + block.gameObject.name);
            }
        }

        private void OnEnable()
        {
            foreach (Block block in _blocks)
            {
                block.OnCollidedWithObstacle.AddListener(OnBlockCollided);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Block>(out Block otherBlock) && !_blocks.Contains(otherBlock))
            {
                Add(otherBlock);
            }
        }

        public void Add(Block block)
        {
            block.transform.SetParent(transform);
            block.transform.SetSiblingIndex(1);
            
            block.AttachTo(_blocks.First.Value);
            _blocks.AddFirst(block);
            
            OnBlockAdded?.Invoke(block);
            
            int i = 0;
            
            foreach (var blo in _blocks)
            {
                Debug.Log("i: " + (i++) + " name: " + blo.gameObject.name);
            }
        }

        private void OnBlockCollided(Block block)
        {
            if (_afterCollisionDelay != null)
                return;
            
            if (_allCollisionsWait == null)
            {
                _allCollisionsWait = DOVirtual.DelayedCall(Time.fixedDeltaTime * 2.0f, OnStackCollided);
            }
        }

        private void OnStackCollided()
        {
            Block newMovable = _blocks.Last((block) => !block.IsCollided);

            Debug.Log("New movable: " + newMovable.gameObject.name);
            
            Block topCollided = _blocks.First((block) => block.IsCollided);         // TODO убрать
            Debug.Log("Top collided: " + topCollided.gameObject.name);

            IEnumerable<Block> lostBlocks = _blocks.SkipWhile((block) => !block.IsCollided);

            foreach (Block block in lostBlocks)
            {
                block.Lose();
            }
            
            //topCollided.Lose();
            
            //newMovable.SetMovable(true);
            newMovable.Detach();
            
            
            _allCollisionsWait = null;
            _afterCollisionDelay = DOVirtual.DelayedCall(2.0f, () =>
            {
                _afterCollisionDelay.Kill();
            });
        }

        private void OnDisable()
        {
            foreach (Block block in _blocks)
            {
                block.OnCollidedWithObstacle.RemoveListener(OnBlockCollided);
            }
        }


    }
}