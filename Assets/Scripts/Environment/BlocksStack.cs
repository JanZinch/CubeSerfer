using System.Collections.Generic;
using Environment.Collectables;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    [RequireComponent(typeof(Collider))]
    public class BlocksStack : MonoBehaviour
    {
        [SerializeField] private List<Block> _initialBlocks;
        [SerializeField] private UnityEvent<Block> _onBlockAdded;
        [SerializeField] private UnityEvent<Block> _onBlockRemoved;
        [SerializeField] private TrailRenderer _trail;
        
        private LinkedList<Block> _blocks;

        public UnityEvent<Block> OnBlockAdded => _onBlockAdded;
        public UnityEvent<Block> OnBlockRemoved => _onBlockRemoved;

        public Block Top => _blocks.Count > 0 ? _blocks.First.Value : null;


        private void Awake()
        {
            _blocks = new LinkedList<Block>(_initialBlocks);
            
            for (LinkedListNode<Block> node = _blocks.First; node != null; node = node.Next){

                if (node.Next != null)
                {
                    node.Value.AttachTo(node.Next.Value);
                }
            }
        }

        private void OnEnable()
        {
            foreach (Block block in _blocks)
            {
                block.OnCollidedWithObstacle.AddListener(OnBlockCollided);
            }
            
            MarkAsLast(_blocks.Last.Value);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Block>(out Block block) && !_blocks.Contains(block))
            {
                Add(block);
            }
            else if (other.TryGetComponent<Gem>(out Gem gem))
            {
                gem.Collect();
            }
        }

        private void Add(Block block)
        {
            block.transform.SetParent(transform);
            block.transform.SetSiblingIndex(1);
            
            block.AttachTo(_blocks.First.Value);
            block.OnCollidedWithObstacle.AddListener(OnBlockCollided);
            _blocks.AddFirst(block);
            
            OnBlockAdded?.Invoke(block);
        }

        private void Remove(Block block)
        {
            LinkedListNode<Block> foundNode = _blocks.Find(block);

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
                else if (foundNode.Previous == null && foundNode.Next != null)
                {
                    //foundNode.Next.Value.PutCharacter();
                }
                else if (foundNode.Previous != null && foundNode.Next == null)
                {
                    foundNode.Previous.Value.Detach();
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


        private void OnBlockCollided(Block block)
        {
            Remove(block);
        }

        private void MarkAsLast(Block block)
        {
            block.OnGrounded.AddListener(OnBlockGrounded);
            block.OnUngrounded.AddListener(OnBlockUngrounded);
        }

        private void UnmarkAsLast(Block block)
        {
            block.OnGrounded.RemoveListener(OnBlockGrounded);
            block.OnUngrounded.RemoveListener(OnBlockUngrounded);
        }

        private void OnBlockGrounded(Block block)
        {
            _trail.transform.SetParent(block.TrailPivot);
            _trail.transform.localPosition = Vector3.zero;
            _trail.emitting = true;
            
        }

        private void OnBlockUngrounded(Block block)
        {
            _trail.transform.SetParent(transform);
            _trail.emitting = false;
        }

        private void OnDisable()
        {
            if (_blocks.Count > 0)
            {
                UnmarkAsLast(_blocks.Last.Value);
            }

            foreach (Block block in _blocks)
            {
                block.OnCollidedWithObstacle.RemoveListener(OnBlockCollided);
            }
        }


    }
}