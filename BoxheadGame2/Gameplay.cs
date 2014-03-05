using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace BoxheadGame2
{
    internal class Gameplay
    {
        public GraphicsDeviceManager graphics;

        //public SpriteBatch spriteBatch;
        public Texture2D tPlayer, tCrate, tBullet, tZombie, tSpawner, tZombieAttack;

        public SpriteFont font;
        public Player player;
        public Controller controller;
        public Camera camera;

        private KeyboardState state, prevState;
        private bool drawHitbox = true;

        private string dirPath = @"C:\Users\HanThi\Disk Google\Dropbox\Projects\BoxheadGame2\level.txt";
        private StreamReader stream;

        public Gameplay(GraphicsDeviceManager graphics, Controller controller, Player player)
        {
            this.graphics = graphics;
            this.controller = controller;
            this.player = player;
            //LoadLevel();

            state = Keyboard.GetState();
        }

        public void LoadLevel()
        {
            if (File.Exists(dirPath))
            {
                stream = new StreamReader(dirPath);
                string s, sPlayer = "";
                List<string> wCrateList = new List<string>();
                List<string> wZombieList = new List<string>();
                List<string> wSpawnList = new List<string>();
                List<string> KEYWORDLIST;
                KEYWORDLIST = new List<string> { "CRATE", "ZOMBIE", "PLAYER", "SPAWNER" };

                s = stream.ReadLine();
                while (s != null)
                {
                    string[] words = s.Split(' ');

                    ParseLevelInfo("CRATE", words, wCrateList);
                    ParseLevelInfo("ZOMBIE", words, wZombieList);
                    ParseLevelInfo("SPAWNER", words, wSpawnList);

                    if (words[0].Equals("PLAYER"))
                    {
                        sPlayer = words[1];
                    }
                    s = stream.ReadLine();
                }

                stream.Close();

                foreach (string vector in wCrateList)
                {
                    if (!KEYWORDLIST.Contains(vector))
                    {
                        string x, y;
                        ParseVectorCoord(vector, out x, out y);
                        if (x != null && y != null)
                        {
                            controller.crateList.Add(new Crate(tCrate, new Vector2(int.Parse(x), int.Parse(y))));
                        }
                    }
                }

                foreach (string vector in wZombieList)
                {
                    if (!KEYWORDLIST.Contains(vector))
                    {
                        string x, y;
                        ParseVectorCoord(vector, out x, out y);
                        if (x != null && y != null)
                        {
                            controller.enemyList.Add(new EnemyZombie(tZombie, new Vector2(int.Parse(x), int.Parse(y)), player, controller, tZombieAttack));
                        }
                    }
                }

                foreach (string vector in wSpawnList)
                {
                    if (!KEYWORDLIST.Contains(vector))
                    {
                        string x, y;
                        ParseVectorCoord(vector, out x, out y);
                        if (x != null && y != null)
                        {
                            controller.spawnList.Add(new Spawner(tSpawner, new Vector2(int.Parse(x), int.Parse(y))));
                        }
                    }
                }
                if (sPlayer != null)
                {
                    string[] xyPlayer = sPlayer.Split(';');
                    player.position = new Vector2(int.Parse(xyPlayer[0]), int.Parse(xyPlayer[1]));
                }
                camera = new Camera(player, graphics);
                player.camera = camera;
                player.controller = controller;
                player.tBullet = tBullet;
            }
        }

        public void LoadTextures(Texture2D tPlayer, Texture2D tCrate, Texture2D tZombie, Texture2D tSpawner, SpriteFont font)
        {
            this.tPlayer = tPlayer;
            this.tCrate = tCrate;
            this.tZombie = tZombie;
            this.tSpawner = tSpawner;
            this.font = font;
        }

        public void ParseLevelInfo(string keyword, string[] words, List<string> list)
        {
            if (words[0].Equals(keyword))
            {
                for (int i = 1; i < words.Length; i++)
                {
                    list.Add(words[i]);
                }
            }
        }

        public void ParseVectorCoord(string vector, out string x, out string y)
        {
            x = null;
            y = null;
            bool currX = true;
            for (int i = 0; i < vector.Length; i++)
            {
                if (vector[i] == ';')
                {
                    currX = false;
                    continue;
                }
                else if (currX == true)
                {
                    x += vector[i];
                }
                else
                {
                    y += vector[i];
                }
            }
        }

        public void Update()
        {
            prevState = state;
            state = Keyboard.GetState();

            //if (state.IsKeyDown(Keys.R))
            if (controller.timeTillSpawn.value == 0)
            {
                foreach (Spawner s in controller.spawnList)
                {
                    //controller.GenerateEnemyZombie(tZombie, new Rectangle(tCrate.Width, tCrate.Height, graphics.PreferredBackBufferWidth - 2 * tCrate.Width, graphics.PreferredBackBufferHeight - 2 * tCrate.Height));
                    controller.GenerateEnemyZombie(tZombie, s.spawnRect);
                }
            }

            if (state.IsKeyDown(Keys.T) && prevState.IsKeyUp(Keys.T))
            {
                drawHitbox = !drawHitbox;
            }

            player.Update();
            controller.Update();
            camera.Update();
        }

        public void ResetLevel()
        {
            controller.crateList.Clear();
            controller.enemyList.Clear();
            controller.bulletList.Clear();
            player.Reset();
            camera = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.GetTransformMatrix());

            //controller.Draw(spriteBatch, camera);
            player.Draw(spriteBatch);
            if (drawHitbox)
            {
                player.hitbox.Draw(tBullet, spriteBatch);
                foreach (Enemy e in controller.enemyList)
                {
                    if (e is EnemyZombie)
                    {
                        (e as EnemyZombie).hitbox.Draw(tBullet, spriteBatch);
                    }
                }
            }
            controller.Draw(spriteBatch, camera);
            spriteBatch.End();
        }
    }
}