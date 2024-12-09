using System.Numerics;
using ZeroElectric.Vinculum;

namespace Netrogue
{
    public class Item
    {
        private Map level;
        public int ID { get; private set; }
        public string ItemName { get; private set; }
        public Vector2 Position { get; private set; }
        private int spriteIndex;

        public Item(string itemname, int id, Vector2 position, int spriteIndex, Map level)
        {
            ItemName = itemname;
            ID = id;
            Position = position;
            this.spriteIndex = spriteIndex;
            this.level = level;
        }

        public void Draw()
        {
            int tileX = (spriteIndex % Game.imagesPerRow) * Game.tileSize;
            int tileY = (spriteIndex / Game.imagesPerRow) * Game.tileSize;
            Raylib.DrawTextureRec(level.MapImage, new Rectangle(tileX, tileY, Game.tileSize, Game.tileSize), new Vector2(Position.X * Game.tileSize, Position.Y * Game.tileSize), Raylib.WHITE);
        }
    }
}
