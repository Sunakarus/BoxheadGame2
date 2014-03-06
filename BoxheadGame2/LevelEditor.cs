using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace BoxheadGame2
{
    internal class LevelEditor
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Texture2D tPlayer, tCrate, tZombie, tSpawner, currentTexture;

        public List<Vector2> crateList = new List<Vector2>();
        public List<Vector2> zombieList = new List<Vector2>();
        public List<Vector2> spawnList = new List<Vector2>();

        private Vector2 player = Vector2.Zero;
        private Vector2 tempVector, drawTempVector;
        public SpriteFont font;

        private MouseState mouse;
        private StreamWriter streamWriter;
        private bool isPressed = false, showVector = true;
        private bool reallyReset = false;

        private enum Select { Crate, Player, Zombie, Spawner };

        private Select select;
        private KeyboardState state, prevState;
        private EditorCamera camera = new EditorCamera();
        private int closest1, closest2;
        private int cameraSpeed = 8;

        //private string dirPath = @"C:\Users\HanThi\Disk Google\Dropbox\Projects\BoxheadGame2\level.txt";
        private string dirPath = @"level.txt";

        private string message = "";

        private int savedTimer = 0;

        public LevelEditor(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
        }

        public void LoadTextures(Texture2D tPlayer, Texture2D tCrate, Texture2D tZombie, Texture2D tSpawner, SpriteFont font)
        {
            this.tPlayer = tPlayer;
            this.tCrate = tCrate;
            this.tZombie = tZombie;
            this.font = font;
            this.tSpawner = tSpawner;

            select = Select.Crate;
            currentTexture = tCrate;
            state = Keyboard.GetState();
        }

        public void Reset()
        {
            reallyReset = false;
            select = Select.Crate;
            currentTexture = tCrate;
            state = Keyboard.GetState();
            isPressed = false;
            showVector = true;
            tempVector = Vector2.Zero;

            //camera = new Camera();
            player = Vector2.Zero;
            crateList.Clear();
            zombieList.Clear();
        }

        public void LoadContent()
        {
            //default
            select = Select.Crate;
            currentTexture = tCrate;
            state = Keyboard.GetState();
        }

        public void Update()
        {
            if (state.IsKeyDown(Keys.Escape))
            {
                // Game1.currentScreen = Game1.CurrentScreen.Menu;
                Reset();
            }
            prevState = state;
            state = Keyboard.GetState();

            if (savedTimer > 0)
            {
                savedTimer--;
            }
            if (state.IsKeyDown(Keys.T) && prevState.IsKeyUp(Keys.T))
            {
                showVector = !showVector;
            }
            if (state.IsKeyDown(Keys.R) && prevState.IsKeyUp(Keys.R))
            {
                reallyReset = true;
            }
            if (state.IsKeyDown(Keys.Y) && prevState.IsKeyUp(Keys.Y) && reallyReset)
            {
                Reset();
            }
            if (state.IsKeyDown(Keys.N) && prevState.IsKeyUp(Keys.N) && reallyReset)
            {
                reallyReset = false;
            }

            //select item
            if (state.IsKeyDown(Keys.D1))
            {
                select = Select.Crate;
                currentTexture = tCrate;
            }

            if (state.IsKeyDown(Keys.D2))
            {
                select = Select.Zombie;
                currentTexture = tZombie;
            }
            if (state.IsKeyDown(Keys.D3))
            {
                select = Select.Player;
                currentTexture = tPlayer;
            }
            if (state.IsKeyDown(Keys.D4))
            {
                select = Select.Spawner;
                currentTexture = tSpawner;
            }
            mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed && !isPressed)
            {
                isPressed = true;
            }

            if (isPressed)
            {
                if (state.IsKeyDown(Keys.Q))
                {
                    tempVector = new Vector2(mouse.X, tempVector.Y);
                }

                if (state.IsKeyDown(Keys.E))
                {
                    tempVector = new Vector2(tempVector.X, mouse.Y);
                }

                if (state.IsKeyDown(Keys.Space) && prevState.IsKeyUp(Keys.Space))
                {
                    tempVector.X = (int)tempVector.X;
                    tempVector.Y = (int)tempVector.Y;
                    closest1 = (int)tempVector.X + (int)camera.position.X;
                    closest2 = (int)tempVector.X + (int)camera.position.X;

                    while (closest1 % currentTexture.Width != 0)
                    {
                        closest1 -= 1;
                    }
                    while (closest2 % currentTexture.Width != 0)
                    {
                        closest2 += 1;
                    }

                    if ((int)tempVector.X + (int)camera.position.X != closest1 || (int)tempVector.X + (int)camera.position.X != closest1)
                    {
                        if ((Math.Abs(((int)tempVector.X + (int)camera.position.X) - closest1)) < (Math.Abs(((int)tempVector.X + (int)camera.position.X) - closest2)))
                        {
                            tempVector.X = closest1 - (int)camera.position.X;
                        }
                        else
                        {
                            tempVector.X = closest2 - (int)camera.position.X;
                        }
                    }

                    closest1 = (int)tempVector.Y + (int)camera.position.Y;
                    closest2 = (int)tempVector.Y + (int)camera.position.Y;

                    while (closest1 % currentTexture.Height != 0)
                    {
                        closest1 -= 1;
                    }
                    while (closest2 % currentTexture.Height != 0)
                    {
                        closest2 += 1;
                    }

                    if ((int)tempVector.Y + (int)camera.position.Y != closest1 || (int)tempVector.Y + (int)camera.position.Y != closest1)
                    {
                        if ((Math.Abs(((int)tempVector.Y + (int)camera.position.Y) - closest1)) < (Math.Abs(((int)tempVector.Y + (int)camera.position.Y) - closest2)))
                        {
                            tempVector.Y = closest1 - (int)camera.position.Y;
                        }
                        else
                        {
                            tempVector.Y = closest2 - (int)camera.position.Y;
                        }
                    }
                }

                if (state.IsKeyUp(Keys.E) && state.IsKeyUp(Keys.Q) && state.IsKeyUp(Keys.Space))
                {
                    tempVector = new Vector2(mouse.X, mouse.Y);
                }
            }

            if (mouse.LeftButton == ButtonState.Released && isPressed)
            {
                isPressed = false;
                switch (select)
                {
                    case Select.Crate:
                        {
                            crateList.Add(new Vector2(tempVector.X + camera.position.X, tempVector.Y + camera.position.Y));
                            break;
                        }
                    case Select.Zombie:
                        {
                            zombieList.Add(new Vector2(tempVector.X + camera.position.X, tempVector.Y + camera.position.Y));
                            break;
                        }
                    case Select.Player:
                        {
                            player = new Vector2(tempVector.X + camera.position.X, tempVector.Y + camera.position.Y);
                            break;
                        }
                    case Select.Spawner:
                        {
                            spawnList.Add(new Vector2(tempVector.X + camera.position.X, tempVector.Y + camera.position.Y));
                            break;
                        }
                }
            }

            if (mouse.RightButton == ButtonState.Pressed)
            {
                switch (select)
                {
                    case Select.Crate:
                        {
                            for (int i = crateList.Count - 1; i > -1; i--)
                            {
                                Rectangle tempRectangle = new Rectangle((int)crateList[i].X, (int)crateList[i].Y, tCrate.Width, tCrate.Height);
                                if (tempRectangle.Contains((int)mouse.X + (int)camera.position.X, (int)mouse.Y + (int)camera.position.Y))
                                {
                                    crateList.RemoveAt(i);
                                }
                            }
                            break;
                        }
                    case Select.Zombie:
                        {
                            for (int i = zombieList.Count - 1; i > -1; i--)
                            {
                                Rectangle tempRectangle = new Rectangle((int)zombieList[i].X, (int)zombieList[i].Y, tZombie.Width, tZombie.Height);
                                if (tempRectangle.Contains((int)mouse.X + (int)camera.position.X, (int)mouse.Y + (int)camera.position.Y))
                                {
                                    zombieList.RemoveAt(i);
                                }
                            }
                            break;
                        }
                    case Select.Player:
                        {
                            player = Vector2.Zero;
                            break;
                        }
                    case Select.Spawner:
                        {
                            for (int i = spawnList.Count - 1; i > -1; i--)
                            {
                                Rectangle tempRectangle = new Rectangle((int)spawnList[i].X, (int)spawnList[i].Y, tSpawner.Width, tSpawner.Height);
                                if (tempRectangle.Contains((int)mouse.X + (int)camera.position.X, (int)mouse.Y + (int)camera.position.Y))
                                {
                                    spawnList.RemoveAt(i);
                                }
                            }
                            break;
                        }
                }
                //tempVector = tempVector + camera.position;
            }
            if (state.IsKeyDown(Keys.P))
            {
                if (player != Vector2.Zero)
                {
                    streamWriter = new StreamWriter(dirPath, false);
                    streamWriter.Write("CRATE ");
                    foreach (Vector2 v in crateList)
                    {
                        streamWriter.Write((int)v.X + ";" + (int)v.Y + " ");
                    }
                    streamWriter.WriteLine();
                    streamWriter.Write("ZOMBIE ");
                    foreach (Vector2 v in zombieList)
                    {
                        streamWriter.Write((int)v.X + ";" + (int)v.Y + " ");
                    }
                    streamWriter.WriteLine();
                    streamWriter.Write("SPAWNER ");
                    foreach (Vector2 v in spawnList)
                    {
                        streamWriter.Write((int)v.X + ";" + (int)v.Y + " ");
                    }
                    streamWriter.WriteLine();
                    streamWriter.Write("PLAYER ");
                    streamWriter.Write((int)player.X + ";" + (int)player.Y);
                    streamWriter.Close();
                    message = "SAVED";
                }
                else
                {
                    message = "PLAYER MISSING";
                }
                savedTimer = 60;
            }

            if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
            {
                camera.position.Y -= cameraSpeed;
            }
            if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
            {
                camera.position.Y += cameraSpeed;
            }
            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
            {
                camera.position.X -= cameraSpeed;
            }
            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            {
                camera.position.X += cameraSpeed;
            }
        }

        public void Draw()
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix());

            drawTempVector = tempVector + camera.position;
            if (isPressed)
            {
                switch (select)
                {
                    case Select.Crate:
                        {
                            spriteBatch.Draw(tCrate, drawTempVector, Color.White);
                            break;
                        }
                    case Select.Zombie:
                        {
                            spriteBatch.Draw(tZombie, drawTempVector, Color.White);
                            break;
                        }
                    case Select.Player:
                        {
                            spriteBatch.Draw(tPlayer, drawTempVector, Color.White);
                            break;
                        }
                    case Select.Spawner:
                        {
                            spriteBatch.Draw(tSpawner, drawTempVector, Color.White);
                            break;
                        }
                }
                if (showVector)
                    spriteBatch.DrawString(font, drawTempVector.ToString(), drawTempVector, Color.Black);
            }
            foreach (Vector2 vector in crateList)
            {
                spriteBatch.Draw(tCrate, vector, Color.White);
                if (showVector)
                    spriteBatch.DrawString(font, vector.ToString(), vector, Color.Black);
            }
            foreach (Vector2 vector in zombieList)
            {
                spriteBatch.Draw(tZombie, vector, Color.White);
                if (showVector)
                    spriteBatch.DrawString(font, vector.ToString(), vector, Color.Black);
            }
            foreach (Vector2 vector in spawnList)
            {
                spriteBatch.Draw(tSpawner, vector, Color.White);
                if (showVector)
                    spriteBatch.DrawString(font, vector.ToString(), vector, Color.Black);
            }
            if (player != Vector2.Zero)
            {
                spriteBatch.Draw(tPlayer, player, Color.White);
                if (showVector)
                    spriteBatch.DrawString(font, player.ToString(), player, Color.Black);
            }
            spriteBatch.DrawString(font, "P: save\nQ: snap to X\nE: snap to Y\nSelected: " + select + "\nT: toggle vectors\nR: clear all\nPRESS 8 TO START THE GAME", camera.position, Color.Black);

            if (savedTimer > 0)
            {
                spriteBatch.DrawString(font, message, camera.position + new Vector2(graphics.PreferredBackBufferWidth / 2 - 20, 30), Color.Black);
            }

            if (reallyReset)
            {
                spriteBatch.DrawString(font, "Clear All? Y/N", camera.position + new Vector2(graphics.PreferredBackBufferWidth / 2 - 30, 60), Color.Black);
            }
            spriteBatch.End();
        }
    }
}