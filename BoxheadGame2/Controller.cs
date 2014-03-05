using CircleCollisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BoxheadGame2
{
    internal class Controller
    {
        public List<Bullet> bulletList = new List<Bullet>();
        public List<Crate> crateList = new List<Crate>();
        public List<Enemy> enemyList = new List<Enemy>();
        public List<Spawner> spawnList = new List<Spawner>();

        public Player player;

        public Texture2D tZombieAttack;
        public Texture2D tPlayer, tCrate, tBullet, tZombie;
        public SpriteFont font;

        public Timer timeTillSpawn = new Timer(60, 60, true);
        private int maxEnemies = 50;

        public Controller(Player player)
        {
            this.player = player;
        }

        public void Update()
        {
            timeTillSpawn.Update();

            for (int i = bulletList.Count - 1; i > -1; i--)
            {
                bool breakAll = false;

                Bullet b = bulletList[i];
                b.Update();

                foreach (Crate c in crateList)
                {
                    if (b.hitbox.CollidesWithRect(c.hitbox))
                    {
                        bulletList.RemoveAt(i);
                        breakAll = true;
                        break;
                    }
                }

                if (breakAll)
                { break; }

                foreach (Enemy e in enemyList)
                {
                    if (b.hitbox.CollidesWithCircle(e.hitbox))
                    {
                        bulletList.RemoveAt(i);
                        e.Death();
                        breakAll = true;
                        break;
                    }
                }

                if (breakAll)
                { break; }

                if (b.lifeTime <= 0)
                {
                    bulletList.RemoveAt(i);
                    break;
                }
            }

            for (int i = enemyList.Count - 1; i > -1; i--)
            {
                enemyList[i].Update();

                if (enemyList[i].state == Enemy.State.Attack && !player.invincible && enemyList[i].attackDelay.value <= 0)
                {
                    player.health -= enemyList[i].damage;
                    enemyList[i].attackDelay.Reset();
                    player.invincible = true;
                    player.invincibleTimer.Reset();
                }

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
            if (enemyList.Count < maxEnemies)
            {
                Vector2 pos;
                EnemyZombie a;
                int counter = 10;

                do
                {
                    pos = new Vector2(Game1.random.Next(area.Width + 1) + area.X, Game1.random.Next(area.Height + 1) + area.Y);
                    a = new EnemyZombie(texture, pos, player, this, tZombieAttack);
                    counter--;
                }

                while ((CollidesWithWall(a.hitbox) || CollidesWithPlayer(a.hitbox) || a.GetEnemyCollision(a.hitbox, Vector2.Zero)) && (counter > 0));
                if (counter > 0)
                {
                    this.enemyList.Add(a);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (Spawner s in spawnList)
            {
                spriteBatch.Draw(s.texture, s.position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            foreach (Crate c in crateList)
            {
                c.Draw(spriteBatch);
            }
            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
            foreach (Spawner s in spawnList)
            {
                spriteBatch.Draw(s.texture, s.position, Color.White);
            }
            foreach (Enemy e in enemyList)
            {
                if (e is EnemyZombie)
                {
                    (e as EnemyZombie).Draw(spriteBatch);
                }
            }
            spriteBatch.DrawString(font, "HEALTH: " + player.health, camera.position, Color.White);
        }
    }
}