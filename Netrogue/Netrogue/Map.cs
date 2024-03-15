using System;

namespace Netrogue
{
    internal class Map
    {
        public int mapWidth { get; set; }
        public int[] mapTiles;

        private MapTile[,] tiles;

       
        
        public int Height { get; private set; }

        public void InitMap()
        {
            mapWidth = mapWidth;
            Height = mapTiles.Length / mapWidth;
            tiles = new MapTile[mapWidth, Height];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int index = x + y * mapWidth; // Calculate index of tile at (x, y)
                    int tileId = mapTiles[index]; // Read the tile value at index
                    switch (tileId)
                    {
                        case 1:
                            SetTile(x, y, MapTile.Floor);
                            break;
                        case 2:
                            SetTile(x, y, MapTile.Wall);
                            break;
                        case 3:
                            SetTile(x, y, MapTile.Exit);
                            break;
                        default:
                            // Invalid tile, treat as floor
                            SetTile(x, y, MapTile.Floor);
                            break;
                    }
                }
            }
        }

        public void InitEmptyMap(int width, int height)
        {
            mapWidth = width;
            Height = height;
            tiles = new MapTile[mapWidth, Height];
        }

        public void SetTile(int x, int y, MapTile tile)
        {
            tiles[x, y] = tile;
        }

        public MapTile GetTile(int x, int y)
        {
            return tiles[x, y];
        }

        public void Draw()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(GetTileSymbol(tiles[x, y]));
                }
            }
        }

        private char GetTileSymbol(MapTile tile)
        {
            switch (tile)
            {
                case MapTile.Floor:
                    return '-';
                case MapTile.Wall:
                    return '#';
                case MapTile.Exit:
                    return 'E';
                default:
                    return '?';
            }
        }
    }

    internal enum MapTile
    {
        Floor,
        Wall,
        Exit
    }
}
