using System.Collections.Generic;

namespace Codebase
{
    public static class DungeonRoomBuilder
    {
        public static void BuildRooms(Tile[,] map, List<DungeonRoom> rooms)
        {
            foreach (var room in rooms)
            {
                for (int x = room.Bounds.xMin; x < room.Bounds.xMax; x++)
                {
                    for (int y = room.Bounds.yMin; y < room.Bounds.yMax; y++)
                    {
                        bool isWall = x == room.Bounds.xMin || x == room.Bounds.xMax - 1 ||
                                      y == room.Bounds.yMin || y == room.Bounds.yMax - 1;

                        if (IsInsideMap(map, x, y))
                        {
                            map[x, y].Type = isWall ? room.WallType : room.FloorType;
                        }
                    }
                }
            }
        }

        private static bool IsInsideMap(Tile[,] map, int x, int y)
        {
            return x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1);
        }
    }
}