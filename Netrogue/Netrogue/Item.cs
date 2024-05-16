using System.Numerics;
using ZeroElectric.Vinculum;

namespace Netrogue
{
    internal class Item
    {
        private Map level;
        public int ID { get; private set; }
        public Vector2 Position { get; private set; }
        private Texture spriteAtlas;
        private int spriteIndex;

        public Item(int id, Vector2 position, Texture spriteAtlas, int spriteIndex, Map level)
        {
            ID = id;
            Position = position;
            this.spriteAtlas = spriteAtlas;
            this.spriteIndex = spriteIndex;
            this.level = level;
        }

        public void Draw()
        {
            int tileX = (spriteIndex % Game.imagesPerRow) * Game.tileSize;
            int tileY = (spriteIndex / Game.imagesPerRow) * Game.tileSize;
            Raylib.DrawTextureRec(spriteAtlas, new Rectangle(tileX, tileY, Game.tileSize, Game.tileSize), new Vector2(Position.X * Game.tileSize, Position.Y * Game.tileSize), Raylib.WHITE);
        }
    }
}
