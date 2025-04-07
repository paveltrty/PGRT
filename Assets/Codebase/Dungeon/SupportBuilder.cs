using System.Collections.Generic;
using Codebase;

public static class HellSupportBuilder
{
    public static void AddSupports(Tile[,] map, List<DungeonRoom> rooms, BlockType supportType = BlockType.Stone)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        bool[,] isRoomTile = new bool[width, height];
        foreach (var room in rooms)
        {
            for (int x = room.Bounds.xMin; x < room.Bounds.xMax; x++)
            {
                for (int y = room.Bounds.yMin; y < room.Bounds.yMax; y++)
                {
                    if (x >= 0 && x < width && y >= 0 && y < height)
                        isRoomTile[x, y] = true;
                }
            }
        }

        foreach (var room in rooms)
        {
            for (int x = room.Bounds.xMin; x < room.Bounds.xMax; x++)
            {
                int startY = room.Bounds.yMin;

                for (int y = startY - 1; y >= 0; y--)
                {
                    if (!IsInsideMap(map, x, y)) break;

                    if (isRoomTile[x, y])
                        break; 

                    if (map[x, y].Type != BlockType.Air)
                        break; 

                    map[x, y].Type = supportType;
                }
            }
        }
    }

    private static bool IsInsideMap(Tile[,] map, int x, int y)
    {
        return x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1);
    }
}