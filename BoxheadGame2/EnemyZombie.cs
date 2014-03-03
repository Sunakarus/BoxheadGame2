using CircleCollisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BoxheadGame2
{
    internal class EnemyZombie : Enemy
    {
        private float movementSpeed = 0.5f;
        private Circle futureHitbox;

        public Texture2D atAttack;
        public Animation aAttack;

        private int attackSpeed = 5;

        public EnemyZombie(Texture2D texture, Vector2 position, Player target, Controller controller, Texture2D atTexture)
            : base(texture, position, target, controller)
        {
            hitbox = new Circle(radius, position - new Vector2(radius, radius));
            state = State.Idle;
            atAttack = atTexture;

            damage = 10;
            attackDelay = new Timer(15);
            aAttack = new Animation(atAttack, 8, 4, 2, position, true, new Timer(attackSpeed));
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

            hitbox = new Circle(radius, position - new Vector2(radius, radius));

            direction = target.position - position;

            direction.Normalize();
            rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);

            if (state != State.Dead)
            {
                float dist = (position - target.position).Length() - target.hitbox.radius - hitbox.radius;
                if (dist < 15)
                {
                    state = State.Attack;
                }
                else
                {
                    state = State.Idle;
                    attackDelay.Reset();
                }

                if (state == State.Attack)
                {
                    aAttack.Update();
                    attackDelay.Update();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case State.Idle:
                    {
                        spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1, SpriteEffects.None, 1);
                        break;
                    }
                case State.Attack:
                    {
                        aAttack.Draw(spriteBatch, position, rotation, origin);
                        break;
                    }
            }
        }
    }
}