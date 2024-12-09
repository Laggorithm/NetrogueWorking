using System.Numerics;
using ZeroElectric.Vinculum;

namespace Netrogue
{
    public class Enemy
    {
        private Map level; // Map reference for rendering

        public int MobLvl { get; set; }
        public string MobName { get; set; }
        public int ID { get; set; }
        public Position Position { get; set; }
        public int SpriteIndex { get; set; }
        
         

        // Constructor for deserialization
        public Enemy(string mobName, int id, Position position, int spriteIndex, Map level)
        {
            MobName = mobName;
            ID = id;
            Position = position;
            SpriteIndex = spriteIndex;
            this.level = level;
            
             
        }

        // Default constructor for editing in Enemy Editor
        public Enemy()
        {
            MobName = "New Mob";
            ID = 0;
            Position = new Position(0, 0);
            SpriteIndex = 0;
            
        }

        // Assign the map for rendering
        public void SetMap(Map map)
        {
            level = map;
        }

        // Render the enemy in-game
        public void Draw()
        {
            if (level == null) return;

            int tileX = (SpriteIndex % Game.imagesPerRow) * Game.tileSize;
            int tileY = (SpriteIndex / Game.imagesPerRow) * Game.tileSize;
            Raylib.DrawTextureRec(level.MapImage, new Rectangle(tileX, tileY, Game.tileSize, Game.tileSize), new Vector2(Position.X * Game.tileSize, Position.Y * Game.tileSize), Raylib.WHITE);

            
        }
    }
}
