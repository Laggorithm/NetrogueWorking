using System;
using System.IO;
using Newtonsoft.Json;
using Netrogue;

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

            string fileContents;

            using (StreamReader reader = File.OpenText(filename))
            {
                // Read all lines into fileContents
                fileContents = reader.ReadToEnd();
            }

            // Deserialize JSON file into Map object
            Map loadedMap = JsonConvert.DeserializeObject<Map>(fileContents);

            return loadedMap;
        }
    }
}
