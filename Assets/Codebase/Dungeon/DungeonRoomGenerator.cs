using System.Collections.Generic;
using UnityEngine;

namespace Codebase
{
    public class HellHouseGenerator
    {
        private int worldWidth, worldHeight;
        private int minRoomSize = 4;
        private int maxRoomSize = 8;
        private int spacing = 0;

        private int maxBranchDepth = 3;
        private int initialRoomCount = 5;
        private System.Random rng;

        public HellHouseGenerator(int worldWidth, int worldHeight, int seed = 42, int initialRoomCount = 5)
        {
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            this.initialRoomCount = initialRoomCount;
            rng = new System.Random(seed);
        }

        public List<DungeonRoom> Generate(RectInt zone)
        {
            List<DungeonRoom> allRooms = new List<DungeonRoom>();
            List<DungeonRoom> frontier = new List<DungeonRoom>();

            int spacing = zone.width / initialRoomCount;

            for (int i = 0; i < initialRoomCount; i++)
            {
                int x = zone.xMin + i * spacing + spacing / 2;
                int width = rng.Next(minRoomSize, maxRoomSize);
                int height = rng.Next(minRoomSize, maxRoomSize);
                int y = zone.yMin + rng.Next(0, zone.height - height);

                var rect = new RectInt(x - width / 2, y, width, height);
                var room = new DungeonRoom(rect, BlockType.Air, BlockType.Dungeon);
                allRooms.Add(room);
                frontier.Add(room);

                GrowRoomChain(room, frontier, allRooms, 0);
            }

            return allRooms;
        }

        private void GrowRoomChain(DungeonRoom parent, List<DungeonRoom> frontier, List<DungeonRoom> allRooms, int depth)
        {
            if (depth >= maxBranchDepth) return;

            int branchCount = rng.Next(1, 3); // 1 or 2 branches

            for (int i = 0; i < branchCount; i++)
            {
                Vector2Int direction = GetDirection(); // Up, Left, or Right

                RectInt newRect = direction switch
                {
                    Vector2Int dir when dir == Vector2Int.up => new RectInt(
                        parent.Bounds.xMin,
                        parent.Bounds.yMax + spacing,
                        parent.Bounds.width,
                        rng.Next(minRoomSize, maxRoomSize)
                    ),

                    Vector2Int dir when dir == Vector2Int.right => new RectInt(
                        parent.Bounds.xMax + spacing,
                        parent.Bounds.yMin,
                        rng.Next(minRoomSize, maxRoomSize),
                        parent.Bounds.height
                    ),

                    Vector2Int dir when dir == Vector2Int.left => new RectInt(
                        parent.Bounds.xMin - spacing - rng.Next(minRoomSize, maxRoomSize),
                        parent.Bounds.yMin,
                        rng.Next(minRoomSize, maxRoomSize),
                        parent.Bounds.height
                    ),

                    _ => parent.Bounds
                };

                // Check if inside map
                if (newRect.xMin < 0 || newRect.xMax >= worldWidth ||
                    newRect.yMin < 0 || newRect.yMax >= worldHeight)
                    continue;

                var newRoom = new DungeonRoom(newRect, BlockType.Air, BlockType.Dungeon);
                allRooms.Add(newRoom);

                // Recursively grow
                GrowRoomChain(newRoom, frontier, allRooms, depth + 1);
            }
        }

        private Vector2Int GetDirection()
        {
            int roll = rng.Next(3);
            return roll switch
            {
                0 => Vector2Int.up,
                1 => Vector2Int.left,
                2 => Vector2Int.right,
                _ => Vector2Int.up
            };
        }
    }
}
