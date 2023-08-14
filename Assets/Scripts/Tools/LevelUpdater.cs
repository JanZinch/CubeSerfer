#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using Environment;
using Environment.Obstacles;

namespace Tools
{
    public class LevelUpdater : MonoBehaviour
    {
        [SerializeField] private Block _blockPrefab;
        [SerializeField] private ObstacleBlock _obstacleBlockPrefab;

#if UNITY_EDITOR
        
        private void ReplaceWithPrefab(string objectsTag, GameObject prefab)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(objectsTag);

            foreach (var obj in objects)
            {
                GameObject prefabInstance = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
                prefabInstance.transform.parent = obj.transform.parent;
                prefabInstance.transform.localPosition = obj.transform.localPosition;
                prefabInstance.transform.localRotation = obj.transform.localRotation;
                DestroyImmediate(obj.gameObject);
            }
        }

        [EasyButtons.Button]
        public void UpdateCollectableBlocks()
        {
            ReplaceWithPrefab("UpdatableBlock", _blockPrefab.gameObject);
        }

        [EasyButtons.Button]
        public void UpdateObstacleBlocks()
        {
            ReplaceWithPrefab("ObstacleBlock", _obstacleBlockPrefab.gameObject);
        }
#endif
        
    }
}
