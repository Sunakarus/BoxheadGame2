using CircleCollisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        
        public Controller()
        {
        }

        public void Update()
        {
            for (int i = bulletList.Count -1; i > -1; i--)
            {
                Bullet b = bulletList[i];
                b.Update();


                foreach (Crate c in crateList)
                {
                    if (b.hitbox.CollidesWithRect(c.hitbox))
                    {

                        bulletList.RemoveAt(i);
                        Console.WriteLine(" - " + bulletList.Count);
                        break;
                    }
                }

                if (b.lifeTime <= 0)
                {
                    bulletList.RemoveAt(i);
                    Console.WriteLine(" - " + bulletList.Count);
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

        }
    }
}
