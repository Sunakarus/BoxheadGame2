using CircleCollisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxheadGame2
{
    class Bullet
    {
        public Bullet(Texture2D texture, Vector2 position, Vector2 direction)
        {
            this.texture = texture;
            this.position = position;
            this.direction = direction;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            hitbox = new Circle(texture.Width / 2, position);
        }

        public Texture2D texture { get; set; }
        public Vector2 position { get; set; }
        public Vector2 direction { get; set; }
        Vector2 origin;
        float speed = 10f;
        public float lifeTime = 150f; //5 sec
        public Circle hitbox;

        public void Update()
        {
            direction.Normalize();
            position += direction * speed;
            if (lifeTime>0) { lifeTime--; }
            hitbox.position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, origin, 1, SpriteEffects.None, 1);
        }

        
    }
}
