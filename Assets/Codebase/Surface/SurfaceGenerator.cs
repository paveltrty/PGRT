using UnityEngine;

namespace Codebase
{
    public class SurfaceGenerator
    {
        private int worldWidth, worldHeight, surfaceHeight, seed;
        private float terrainFrequency, terrainAmplitude;

        public SurfaceGenerator(int width, int height, int surfaceY, float frequency, float amplitude, int seed)
        {
            this.worldWidth = width;
            this.worldHeight = height;
            this.surfaceHeight = surfaceY;
            this.terrainFrequency = frequency;
            this.terrainAmplitude = amplitude;
            this.seed = seed;
        }

        public Tile[,] GenerateSurface()
        {
            Tile[,] world = new Tile[worldWidth, worldHeight];
            System.Random rng = new System.Random(seed);

            for (int x = 0; x < worldWidth; x++)
            {
                float noise = Mathf.PerlinNoise((x + seed) * terrainFrequency, 0f);
                int surfaceY = surfaceHeight + Mathf.RoundToInt(noise * terrainAmplitude);

                for (int y = 0; y < worldHeight; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    Tile tile = new Tile { Position = pos };
                    
                    if (y > surfaceY)
                    {
                        tile.Type = BlockType.Air;
                    }
                    else if (y == surfaceY)
                    {
                        tile.Type = BlockType.Grass;
                    }
                    else
                    {
                        int depth = surfaceY - y;
                        int maxDirtDepth = Mathf.CeilToInt(worldHeight * 0.2f);

                        if (depth <= maxDirtDepth)
                        {
                            tile.Type = rng.NextDouble() < 0.8 ? BlockType.Dirt : BlockType.Stone;
                        }
                        else
                        {
                            float chance = (float)rng.NextDouble();
                            if (chance < 0.2f)
                            {
                                tile.Type = BlockType.Dirt;
                            }
                            else
                            {
                                tile.Type = rng.NextDouble() < 0.4f ? BlockType.Air : BlockType.Stone;
                            }
                        }
                    }

                    world[x, y] = tile;
                }
            }

            return world;
        }
    }
}
