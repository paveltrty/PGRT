using UnityEngine;

namespace Codebase
{
    public class TerrainVisualizer : MonoBehaviour
    {
        [Header("Block Prefabs")]
        public GameObject grassPrefab;
        public GameObject dirtPrefab;
        public GameObject stonePrefab;
        public Transform tileParent;
        [SerializeField] private GameObject dungeonPrefab;
        [SerializeField] private GameObject waterPrefab;
        [SerializeField] private GameObject hellPrefab;

        public void Visualize(Tile[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tile = map[x, y];
                    GameObject prefab = GetPrefabForBlock(tile.Type);

                    if (prefab != null)
                    {
                        GameObject obj = Instantiate(prefab, new Vector3(x * 0.08f, y * 0.08f, 0), Quaternion.identity, tileParent);

                    }
                }
            }
        }

        private GameObject GetPrefabForBlock(BlockType type)
        {
            return type switch
            {
                BlockType.Grass => grassPrefab,
                BlockType.Dirt => dirtPrefab,
                BlockType.Stone => stonePrefab,
                BlockType.Dungeon => dungeonPrefab,
                BlockType.Water => waterPrefab,
                _ => null
            };
        }
    }
}