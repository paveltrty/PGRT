using UnityEngine;
using System;

public class DrunkardsWalkCaves
{
    private int width, height, seed;

    private int numWalks = 3;
    private int maxSteps = 20000;
    private float digChance = 1.0f;
    private int digRadius = 1;

    public DrunkardsWalkCaves(int width, int height, int seed = 42)
    {
        this.width = width;
        this.height = height;
        this.seed = seed;
    }

    public Tile[,] DigCaves(Tile[,] map)
    {
        System.Random rng = new System.Random(seed);
        int spacing = width / numWalks;

        for (int i = 0; i < numWalks; i++)
        {
            int startX = i * spacing + spacing / 2;
            if (startX >= width) break;

            int startY = FindSurfaceY(map, startX);
            if (startY == -1) continue;

            int x = startX;
            int y = startY;

            for (int step = 0; step < maxSteps; step++)
            {
                if (x < 1 || x >= width - 1 || y < 1 || y >= height - 1)
                    break;

                if (rng.NextDouble() < digChance)
                    DigCircle(map, x, y, digRadius);

                int dir = rng.Next(100);
                if (dir < 30) y--;       
                else if (dir < 60) x++;  
                else if (dir < 90) x--;
                else y++;
            }
        }

        return map;
    }

    private void DigCircle(Tile[,] map, int cx, int cy, int radius)
    {
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                int nx = cx + dx;
                int ny = cy + dy;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (dx * dx + dy * dy <= radius * radius)
                    {
                        map[nx, ny].Type = BlockType.Air;
                    }
                }
            }
        }
    }

    private int FindSurfaceY(Tile[,] map, int x)
    {
        for (int y = height - 1; y >= 0; y--)
        {
            if (map[x, y].Type == BlockType.Grass)
                return y - 1;
        }
        return -1;
    }
}
