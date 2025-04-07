using System.Xml;
using UnityEngine;

namespace Codebase
{
    public class CAGenerator
    {
        private int width, height;
        private int rockThreshold = 4; 
        private int generations = 3;

        public CAGenerator(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public Tile[,] ApplyCA(Tile[,] map)
        {
            for (int gen = 0; gen < generations; gen++)
            {
                Tile[,] newMap = new Tile[width, height];

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (y < height - 1 && map[x, y + 1].Type == BlockType.Grass)
                        {
                            newMap[x, y] = map[x, y]; 
                            continue;
                        }
                        int stoneNeighbors = CountStoneNeighbors(map, x, y);
                        Tile current = map[x, y];
                        Tile updated = new Tile
                        {
                            Position = current.Position,
                            Type = current.Type
                        };

                        if (current.Type == BlockType.Stone || current.Type == BlockType.Dirt)
                        {
                            updated.Type = stoneNeighbors >= rockThreshold ? current.Type : BlockType.Air;
                        }

                        newMap[x, y] = updated;
                    }
                }

                map = newMap;
            }

            return map;
        }

        private int CountStoneNeighbors(Tile[,] map, int x, int y)
        {
            int count = 0;
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                    {
                        BlockType type = map[nx, ny].Type;
                        if (type == BlockType.Stone || type == BlockType.Dirt)
                            count++;
                    }
                    else
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
