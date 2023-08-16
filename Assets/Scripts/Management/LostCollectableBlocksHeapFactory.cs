using System;
using CoreModel;
using Environment;
using Environment.Collectables;
using UnityEngine;

namespace Management
{
    public class LostCollectableBlocksHeapFactory : MonoBehaviour
    {
        [SerializeField] private CollectableBlocksStack _blocksStack;
        [SerializeField] private LostCollectableBlocksHeap _heapPrefab;
        
        //private static LostCollectableBlocksHeapFactory _factoryInstance = null; 
        private LostCollectableBlocksHeap _heapInstance = null;

        private void OnEnable()
        {
            _blocksStack.OnBlockRemoved.AddListener(OnBlockLost);
        }

        private LostCollectableBlocksHeap GetHeapInstance()
        {
            if (_heapInstance == null)
            {
                _heapInstance = Instantiate<LostCollectableBlocksHeap>(_heapPrefab);
            }

            return _heapInstance;
        }

        private void OnBlockLost(CollectableBlock block)
        {
            LostCollectableBlocksHeap heap = GetHeapInstance();
            heap.Add(block);
        }
    }
}