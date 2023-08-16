using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreModel;
using DG.Tweening;
using Environment.Collectables;
using UnityEngine;

namespace Environment
{
    public class LostCollectableBlocksHeap : MonoBehaviour
    {
        [SerializeField] private CollectableBlocksStack _blocksStack;
        
        [SerializeField] private List<CollectableBlock> _blocks = new List<CollectableBlock>();

        private const float CollectionTime = 0.001f;

        private Coroutine _collectingRoutine;
        
        private void OnEnable()
        {
            _blocksStack.OnBlockRemoved.AddListener(Add);
        }
        
        public void Add(CollectableBlock block)
        {
            _blocks.Add(block);
            OrderByHeights();

            if (_collectingRoutine == null)
            {
                _collectingRoutine = StartCoroutine(Collecting());
            }
        }

        private IEnumerator Collecting()
        {
            yield return new WaitForFixedUpdate();

            Fix();
            _blocks.Clear();

            _collectingRoutine = null;
        }


        private CollectableBlock Top => _blocks.Last();

        private void OrderByHeights()
        {
            _blocks = new List<CollectableBlock>(_blocks.OrderBy(everyBlock => everyBlock.transform.position.y));
        }

        [EasyButtons.Button]
        private void Fix()
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                for (int j = 0; j < _blocks.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    else
                    {
                        if (CollectableBlock.AreIntersects(_blocks[i], _blocks[j]))
                        {
                            Debug.LogWarning("INTERSECTION");
                            
                            _blocks[j].AttachTo(Top);
                            OrderByHeights();
                            
                            /*Fix();
                            break;*/
                        }
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