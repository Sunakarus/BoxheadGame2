using CircleCollisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BoxheadGame2
{
    internal class Bullet
    {
        public Bullet(Texture2D texture, Vector2 position, Vector2 direction)
        {
            this.texture = texture;
            this.position = position;
            this.direction = direction;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);

            scale = radius * 2 / texture.Width;
            hitbox = new Circle(radius, position);
        }

        public Texture2D texture { get; set; }

        public Vector2 position { get; set; }

        public Vector2 direction { get; set; }

        private Vector2 origin;
        private float speed = 15f;
        public float lifeTime = 150f; //5 sec
        public Circle hitbox;

        public float scale;
        public static float radius = 4f;

        public void Update()
        {
            direction.Normalize();
            position += direction * speed;
            if (lifeTime > 0) { lifeTime--; }
            hitbox.position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 1);
        }
    }
}