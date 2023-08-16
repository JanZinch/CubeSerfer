using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreModel;
using Environment.Collectables;
using UnityEngine;

namespace Environment
{
    public class LostCollectableBlocksHeap : MonoBehaviour
    {
        [SerializeField] private CollectableBlocksStack _blocksStack;
        
        private List<CollectableBlock> _blocks = new List<CollectableBlock>();
        private Coroutine _heapLifetimeRoutine;
        private static readonly WaitForFixedUpdate WaitForPhysicsUpdate = new WaitForFixedUpdate();
        
        private CollectableBlock Top => _blocks?.Last();
        
        private void OnEnable()
        {
            _blocksStack.OnBlockRemoved.AddListener(Add);
        }
        
        private void Add(CollectableBlock block)
        {
            _blocks.Add(block);
            OrderByHeights();

            _heapLifetimeRoutine ??= StartCoroutine(DelayedHeightsFix());
        }

        private IEnumerator DelayedHeightsFix()
        {
            yield return WaitForPhysicsUpdate;

            RemoveDestroyedBlocks();
            FixBlockHeights();
            _blocks.Clear();

            _heapLifetimeRoutine = null;
        }

        private void RemoveDestroyedBlocks()
        {
            _blocks.RemoveAll(block => block.gameObject == null);
        }

        private void OrderByHeights()
        {
            _blocks = new List<CollectableBlock>(_blocks.OrderBy(everyBlock => everyBlock.transform.position.y));
        }
        
        private void FixBlockHeights()
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                for (int j = 0; j < _blocks.Count; j++)
                {
                    if (i != j && CollectableBlock.AreIntersects(_blocks[i], _blocks[j]))
                    {
                        _blocks[j].AttachTo(Top);
                        OrderByHeights();
                    }
                }
            }
        }
        
        private void OnDisable()
        {
            _blocksStack.OnBlockRemoved.RemoveListener(Add);
        }

    }
}