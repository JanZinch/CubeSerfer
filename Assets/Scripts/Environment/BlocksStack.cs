﻿using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
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
            //block.transform.SetParent(transform);
            //block.transform.SetAsFirstSibling();
            
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
            Block newMovable = _blocks.First((block) => block.IsCollided);

            Block lastCollided = _blocks.Find(newMovable).Next.Value;
            
            lastCollided.Leave();
            
            newMovable.SetMovable(true);
            
            Debug.Log("New movable: " + newMovable.gameObject.name);
            
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