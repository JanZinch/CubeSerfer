using System;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class BlocksStack : MonoBehaviour
    {
        private LinkedList<Block> _blocks;

        private void Awake()
        {
            _blocks = new LinkedList<Block>(GetComponentsInChildren<Block>());

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
            block.transform.SetParent(transform);
            block.transform.SetAsFirstSibling();
            
            block.AttachTo(_blocks.First.Value);
            _blocks.AddFirst(block);
            
            int i = 0;
            
            foreach (var blo in _blocks)
            {
                Debug.Log("i: " + (i++) + " name: " + blo.gameObject.name);
            }
        }
        
        

    }
}