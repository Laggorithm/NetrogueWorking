using System.Numerics;
using System.Text.Json.Serialization;
using ZeroElectric.Vinculum;

namespace Netrogue
{
    public class Enemy
    {
        private Map level;

        public int MobLvl { get; private set; }
        public string MobName { get; set; }
        public int ID { get; private set; }
        public Position Position { get; set; }  // Change this to Position
        public int SpriteIndex { get; set; }
        public int HitPoints { get; set; }
        public string Element { get; set; }

        [JsonConstructor]
        public Enemy(string mobName, int id, Position position, int spriteIndex, int moblvl, int hitPoints, Map map) // Update to Position
        {
            MobName = mobName;
            ID = id;
            Position = position;
            SpriteIndex = spriteIndex;
            MobLvl = moblvl;
            HitPoints = hitPoints;
            level = map; // Assign the map instance
        }

        public Enemy(string mobName, int spriteIndex, int hitPoints, string element)
        {
            MobName = mobName;
            SpriteIndex = spriteIndex;
            HitPoints = hitPoints;
            Element = element;
            Position = new Position(0, 0); // Default position
        }

        public void Draw()
        {
            int tileX = (SpriteIndex % Game.imagesPerRow) * Game.tileSize;
            int tileY = (SpriteIndex / Game.imagesPerRow) * Game.tileSize;
            Texture mapImage = level.MapImage; // Use the assigned map
            Rectangle source = new Rectangle(tileX, tileY, Game.tileSize, Game.tileSize);
            Vector2 position = new Vector2(Position.X * Game.tileSize, Position.Y * Game.tileSize);
            Raylib.DrawTextureRec(mapImage, source, position, Raylib.WHITE);
        }

        public Enemy() // Parameterless constructor
        {
            MobName = "New Mob"; // Default name
            ID = 0; // Default ID
            Position = new Position(0, 0); // Default position
            SpriteIndex = 0; // Default sprite index
            HitPoints = 100; // Default hit points
            Element = "Neutral"; // Default element, if applicable
        }

    }
}
