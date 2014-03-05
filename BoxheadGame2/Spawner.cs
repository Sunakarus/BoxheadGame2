using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BoxheadGame2
{
    internal class Spawner
    {
        public Spawner(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            spawnRect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public Texture2D texture { get; set; }

        public Vector2 position { get; set; }

        public Rectangle spawnRect;
    }
}