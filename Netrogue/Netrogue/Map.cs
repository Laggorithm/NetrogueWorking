using Netrogue_working_;
using System;
using System.Collections.Generic;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace Netrogue
{
    internal class Map
    {
        public List<Enemy> enemies;
        public List<Item> items;

        public Map()
        {
            enemies = new List<Enemy>();
            items = new List<Item>();
        }

        public void LoadEnemiesAndItems(MapLayer enemyLayer, MapLayer itemLayer, Texture spriteAtlas)
        {
            LoadEnemies(enemyLayer, spriteAtlas);
            LoadItems(itemLayer, spriteAtlas);
        }

        private void LoadEnemies(MapLayer layer, Texture spriteAtlas)
        {
            for (int y = 0; y < layer.mapTiles.Length / layer.width; y++)
            {
                for (int x = 0; x < layer.width; x++)
                {
                    Vector2 position = new Vector2(x, y);
                    int index = x + y * layer.width;
                    int tileId = layer.mapTiles[index];
                    switch (tileId)
                    {
                        case 1:
                            enemies.Add(new Enemy("Orc", position, spriteAtlas, tileId));
                            break;
                            // Add more cases for different enemy types if needed
                    }
                }
            }
        }

        private void LoadItems(MapLayer layer, Texture spriteAtlas)
        {
            // Implement loading items from the item layer if needed
        }

        public void Draw()
        {
            foreach (var item in items)
            {
                item.Draw();
            }

            foreach (var enemy in enemies)
            {
                enemy.Draw();
            }
        }

        public Enemy GetEnemyAt(int x, int y)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.position.X == x && enemy.position.Y == y)
                {
                    return enemy;
                }
            }
            return null;
        }

        public Item GetItemAt(int x, int y)
        {
            // Implement getting item at position (x, y) if needed
            return null;
        }
    }
}
