using CircleCollisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxheadGame2
{
    class EnemyZombie : Enemy
    {
        float movementSpeed = 0.5f;
        Circle futureHitbox;

        public EnemyZombie(Texture2D texture, Vector2 position, Player target, Controller controller) : base(texture, position, target, controller)
        {
            hitbox = new Circle(radius, position - new Vector2(radius, radius));
            state = State.Idle;
        }

        public override void Update()
        {
            base.Update();
            

            velocity = direction * movementSpeed;

            Vector2 tempVel = Vector2.Zero;

            futureHitbox = new Circle(hitbox.radius, hitbox.position - new Vector2(hitbox.radius, hitbox.radius));

            if (!GetWallCollision(futureHitbox, new Vector2(velocity.X, 0)) && !GetPlayerCollision(futureHitbox, new Vector2(0, velocity.X)) && !GetEnemyCollision(futureHitbox, new Vector2(0, velocity.X)))
            {
                tempVel.X = velocity.X;
            }

            futureHitbox = new Circle(hitbox.radius, hitbox.position - new Vector2(hitbox.radius, hitbox.radius));

            if (!GetWallCollision(futureHitbox, new Vector2(0, velocity.Y)) && !GetPlayerCollision(futureHitbox, new Vector2(0, velocity.Y)) && !GetEnemyCollision(futureHitbox, new Vector2(0, velocity.Y)))
            {
                tempVel.Y = velocity.Y;
            }

            position += tempVel;
            hitbox.position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1, SpriteEffects.None, 1);
        }
    }
}
