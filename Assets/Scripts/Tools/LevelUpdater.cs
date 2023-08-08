using UnityEngine;
using Environment;

namespace Tools
{
    public class LevelUpdater : MonoBehaviour
    {
        [SerializeField] private Block _blockPrefab;
    
        [EasyButtons.Button]
        public void UpdateBlocks()
        {
            GameObject[] updatabaleBlocks = GameObject.FindGameObjectsWithTag("UpdatabaleBlock");

            foreach (var block in updatabaleBlocks)
            {
                Instantiate(_blockPrefab, block.transform.position, Quaternion.identity);
                DestroyImmediate(block.gameObject);
            }
        }
    }
}
