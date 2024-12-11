using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using ZeroElectric.Vinculum;
using TurboMapReader;

namespace Netrogue
{
    public class Map
    {
        public Texture MapImage { get; set; }
        public MapLayer[] layers { get; set; }
        public int mapWidth { get; set; }
        public int Height { get; private set; }
        private MapTile[,] tiles;
        public static List<Enemy> enemies { get; private set; } = new List<Enemy>();
        public List<Item> items { get; private set; } = new List<Item>();

        // Lists to store the positions of items, mobs, and walls
        public List<Vector2> ItemPositions { get; private set; } = new List<Vector2>();
        public List<Vector2> MobPositions { get; private set; } = new List<Vector2>();
        public List<Vector2> WallPositions { get; private set; } = new List<Vector2>();

        private readonly int[] wallTileIds = { 5, 27, 6, 13, 2, 16, 14, 26, 17, 3, 18 };
        private Dictionary<int, List<Enemy>> enemyLookup = new Dictionary<int, List<Enemy>>();


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
                    int tileId = layers[0].mapTiles[index]; // Adjusted to match the original offset

                    // Check if the tileId exists in the wallTileIds list
                    if (Array.Exists(wallTileIds, id => id == tileId))
                    {
                        SetTile(x, y, MapTile.Wall); // Mark as Wall if tileId matches
                    }
                    else
                    {
                        SetTile(x, y, MapTile.Floor); // Otherwise, mark as Floor
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

        public MapTile GetTileId(int x, int y)
        {
            if (x >= 0 && x < mapWidth && y >= 0 && y < Height)
            {
                return tiles[x, y];
            }
            return MapTile.Wall; // Return an invalid ID if out of bounds
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

            ProcessLayer(groundLayer, (position, tileId) =>
            {
                if (wallTileIds.Contains(tileId))
                {
                    WallPositions.Add(position); // Store the position of the wall
                }
            });

            ProcessLayer(itemLayer, (position, tileId) =>
            {
                if (tileId == 125)
                {
                    int tileIDToFind = tileId;
                    var item = new Item("helmet", tileId, position, tileId-1, this);
                    items.Add(item);
                    ItemPositions.Add(position); // Store the position of the spawned item
                }
            });

            // Process enemies
            ProcessLayer(enemyLayer, (position, tileId) =>
            {
                if (tileId != 0)
                {
                    // Check if the tileId exists in the loaded enemies
                    if (enemyLookup.ContainsKey(tileId))
                    {

                        
                        foreach (var enemyData in enemyLookup[tileId])
                        {
                            foreach (var key in enemyLookup.Keys)
                            {
                                if (key == tileId)
                                {
                                    Console.WriteLine($"Loaded enemy ID in lookup: {key}");

                                    // Create the enemy based on the data found in the JSON
                                    var enemy = new Enemy(
                                        enemyData.MobName, // Use name from JSON data
                                        enemyData.ID,      // Use ID from JSON data
                                        new Position(position.X, position.Y), // Use the position on the map
                                        tileId,            // Use the tileId as the sprite index
                                        this


                                    );

                                    // Add the enemy to the list and store its position
                                    enemies.Add(enemy);
                                    MobPositions.Add(position);

                                    Console.WriteLine($"Spawned enemy {enemy.MobName} at position {position}");
                                }
                                
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine($"No enemy found in JSON for tileId {tileId}");
                    }
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
                        int tileId = mapTiles[tileIndex] -1;

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
                    mapTiles = layer.data.Select(t => t).ToArray() // Adjust tile indices
                };
                layers[i] = mapLayer;
            }

            InitMap();
            LoadEnemiesFromJson("enemies.json");
            LoadEnemiesAndItems();
            
        }


        public void LoadEnemiesFromJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            try
            {
                // Read the JSON file contents
                string jsonContent = File.ReadAllText(filePath);

                // Deserialize the JSON into a list of Enemy objects
                var loadedEnemies = JsonSerializer.Deserialize<List<Enemy>>(jsonContent);

                if (loadedEnemies == null || loadedEnemies.Count == 0)
                {
                    Console.WriteLine("No enemies found in the JSON file.");
                    return;
                }

                // Populate the class-level enemyLookup dictionary
                foreach (var enemy in loadedEnemies)
                {
                    if (!enemyLookup.ContainsKey(enemy.ID))
                    {
                        enemyLookup[enemy.ID] = new List<Enemy>();
                    }

                    enemyLookup[enemy.ID].Add(enemy);
                }

                Console.WriteLine($"Loaded {loadedEnemies.Count} enemies from JSON.");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error loading enemies: {ex.Message}");
                throw;
            }
        }
    }


        public enum MapTile
        {
            Floor,
            Wall,
            Mob
        }

        public class MapLayer
        {
            public string name;
            public int[] mapTiles;
        }
}
