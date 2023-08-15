using System.Collections.Generic;
using Environment.Collectables;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    [RequireComponent(typeof(Collider))]
    public class CollectableBlocksStack : MonoBehaviour
    {
        [SerializeField] private List<CollectableBlock> _initialBlocks;
        [SerializeField] private UnityEvent<CollectableBlock> _onBlockAdded;
        [SerializeField] private UnityEvent<CollectableBlock> _onBlockRemoved;
        [SerializeField] private TrailRenderer _trail;
        
        private LinkedList<CollectableBlock> _blocks;

        public UnityEvent<CollectableBlock> OnBlockAdded => _onBlockAdded;
        public UnityEvent<CollectableBlock> OnBlockRemoved => _onBlockRemoved;

        public CollectableBlock Top => _blocks.Count > 0 ? _blocks.First.Value : null;
        
        private void Awake()
        {
            _blocks = new LinkedList<CollectableBlock>(_initialBlocks);
            
            for (LinkedListNode<CollectableBlock> node = _blocks.First; node != null; node = node.Next){

                if (node.Next != null)
                {
                    node.Value.AttachTo(node.Next.Value);
                }
            }
        }

        private void OnEnable()
        {
            foreach (CollectableBlock block in _blocks)
            {
                block.OnCollidedWithObstacle.AddListener(OnBlockCollided);
            }
            
            MarkAsLast(_blocks.Last.Value);
        }

        private void Add(CollectableBlock block)
        {
            block.transform.SetParent(transform);
            block.transform.SetSiblingIndex(1);
            
            block.AttachTo(_blocks.First.Value);
            block.OnCollidedWithObstacle.AddListener(OnBlockCollided);
            _blocks.AddFirst(block);
            
            OnBlockAdded?.Invoke(block);
        }

        private void Remove(CollectableBlock block)
        {
            LinkedListNode<CollectableBlock> foundNode = _blocks.Find(block);

            if (foundNode == null)
            {
                Debug.LogWarning("Block doesn't contain in stack");
            }
            else
            {
                if (foundNode.Previous != null && foundNode.Next != null)
                {
                    foundNode.Previous.Value.AttachTo(foundNode.Next.Value);
                }
                else if (foundNode.Previous != null && foundNode.Next == null)
                {
                    OnBlockUngrounded(block);
                    UnmarkAsLast(foundNode.Value);
                    MarkAsLast(foundNode.Previous.Value);
                }
                
                block.OnCollidedWithObstacle.RemoveListener(OnBlockCollided);
                block.Lose();
                
                _blocks.Remove(foundNode);
                
                OnBlockRemoved?.Invoke(block);
            }
        }
        
        private void OnBlockCollided(CollectableBlock block)
        {
            Remove(block);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<CollectableBlock>(out CollectableBlock block) && !block.IsCollided && !_blocks.Contains(block))
            {
                Add(block);
            }
            else if (other.TryGetComponent<Gem>(out Gem gem))
            {
                gem.Collect();
            }
        }
        
        private void OnBlockGrounded(CollectableBlock block)
        {
            _trail.transform.SetParent(block.TrailPivot);
            _trail.transform.localPosition = Vector3.zero;
            _trail.emitting = true;
        }

        private void OnBlockUngrounded(CollectableBlock block)
        {
            _trail.transform.SetParent(transform);
            _trail.emitting = false;
        }
        
        private void MarkAsLast(CollectableBlock block)
        {
            block.OnGrounded.AddListener(OnBlockGrounded);
            block.OnUngrounded.AddListener(OnBlockUngrounded);
        }

        private void UnmarkAsLast(CollectableBlock block)
        {
            block.OnGrounded.RemoveListener(OnBlockGrounded);
            block.OnUngrounded.RemoveListener(OnBlockUngrounded);
        }
        
        private void OnDisable()
        {
            if (_blocks.Count > 0)
            {
                UnmarkAsLast(_blocks.Last.Value);
            }

            foreach (CollectableBlock block in _blocks)
            {
                block.OnCollidedWithObstacle.RemoveListener(OnBlockCollided);
            }
        }
        
    }
}