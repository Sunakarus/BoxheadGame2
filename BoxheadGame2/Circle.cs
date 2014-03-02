using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleCollisions
{
    class Circle
    {
        public float radius, scale;
        public Texture2D texture;
        public Vector2 origin, position;
        SpriteBatch spriteBatch;

        public Circle(float radius, Vector2 position, SpriteBatch spriteBatch, Texture2D texture)
        {
            this.radius = radius;
            this.position = position;
            this.texture = texture;
            this.spriteBatch = spriteBatch;

            origin = new Vector2(texture.Width/2, texture.Height/2);
            scale = radius * 2 / texture.Width;
            
        }

        public Circle(float radius, Vector2 position)
        {
            this.radius = radius;
            this.position = position;
            this.position += new Vector2(radius, radius);
        }

        public bool CollidesWithCircle(Circle circle)
        {
            double sqrDistance = Math.Pow((this.position.X - circle.position.X), 2.0d) + Math.Pow((this.position.Y - circle.position.Y), 2.0d);
            double sqrRadius = Math.Pow(this.radius + circle.radius, 2);
            return (sqrDistance <= sqrRadius);

        }

        public bool CollidesWithRect(Rectangle rect)
        {
            //explanation: http://stackoverflow.com/a/402010/3348935
            //rectangle must be axis-aligned

            Point rectOrigin = new Point(rect.X + rect.Width/2, rect.Y + rect.Height/2);

            double distanceX = Math.Abs(position.X - rectOrigin.X);
            double distanceY = Math.Abs(position.Y - rectOrigin.Y);

            if (distanceX > (rect.Width / 2 + radius)) { return false; }
            if (distanceY > (rect.Height / 2 + radius)) { return false; }

            if (distanceX <= (rect.Width / 2)) 
            { return true; }
            if (distanceY <= (rect.Height / 2)) { return true; }

            double sqrDistance = Math.Pow(distanceX - (rect.Width/2), 2) + Math.Pow(distanceY - (rect.Height/2), 2);

            return (sqrDistance <= Math.Pow(radius, 2));
        }

        public void Update()
        {
            
        }

        public void Draw()
        {
            if (spriteBatch != null && texture != null)
            {
                scale = radius * 2 / texture.Width;
                spriteBatch.Draw(texture, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 1);
            }
        }

        public void Draw(Texture2D texture, SpriteBatch spriteBatch)
        {
            if (this.spriteBatch == null && this.texture == null)
            {
                scale = radius * 2 / texture.Width;
                spriteBatch.Draw(texture, position - new Vector2(radius, radius), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
            }
        }

    }
}
