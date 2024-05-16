using System;
using System.IO;
using Newtonsoft.Json;
using Netrogue;
using TurboMapReader;

namespace Netrogue_working_
{
    class MapLoader
    {
        public Map ReadMapFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine($"File {filename} not found");
                return null;
            }

            // Load the map using TurboMapReader
            TiledMap loadedTileMap = TurboMapReader.MapReader.LoadMapFromFile(filename);
            if (loadedTileMap == null)
            {
                Console.WriteLine($"Failed to load map from file {filename}");
                return null;
            }

            // Create a new Map object from the loaded TiledMap data
            Map loadedMap = CreateMapObject(loadedTileMap);

            return loadedMap;
        }

        private Map CreateMapObject(TiledMap loadedTileMap)
        {
            Map map = new Map();
            map.LoadFromTileMap(loadedTileMap);
            return map;
        }
    }
}
