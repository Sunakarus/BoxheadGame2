using CircleCollisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxheadGame2
{
    class Controller
    {
        public List<Bullet> bulletList = new List<Bullet>();
        public List<Crate> crateList = new List<Crate>();
        public List<Enemy> enemyList = new List<Enemy>();

        public Player player;
        
        public Controller(Player player)
        {
            this.player = player;
        }

        public void Update()
        {
            

            for (int i = bulletList.Count -1; i > -1; i--)
            {
                bool breakAll = false;

                Bullet b = bulletList[i];
                b.Update();


                foreach (Crate c in crateList)
                {
                    if (b.hitbox.CollidesWithRect(c.hitbox))
                    {

                        bulletList.RemoveAt(i);
                        Console.WriteLine(" - " + bulletList.Count);
                        breakAll = true;
                        break;
                    }
                }

                if (breakAll)
                { break; }

                if (enemyList.Count > 0)
                {
                    foreach (Enemy e in enemyList)
                    {
                        if (b.hitbox.CollidesWithCircle(e.hitbox))
                        {
                            bulletList.RemoveAt(i);
                            Console.WriteLine(" - " + bulletList.Count);
                            e.state = Enemy.State.Dead;
                            breakAll = true;
                            break;
                        }
                    }
                }

                if (breakAll)
                { break; }

                if (b.lifeTime <= 0)
                {
                    bulletList.RemoveAt(i);
                    Console.WriteLine(" - " + bulletList.Count);
                    break;
                }
            }

            for (int i = enemyList.Count - 1; i>-1; i--)
            {
                enemyList[i].Update();
                if (enemyList[i].state == Enemy.State.Dead)
                {
                    enemyList.RemoveAt(i);
                    break;
                }
            }
        }

        public bool CollidesWithWall(Circle circle)
        {
            foreach (Crate c in crateList)
            {
                if (circle.CollidesWithRect(c.hitbox))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CollidesWithEnemy(Circle circle)
        {
            foreach (Enemy e in enemyList)
            {
                if (circle.CollidesWithCircle(e.hitbox))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CollidesWithPlayer(Circle circle)
        {
            if (circle.CollidesWithCircle(player.hitbox))
            {
                return true;
            }
            return false;
        }

        public void GenerateEnemyZombie(Texture2D texture, Rectangle area)
        {

            Vector2 pos;
            EnemyZombie a;
            int counter = 10;

            do
            {
                pos = new Vector2(Game1.random.Next(area.Width + 1) + area.X, Game1.random.Next(area.Height + 1) + area.Y);
                a = new EnemyZombie(texture, pos, player, this);
                counter--;
            }

            while ((CollidesWithWall(a.hitbox) || CollidesWithPlayer(a.hitbox) || a.GetEnemyCollision(a.hitbox, Vector2.Zero)) && (counter>0));
            if (counter > 0)
            {
                this.enemyList.Add(a);
            }
            else
            {
                Console.WriteLine("Spawning EnemyZombie failed.");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (Crate c in crateList)
            {
                c.Draw(spriteBatch);
            }
            foreach(Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
            foreach(Enemy e in enemyList)
            {
                if (e is EnemyZombie)
                {
                   (e as EnemyZombie).Draw(spriteBatch);
                }
            }

        }
    }
}
