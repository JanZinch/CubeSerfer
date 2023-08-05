using System;
using System.Collections.Generic;
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
        
        private void Awake()
        {
            _blocks = new LinkedList<Block>(_initialBlocks);
            
            
            int i = 0;
            
            foreach (var block in _blocks)
            {
                Debug.Log("i: " + (i++) + " name: " + block.gameObject.name);
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
        
        

    }
}