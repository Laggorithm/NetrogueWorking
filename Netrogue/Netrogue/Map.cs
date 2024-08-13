using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using ZeroElectric.Vinculum;
using TurboMapReader;

namespace Netrogue
{
    internal class Map
    {
        public Texture MapImage { get; set; }
        public MapLayer[] layers { get; set; }
        public int mapWidth { get; set; }
        public int Height { get; private set; }
        private MapTile[,] tiles;
        public List<Enemy> enemies { get; private set; } = new List<Enemy>();
        public List<Item> items { get; private set; } = new List<Item>();

        // Lists to store the positions of items, mobs, and walls
        public List<Vector2> ItemPositions { get; private set; } = new List<Vector2>();
        public List<Vector2> MobPositions { get; private set; } = new List<Vector2>();
        public List<Vector2> WallPositions { get; private set; } = new List<Vector2>();

        private readonly int[] wallTileIds = { 5, 27, 6, 13, 25, 16, 14, 26, 17, 3, 18 };


        public Map()
        {
            layers = new MapLayer[3]; // Assuming 3 layers: ground, items, enemies
        }

        public void InitMap()
        {
            if (layers == null || layers[0] == null || layers[0].mapTiles == null)
            {
                throw new Exception("Map layers or map tiles not properly initialized.");
            }

            Height = layers[0].mapTiles.Length / mapWidth;
            tiles = new MapTile[mapWidth, Height];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int index = x + y * mapWidth;
                    int tileId = layers[0].mapTiles[index];
                    switch (tileId)
                    {
                        case (int)MapTile.Floor:
                            SetTile(x, y, MapTile.Floor);
                            break;
                        case (int)MapTile.Wall:
                            SetTile(x, y, MapTile.Wall);
                            break;
                        case (int)MapTile.Exit:
                            SetTile(x, y, MapTile.Exit);
                            break;
                        default:
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

        public int GetTileId(int x, int y)
        {
            if (x >= 0 && x < mapWidth && y >= 0 && y < Height)
            {
                return (int)tiles[x, y];
            }
            return -1; // Return an invalid ID if out of bounds
        }

        public MapLayer GetLayer(string layerName)
        {
            foreach (var layer in layers)
            {
                if (layer.name == layerName)
                {
                    return layer;
                }
            }
            return null; // Wanted layer was not found!
        }

        public void LoadEnemiesAndItems()
        {
            enemies.Clear();
            ItemPositions.Clear();
            MobPositions.Clear();
            WallPositions.Clear();

            MapLayer enemyLayer = GetLayer("Monsters");
            MapLayer itemLayer = GetLayer("items");
            MapLayer groundLayer = GetLayer("Ground");

            // Process enemies
            ProcessLayer(enemyLayer, (position, tileId) =>
            {
                if (tileId == 111)
                {
                    var enemy = new Enemy("Mage", 111, position, tileId, this);
                    enemies.Add(enemy);
                    MobPositions.Add(position); // Store the position of the spawned mob
                }
            });

            // Process items
            ProcessLayer(itemLayer, (position, tileId) =>
            {
                if (tileId == 124)
                {
                    var item = new Item("helmet", 124, position, tileId, this);
                    items.Add(item);
                    ItemPositions.Add(position); // Store the position of the spawned item
                }
            });

            // Process walls
            ProcessLayer(groundLayer, (position, tileId) =>
            {
                if (wallTileIds.Contains(tileId))
                {
                    WallPositions.Add(position); // Store the position of the wall
                }
            });
        }

        private void ProcessLayer(MapLayer layer, Action<Vector2, int> processTile)
        {
            if (layer == null || layer.mapTiles == null) return;

            int[] tilesArray = layer.mapTiles;
            int mapHeight = tilesArray.Length / mapWidth;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    Vector2 position = new Vector2(x, y);
                    int index = x + y * mapWidth;
                    int tileId = tilesArray[index];
                    processTile(position, tileId);
                }
            }
        }

        public void Draw()
        {
            MapLayer groundLayer = GetLayer("Ground");

            if (groundLayer != null)
            {
                int[] mapTiles = groundLayer.mapTiles;
                int mapHeight = mapTiles.Length / mapWidth;

                for (int y = 0; y < mapHeight; y++)
                {
                    for (int x = 0; x < mapWidth; x++)
                    {
                        int tileIndex = x + y * mapWidth;
                        int tileId = mapTiles[tileIndex];

                        int TileX = (tileId % Game.imagesPerRow) * Game.tileSize;
                        int TileY = (tileId / Game.imagesPerRow) * Game.tileSize;

                        Raylib.DrawTextureRec(MapImage, new Rectangle(TileX, TileY, Game.tileSize, Game.tileSize), new Vector2(x * Game.tileSize, y * Game.tileSize), Raylib.WHITE);
                    }
                }
            }

            foreach (var enemy in enemies)
            {
                enemy.Draw();
            }

            foreach (var item in items)
            {
                item.Draw();
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

        public void LoadFromTileMap(TiledMap tileMap)
        {
            mapWidth = tileMap.width;
            Height = tileMap.height;
            tiles = new MapTile[mapWidth, Height];
            layers = new MapLayer[tileMap.layers.Count];

            for (int i = 0; i < tileMap.layers.Count; i++)
            {
                var layer = tileMap.layers[i];
                MapLayer mapLayer = new MapLayer
                {
                    name = layer.name,
                    mapTiles = layer.data.Select(t => t - 1).ToArray() // Adjust tile indices
                };
                layers[i] = mapLayer;
            }

            InitMap();
            LoadEnemiesAndItems();
        }
    }

    internal enum MapTile
    {
        Floor,
        Wall,
        Exit,
        Mob
    }

    internal class MapLayer
    {
        public string name;
        public int[] mapTiles;
    }
}
