using UnityEngine;

namespace Codebase
{
    public class CoastlineAgent
    {
        private int width, height;
        private int shoreWidth = 40;
        private float shoreDepthFactor = 0.6f;
        private float noiseScale = 0.05f;
        private float depthNoiseScale = 0.1f;
        private float edgeJaggedness = 5f;

        public CoastlineAgent(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public static int GetLowestSurfaceY(Tile[,] map, int ix)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            int lowestY = height; 

            for (int y = height - 1; y >= 0; y--)
            {
                if (map[ix, y].Type == BlockType.Grass)
                {
                    lowestY = y;
                    break; 
                }
            }
            return lowestY;
        }

        public Tile[,] ApplyCoastline(Tile[,] map)
        {
            float seed = Random.Range(0f, 1000f);
            int rlowy = GetLowestSurfaceY(map, shoreWidth) - 1;
            int llowy = GetLowestSurfaceY(map, width - shoreWidth) - 1;
            for (int x = 0; x < width; x++)
            {
                float coastFactor = 0f;

                if (x < shoreWidth)
                {
                    coastFactor = 1f - (x / (float)shoreWidth);
                }
                else if (x > width - shoreWidth)
                {
                    coastFactor = (x - (width - shoreWidth)) / (float)shoreWidth;
                }

                if (coastFactor > 0)
                {
                    float noiseOffset = Mathf.PerlinNoise(seed, x * noiseScale) * edgeJaggedness;
                    float depthFactor = Mathf.PerlinNoise(x * depthNoiseScale, seed);
                    int maxDepth = Mathf.RoundToInt(height * shoreDepthFactor * coastFactor * Mathf.Lerp(0.7f, 1.3f, depthFactor));

                    int bottomY = height - 1 - Mathf.RoundToInt(noiseOffset);

                    for (int y = bottomY; y >= bottomY - maxDepth; y--)
                    {
                        if (x > width - shoreWidth && y > llowy && map[x, y].Type == BlockType.Air)
                        {
                            continue;
                        }
                        if (x < shoreWidth && y > rlowy && map[x, y].Type == BlockType.Air)
                        {
                            continue;
                        }
                        if (x > width - shoreWidth)
                        {
                            if (y >= 0 && y <= rlowy)
                                map[x, y].Type = BlockType.Water;  
                        }
                        else if (x < shoreWidth)
                        {
                            if (y >= 0 && y <= llowy)
                                map[x, y].Type = BlockType.Water; 
                        }
                    }
                }
            }

            ClearAboveWater(map);
            return map;
        }

        public Tile[,] ClearAboveWater(Tile[,] map)
        {
            int mapWidth = map.GetLength(0);
            int mapHeight = map.GetLength(1);

            for (int x = 0; x < mapWidth; x++)
            {
                int waterY = -1;
                for (int y = mapHeight - 1; y >= 0; y--)
                {
                    if (map[x, y].Type == BlockType.Water)
                    {
                        waterY = y;
                        break;
                    }
                }

                if (waterY != -1)
                {
                    for (int y = waterY + 1; y < mapHeight; y++)
                    {
                        map[x, y].Type = BlockType.Air;
                    }
                }
            }

            return map;
        }
    }
}
