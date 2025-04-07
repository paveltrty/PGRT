using UnityEngine;

namespace Codebase
{
    public class DungeonRoom
    {
        public RectInt Bounds;
        public BlockType FloorType;
        public BlockType WallType;

        public DungeonRoom(RectInt bounds, BlockType floor, BlockType wall)
        {
            Bounds = bounds;
            FloorType = floor;
            WallType = wall;
        }

        public Vector2Int Center => new Vector2Int(
            Bounds.x + Bounds.width / 2,
            Bounds.y + Bounds.height / 2);
    }
}