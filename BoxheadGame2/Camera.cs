using Microsoft.Xna.Framework;

namespace BoxheadGame2
{
    internal class Camera
    {
        public Camera(Player player, GraphicsDeviceManager graphics)
        {
            this.player = player;
            this.graphics = graphics;
            position = player.position - new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
        }

        private Player player;
        public Vector2 position;
        private GraphicsDeviceManager graphics;
        private float marginSize = 160f;

        public void Update()
        {
            Vector2 marginLeft = position + new Vector2(marginSize, marginSize);
            Vector2 marginRight = new Vector2(graphics.PreferredBackBufferWidth - marginSize, graphics.PreferredBackBufferHeight - marginSize) + position;

            if (player.position.X < marginLeft.X)
            {
                position.X -= marginLeft.X - player.position.X;
            }

            if (player.position.X > marginRight.X)
            {
                position.X += player.position.X - marginRight.X;
            }

            if (player.position.Y < marginLeft.Y)
            {
                position.Y -= marginLeft.Y - player.position.Y;
            }

            if (player.position.Y > marginRight.Y)
            {
                position.Y += player.position.Y - marginRight.Y;
            }
        }

        public Matrix GetTransformMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0));
        }
    }
}