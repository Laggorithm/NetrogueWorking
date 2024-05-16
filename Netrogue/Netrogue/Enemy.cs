using System.Numerics;
using ZeroElectric.Vinculum;

namespace Netrogue
{
    internal class Enemy
    {
        private Map level;
        public string MobName { get; set; }
        public int ID { get; private set; }
        public Vector2 Position { get; set; }
        private int spriteIndex;

        public Enemy(string name, int id, Vector2 position, int spriteIndex, Map lvl)
        {
            MobName = name;
            ID = id;
            Position = position;
            level = lvl;
             
            this.spriteIndex = spriteIndex;
        }

        public void Draw()
        {
            int tileX = (spriteIndex % Game.imagesPerRow) * Game.tileSize;
            int tileY = (spriteIndex / Game.imagesPerRow) * Game.tileSize;
            Raylib.DrawTextureRec(level.MapImage, new Rectangle(tileX, tileY, Game.tileSize, Game.tileSize), new Vector2(Position.X * Game.tileSize, Position.Y * Game.tileSize), Raylib.WHITE);
        }
    }
}
