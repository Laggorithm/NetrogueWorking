using System;

namespace Netrogue
{
    internal class Map
    {
        private MapTile[,] tiles;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            tiles = new MapTile[width, height];
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
                for (int x = 0; x < Width; x++)
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
