using Environment;
using Environment.Collectables;
using UnityEngine;
using UnityEngine.Events;

namespace CoreModel
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private CollectableBlocksStack _blocksStack;

        [HideInInspector] [SerializeField] private UnityEvent _onWon;
        [HideInInspector] [SerializeField] private UnityEvent _onLost; 
        
        public UnityEvent OnWon => _onWon;
        public UnityEvent OnLost => _onLost;
        
        private CollectableBlock _baseBlock;
        
        private void OnEnable()
        {
            _blocksStack.OnBlockAdded.AddListener(OnBlockAdded);
            _blocksStack.OnBlockRemoved.AddListener(OnBlockRemoved);
        }
        
        private void Start()
        {
            _baseBlock = _blocksStack.Top;
        }

        private void OnBlockAdded(CollectableBlock newBaseBlock)
        {
            newBaseBlock.PutObject(transform);
        }

        private void OnBlockRemoved(CollectableBlock block)
        {
            if (_baseBlock == block)
            {
                if (_blocksStack.Top != null)
                {
                    _baseBlock = _blocksStack.Top;
                    _blocksStack.Top.PutObject(transform);
                }
                else
                {
                    Lose();
                }
            }
        }
        
        private void Lose()
        {
            _onLost?.Invoke();
        }

        private void OnDisable()
        {
            _blocksStack.OnBlockRemoved.RemoveListener(OnBlockRemoved);
            _blocksStack.OnBlockAdded.RemoveListener(OnBlockAdded);
        }
    }
}