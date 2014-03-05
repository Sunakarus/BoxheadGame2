using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BoxheadGame2
{
    internal class Crate
    {
        public Crate(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            hitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        private Texture2D texture;
        public Vector2 position;
        public Rectangle hitbox;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}