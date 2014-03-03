using CircleCollisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BoxheadGame2
{
    internal class Player
    {
        private Texture2D texture;
        public Texture2D tBullet;
        public Vector2 position;
        private KeyboardState keyState, prevKey;
        private MouseState mouseState, prevMouse;
        private float movementSpeed = 5f, rotation = 0;
        public Vector2 direction, velocity;
        public Controller controller;
        public Circle hitbox;

        public Player(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            hitbox = new Circle(texture.Width / 2, position);
            velocity = Vector2.Zero;
        }

        public void Update()
        {
            prevKey = keyState;
            prevMouse = mouseState;
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            direction = mousePosition - position;
            Circle futureHitbox = new Circle(hitbox.radius, hitbox.position - new Vector2(hitbox.radius, hitbox.radius));

            if (direction.Length() != 0)
            {
                direction.Normalize();
                rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);
            }

            if (keyState.IsKeyDown(Keys.W) && keyState.IsKeyUp(Keys.S))
            {
                //  if (!GetWallCollision(futureHitbox, new Vector2(0,-movementSpeed)))
                velocity.Y = -movementSpeed;
            }

            if (keyState.IsKeyDown(Keys.S) && keyState.IsKeyUp(Keys.W))
            {
                //   if (!GetWallCollision(futureHitbox, new Vector2(0, movementSpeed)))
                velocity.Y = movementSpeed;
            }

            if (keyState.IsKeyDown(Keys.A) && keyState.IsKeyUp(Keys.D))
            {
                // if (!GetWallCollision(futureHitbox, new Vector2(-movementSpeed, 0)))
                velocity.X = -movementSpeed;
            }

            if (keyState.IsKeyDown(Keys.D) && keyState.IsKeyUp(Keys.A))
            {
                //if (!GetWallCollision(futureHitbox, new Vector2(movementSpeed, 0)))
                velocity.X = movementSpeed;
            }

            if (keyState.IsKeyUp(Keys.W) && keyState.IsKeyUp(Keys.S))
            {
                velocity.Y = 0;
            }

            if (keyState.IsKeyUp(Keys.A) && keyState.IsKeyUp(Keys.D))
            {
                velocity.X = 0;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
            {
                controller.bulletList.Add(new Bullet(tBullet, position + direction * (hitbox.radius + tBullet.Width / 2), direction));
                Console.WriteLine(" + " + controller.bulletList.Count);
            }

            Vector2 tempVel = Vector2.Zero;

            if (!GetWallCollision(futureHitbox, new Vector2(velocity.X, 0)) && !GetEnemyCollision(futureHitbox, new Vector2(0, velocity.Y)))
            {
                tempVel.X = velocity.X;
            }

            futureHitbox = new Circle(hitbox.radius, hitbox.position - new Vector2(hitbox.radius, hitbox.radius));

            if (!GetWallCollision(futureHitbox, new Vector2(0, velocity.Y)) && !GetEnemyCollision(futureHitbox, new Vector2(0, velocity.Y)))
            {
                tempVel.Y = velocity.Y;
            }

            position += tempVel;
            hitbox.position = position;
        }

        public bool GetWallCollision(Circle circle, Vector2 offset)
        {
            circle.position += offset;
            if (controller.CollidesWithWall(circle))
            {
                return true;
            }
            return false;
        }

        public bool GetEnemyCollision(Circle circle, Vector2 offset)
        {
            circle.position += offset;
            if (controller.CollidesWithEnemy(circle))
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1, SpriteEffects.None, 1);
        }
    }
}