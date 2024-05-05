using System;
using ZeroElectric.Vinculum;

namespace Netrogue
{
    internal class Map
    {
        
        class Tiles
        {
            int MapImageIndex { get; set; }
            public Tiles(int mapImageIndex) 
            {
                MapImageIndex = mapImageIndex;
            }

            
        }

        class Floor : Tiles
        {
            public Floor() : base(1) { }
        }
        class Wall : Tiles
        {
            public Wall() : base(2) { }
        }
        class Exit : Tiles
        {
            public Exit() : base(3) { }
        }
        
        public int mapWidth { get; set; }
        public int[] mapTiles;

        private MapTile[,] tiles;
        private Random random = new Random(); // Random generator for mob spawning

        public int Height { get; private set; }

        public void InitMap()
        {
            mapWidth = mapWidth;
            Height = mapTiles.Length / mapWidth;
            tiles = new MapTile[mapWidth, Height];

            // Initialize mob count and limit
            int mobCount = 0;
            int maxMobs = random.Next(3, 6); // Randomly select mob count between 3 to 5

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
                            // Try to spawn a mob on this floor tile if mob count is within limit and it's a valid spawn position
                            if (mobCount < maxMobs && tiles[x, y] == MapTile.Floor)
                            {
                                // Randomly decide if there's a mob at this tile
                                if (random.Next(0, 100) < 10) // 10% chance of mob
                                {
                                    SetTile(x, y, MapTile.Mob);
                                    mobCount++;
                                }
                            }
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

                    Raylib.DrawRectangle(x * Game.tileSize, y * Game.tileSize, Game.tileSize, Game.tileSize, Raylib.GREEN);
                    Raylib.DrawText((GetTileSymbol(tiles[x, y]).ToString()), x * Game.tileSize, y * Game.tileSize, Game.tileSize, Raylib.WHITE);
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
                case MapTile.Mob:
                    return 'M';
                default:
                    return '?';
            }
        }
    }

    internal enum MapTile
    {
        Floor,
        Wall,
        Exit,
        Mob
    }
}
