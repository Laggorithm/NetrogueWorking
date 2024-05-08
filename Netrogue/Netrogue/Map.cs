using System;

namespace Netrogue
{
    internal class Map
    {
        public int mapWidth { get; set; }
        public int Height { get; set; } // Added Height property

        public MapTile[,] tiles; // Changed from private to public

        public void InitMap()
        {
            int height = Height; // Changed to use Height property
            tiles = new MapTile[mapWidth, height];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int tileId = tiles[x, y];
                    tiles[x, y] = (MapTile)tileId;
                }
            }
        }

        public void InitEmptyMap(int width, int height)
        {
            mapWidth = width;
            Height = height; // Set the Height property
            tiles = new MapTile[mapWidth, Height];
        }

        public MapTile GetTile(int x, int y)
        {
            return tiles[x, y];
        }
    }

    internal enum MapTile
    {
        Floor = 1,
        Wall = 40,
        Exit = 9,
        Mob = 119
    }
}
