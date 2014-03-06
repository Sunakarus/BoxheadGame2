using CircleCollisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BoxheadGame2
{
    internal class Enemy
    {
        public Texture2D texture;
        public Vector2 position, origin, direction, velocity = Vector2.Zero;
        public Player target;
        public float rotation, radius;
        public Circle hitbox;
        public Controller controller;
        public int scoreValue;

        public enum State { Attack, Idle, Dead };

        public State state;
        public int damage;
        public Timer attackDelay;

        public Enemy(Texture2D texture, Vector2 position, Player target, Controller controller)
        {
            this.texture = texture;
            this.position = position;
            this.target = target;
            this.controller = controller;

            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            radius = (texture.Width / 2 + texture.Height / 2) / 2 - 2;
            hitbox = new Circle(radius, position - new Vector2(radius, radius));
            state = State.Idle;

            direction = target.position - position;
            direction.Normalize();
            rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);
        }

        public virtual void Update()
        {
            /*hitbox = new Circle(radius, position - new Vector2(radius, radius));
            direction = target.position - position;
            direction.Normalize();
            rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);*/
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

        public bool GetPlayerCollision(Circle circle, Vector2 offset)
        {
            circle.position += offset;
            if (controller.CollidesWithPlayer(circle))
            {
                return true;
            }
            return false;
        }

        public bool GetEnemyCollision(Circle circle, Vector2 offset)
        {
            circle.position += offset;
            List<Enemy> tempList = controller.enemyList.ToList<Enemy>();

            for (int i = tempList.Count - 1; i > -1; i--)
            {
                if (tempList[i] == this)
                {
                    tempList.RemoveAt(i);
                    break;
                }
            }

            foreach (Enemy e in tempList)
            {
                if (circle.CollidesWithCircle(e.hitbox))
                {
                    return true;
                }
            }
            return false;
        }

        public void Death()
        {
            state = State.Dead;
        }
    }
}