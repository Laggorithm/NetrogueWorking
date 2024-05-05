using Netrogue;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace Netrogue_working_
{
    class MapLoader
    {
        private Random random = new Random();

        public Map LoadTestMap()
        {
            // Load a test map with predefined dimensions
            int width = 20;
            int height = 20;
            Map map = new Map();
            map.InitEmptyMap(width, height);

            // Fill map with border walls
            for (int x = 0; x < width; x++)
            {
                map.SetTile(x, 0, MapTile.Wall); // Top border
                map.SetTile(x, height - 1, MapTile.Wall); // Bottom border
            }
            for (int y = 0; y < height; y++)
            {
                map.SetTile(0, y, MapTile.Wall); // Left border
                map.SetTile(width - 1, y, MapTile.Wall); // Right border
            }

            // Load map content
            char[,] mapContent = LoadMap(width, height);

            // Place map content (excluding border) onto the map
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    switch (mapContent[x, y])
                    {
                        case '-':
                            map.SetTile(x, y, MapTile.Floor);
                            break;
                        case '#':
                            map.SetTile(x, y, MapTile.Wall);
                            break;
                        case 'E':
                            map.SetTile(x, y, MapTile.Exit);
                            break;
                        default:
                            // Invalid tile, treat as floor
                            map.SetTile(x, y, MapTile.Floor);
                            break;
                    }
                }
            }

            // Add mobs to the map after it's fully drawn
            AddMobs(map);

            return map;
        }

        private char[,] LoadMap(int width, int height)
        {
            char[,] mapTiles = new char[width, height];

            // Fill map with floor tiles
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    mapTiles[x, y] = '-';
                }
            }

            // Add walls
            int maxWalls = 35;
            int wallCount = 0;

            while (wallCount < maxWalls)
            {
                int x = random.Next(1, width - 1); // Exclude border
                int y = random.Next(1, height - 1); // Exclude border

                // Ensure there's only one tile of space around each wall
                if (mapTiles[x, y] == '-')
                {
                    mapTiles[x, y] = '#'; // Place wall
                    wallCount++;
                }
            }

            // Add exit
            int exitX, exitY;
            do
            {
                exitX = random.Next(1, width - 1); // Exclude border
                exitY = random.Next(1, height - 1); // Exclude border
            } while (mapTiles[exitX, exitY] != '-'); // Ensure exit is placed on a floor tile
            mapTiles[exitX, exitY] = 'E'; // Place exit

            return mapTiles;
        }

        private void AddMobs(Map map)
        {
            int width = map.mapWidth;
            int height = map.Height;
            int mobCount = 0;
            int maxMobs = random.Next(3, 6); // Randomly select mob count between 3 to 5

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map.GetTile(x, y) == MapTile.Floor)
                    {
                        // Try to spawn a mob on this floor tile if mob count is within limit
                        if (mobCount < maxMobs && random.Next(0, 100) < 10) // 10% chance of mob
                        {
                            map.SetTile(x, y, MapTile.Mob);
                            mobCount++;
                        }
                    }
                }
            }
        }

        public Map ReadMapFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine($"File {filename} not found");
                return LoadTestMap(); // Return the test map as fallback
            }

            string fileContents;

            using (StreamReader reader = File.OpenText(filename))
            {
                // Read all lines into fileContents
                fileContents = reader.ReadToEnd();
            }

            // Deserialize .Json file into string 
            Map loadedMap = JsonConvert.DeserializeObject<Map>(fileContents);

            // Add mobs to the loaded map after it's fully drawn
            AddMobs(loadedMap);

            return loadedMap;
        }

        public void TestFileReading(string filename)
        {
            using (StreamReader reader = File.OpenText(filename))
            {
                Console.WriteLine("File contents:");
                Console.WriteLine();

                string line;
                while (true)
                {
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        break; // End of file
                    }
                    Console.WriteLine(line);
                }
            }
        }
    }

    /*private void DrawMap()
    {


        // Determine the image index based on the player's class
        int rowIndex = (int)Game.MapImageIndex;

        int ImageX = rowIndex % imagesPerRow;
        int ImageY = (int)(rowIndex / imagesPerRow);
        player.imagePixelX = ImageX * tileSize;
        player.imagePixelY = ImageY * tileSize;
        int pixelPositionX = (int)player.position.X * Game.tileSize;
        int pixelPositionY = (int)player.position.Y * Game.tileSize;
        Vector2 pixelPosition = new Vector2(pixelPositionX, pixelPositionY);
        Rectangle imageRect = new Rectangle(player.imagePixelX, player.imagePixelY, Game.tileSize, Game.tileSize);
        Raylib.DrawTextureRec(player.image, imageRect, pixelPosition, Raylib.WHITE);
    }*/
}