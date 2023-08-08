using System.Collections.Generic;
using UnityEngine;

namespace Environment.Obstacles
{
    public class ObstacleBlocksLevel : MonoBehaviour
    {
        [SerializeField] private List<ObstacleBlock> _blocks;

        [EasyButtons.Button]
        private void InitializeBlocksList()
        {
            _blocks = new List<ObstacleBlock>(GetComponentsInChildren<ObstacleBlock>());
        }

        private void StartListening()
        {
            foreach (ObstacleBlock block in _blocks)
            {
                block.OnCollided.AddListener(Collide);
            }
        }
        
        private void CompleteListening()
        {
            foreach (ObstacleBlock block in _blocks)
            {
                block.OnCollided.RemoveListener(Collide);
            }
        }

        private void OnEnable()
        {
            StartListening();
        }

        private void Collide()
        {
            CompleteListening();
            
            foreach (ObstacleBlock block in _blocks)
            {
                block.Collide();
            }
        }

        private void OnDisable()
        {
            CompleteListening();
        }
    }
}