using System.Collections.Generic;
using UnityEngine;

namespace Codebase
{
    public class TerrainManager : MonoBehaviour
    {
        public int width = 100;
        public int height = 60;
        public int surfaceHeight = 30;
        public float freq = 0.05f;
        public float amp = 6f;
        public int seed = 42;
        public TerrainVisualizer visualizer;
        void Start()
        {
            SurfaceGenerator surfaceGen = new SurfaceGenerator(width, height, surfaceHeight, freq, amp, seed);
            Tile[,] terrain = surfaceGen.GenerateSurface();

            var coast = new CoastlineAgent(width, height);
            terrain = coast.ApplyCoastline(terrain);

            var drunk = new DrunkardsWalkCaves(width, height, seed);
            terrain = drunk.DigCaves(terrain);
            
            var ca = new CAGenerator(width, height);
            terrain = ca.ApplyCA(terrain);
            
            
            RectInt hellZone = new RectInt(0, 0, width, height / 10);
            ClearHellArea(terrain, hellZone);
            HellHouseGenerator houseGen = new HellHouseGenerator(width, height, seed, initialRoomCount: 6);
            var hellRooms = houseGen.Generate(hellZone);
            DungeonRoomBuilder.BuildRooms(terrain, hellRooms);
            HellSupportBuilder.AddSupports(terrain, hellRooms, BlockType.Stone);

            

            visualizer.Visualize(terrain);
        }
        public static void ClearHellArea(Tile[,] map, RectInt zone)
        {
            for (int x = zone.xMin; x < zone.xMax; x++)
            {
                for (int y = zone.yMin; y < zone.yMax; y++)
                {
                    if (IsInsideMap(map, x, y))
                        map[x, y].Type = BlockType.Air;
                }
            }
        }

        private static bool IsInsideMap(Tile[,] map, int x, int y)
        {
            return x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1);
        }

    }
}