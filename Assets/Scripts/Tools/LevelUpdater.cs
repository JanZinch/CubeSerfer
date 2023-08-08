using UnityEngine;
using Environment;
using Environment.Obstacles;

namespace Tools
{
    public class LevelUpdater : MonoBehaviour
    {
        [SerializeField] private Block _blockPrefab;
        [SerializeField] private ObstacleBlock _obstacleBlockPrefab;
        
        [EasyButtons.Button]
        public void UpdateCollectableBlocks()
        {
            GameObject[] updatabaleBlocks = GameObject.FindGameObjectsWithTag("UpdatabaleBlock");

            foreach (var block in updatabaleBlocks)
            {
                Instantiate(_blockPrefab, block.transform.position, Quaternion.identity);
                DestroyImmediate(block.gameObject);
            }
        }

        [EasyButtons.Button]
        public void UpdateObstacleBlocks()
        {
            GameObject[] updatabaleBlocks = GameObject.FindGameObjectsWithTag("ObstacleBlock");

            foreach (var block in updatabaleBlocks)
            {
                Instantiate(_obstacleBlockPrefab, block.transform.position, Quaternion.identity);
                DestroyImmediate(block.gameObject);
            }
        }
        
    }
}
