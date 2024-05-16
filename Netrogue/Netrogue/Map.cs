using System;
using System.Collections.Generic;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace Netrogue
{
    internal class Map
    {
        public Texture MapImage { get; set; }
        public MapLayer[] layers { get; set; }
        public int mapWidth { get; set; }
        public int Height { get; private set; }
        private MapTile[,] tiles;
        public List<Enemy> enemies { get; set; }
        public List<Item> items { get; set; }

        
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
            enemies = new List<Enemy>();
            MapLayer enemyLayer = GetLayer("enemies");

            int[] enemyTiles = enemyLayer.mapTiles;
            int mapHeight = enemyTiles.Length / mapWidth;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    Vector2 position = new Vector2(x, y);

                    int index = x + y * mapWidth;
                    int tileId = enemyTiles[index];
                    if (tileId != 0)
                    {
                        switch (tileId)
                        {
                            case 0: break;
                            case 111: enemies.Add(new Enemy("Mage", 111, position, tileId, this)); break;

                        }
                    }
                }
            }

            items = new List<Item>();
            MapLayer itemLayer = GetLayer("items");

            int[] itemTiles = itemLayer.mapTiles;
            int itemMapHeight = itemTiles.Length / mapWidth;
            for (int y = 0; y < itemMapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    Vector2 position = new Vector2(x, y);

                    int index = x + y * mapWidth;
                    int tileId = itemTiles[index];
                    switch (tileId)
                    {
                        case 0: break;
                        case 124: items.Add(new Item("helmet", 124, position, tileId, this)); break;
                    }
                }
            }
        }
        public void Draw()
        {
            MapLayer groundLayer = GetLayer("ground");

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
    }

    internal enum MapTile
    {
        Floor = 0,
        Wall = 40,
        Exit = 35,
        Mob = 111
    }

    internal class MapLayer
    {
        public string name;
        public int[] mapTiles;
    }
}
