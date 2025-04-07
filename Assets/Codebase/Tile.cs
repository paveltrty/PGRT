using UnityEngine;
public enum BlockType
{
    Air,
    Grass,
    Dirt,
    Stone,
    Dungeon,
    Water
}

public class Tile
{
    public Vector2Int Position;
    public BlockType Type;
}

