using CircleCollisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BoxheadGame2
{
    internal class EnemyZombie : Enemy
    {
        private float movementSpeed = 0.8f;

        public Texture2D atAttack;
        public Animation aAttack;

        private int attackSpeed = 5;
        private float attackDistance = 15f;

        public EnemyZombie(Texture2D texture, Vector2 position, Player target, Controller controller, Texture2D atTexture)
            : base(texture, position, target, controller)
        {
            hitbox = new Circle(radius, position - new Vector2(radius, radius));
            state = State.Idle;
            atAttack = atTexture;

            damage = 10;
            attackDelay = new Timer(15);
            aAttack = new Animation(atAttack, 8, 4, 2, position, true, new Timer(attackSpeed));

            //movementSpeed += (float)Game1.random.NextDouble()/2;
        }

        public Vector2 TryMove(Circle circle, Vector2 offset)
        {
            Vector2 tempVel = Vector2.Zero;

            Circle offsetCircle = new Circle(circle.radius, circle.position - new Vector2(circle.radius, circle.radius));

            if (!GetWallCollision(offsetCircle, new Vector2(offset.X, 0)) && !GetPlayerCollision(offsetCircle, new Vector2(0, offset.X)) && !GetEnemyCollision(offsetCircle, new Vector2(0, offset.X)))
            {
                tempVel.X = offset.X;
            }

            offsetCircle = new Circle(circle.radius, circle.position - new Vector2(circle.radius, circle.radius));

            if (!GetWallCollision(offsetCircle, new Vector2(0, offset.Y)) && !GetPlayerCollision(offsetCircle, new Vector2(0, offset.Y)) && !GetEnemyCollision(offsetCircle, new Vector2(0, offset.Y)))
            {
                tempVel.Y = offset.Y;
            }

            return tempVel;
        }

        public override void Update()
        {
            base.Update();

            velocity = direction * movementSpeed;

            Vector2 tempVel = TryMove(hitbox, velocity);

            //Anti zasekavani
            if (tempVel == Vector2.Zero && state != State.Attack && GetEnemyCollision(hitbox, Vector2.Zero))
            {
                Vector2 newDir = velocity * (-1);
                tempVel = TryMove(hitbox, newDir);

                if (tempVel == Vector2.Zero)
                {
                    newDir = new Vector2(velocity.Y, -velocity.X);
                    tempVel = TryMove(hitbox, newDir);
                }

                if (tempVel == Vector2.Zero)
                {
                    newDir = new Vector2(-velocity.Y, velocity.X);
                    tempVel = TryMove(hitbox, newDir);
                }
            }

            position += tempVel;
            hitbox.position = position;
            //direction = target.position - position;
            /*if (tempVel == Vector2.Zero && state != State.Attack && GetEnemyCollision(hitbox, Vector2.Zero))
            {
                direction = target.position - position;
                float ran = (float)Game1.random.NextDouble();
                direction += new Vector2(ran, ran);
                ran = (float)Game1.random.NextDouble() *2;
                direction -= new Vector2(ran, ran);

                rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);
            }
            else
            {*/
            direction = target.position - position;
            direction.Normalize();
            rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);

            if (state != State.Dead)
            {
                float dist = (position - target.position).Length() - target.hitbox.radius - hitbox.radius;
                if (dist < attackDistance)
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