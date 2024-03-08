using Netrogue;
using System;
using System.Reflection.PortableExecutable;
using System.IO;
using Newtonsoft.Json;

namespace Netrogue_working_
{
    class MapLoader
    {
        public char[,] LoadMap(int width, int height)
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
            Random random = new Random();
            int maxWalls = 7;
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

        public Map LoadTestMap()
        {
            // Load a test map with predefined dimensions
            int width = 20;
            int height = 10;
            Map map = new Map(); //(width, height);
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

            return map;
        }

        private bool fileFound(string filename)
        {
            bool exists = File.Exists(filename);
            return exists;
        }
        public Map ReadMapFromFile(string filename)
        {
            if (fileFound(filename) == false)
            {
                Console.WriteLine($"File {filename} not found");
                return LoadTestMap(); // Return the test map as fallback
            }

            string fileContents;

            using (StreamReader reader = File.OpenText(filename))
            {
               //Read all lines into fileContens
               fileContents = reader.ReadToEnd();
            }
            //Deserialize .Json file in to string 
            Map loadedMap = JsonConvert.DeserializeObject<Map>(fileContents);

            return loadedMap;
        }
        public void TestFileReading(string filename)
        {
            using(StreamReader reader = File.OpenText(filename))
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



}
