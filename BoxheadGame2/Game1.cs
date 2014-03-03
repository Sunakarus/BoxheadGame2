#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace BoxheadGame2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D tPlayer, tCrate, tBullet, tZombie;
        SpriteFont font;
        Player player;
        Controller controller;
        public static Random random = new Random();

        KeyboardState state, prevState;
        bool drawHitbox = true;

        //Texture2D tZombieAttack;
        //Animation ZombieAnim;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tBullet = Content.Load<Texture2D>("bullet");
            tPlayer = Content.Load<Texture2D>("spr_Player");
            tCrate = Content.Load<Texture2D>("crate");
            font = Content.Load<SpriteFont>("tahoma");
            tZombie = Content.Load<Texture2D>("Zombie");
            //tZombieAttack = Content.Load<Texture2D>("Zombie_attack");
            
            player = new Player(tPlayer, new Vector2(200, 100));
            controller = new Controller(player);
            player.controller = controller;
            player.tBullet = tBullet;

            //ZombieAnim = new Animation(ZombieAttack, 8, 4, 2, new Vector2(100,100), true, new Timer(10));

            for (int i = 0; i<15; i++)
            {
                controller.crateList.Add(new Crate(tCrate, new Vector2(i * tCrate.Width, 0)));
                controller.crateList.Add(new Crate(tCrate, new Vector2(i * tCrate.Width, graphics.PreferredBackBufferHeight - tCrate.Height)));
                controller.crateList.Add(new Crate(tCrate, new Vector2(0, i * tCrate.Height)));
                controller.crateList.Add(new Crate(tCrate, new Vector2(graphics.PreferredBackBufferWidth - tCrate.Width, i* tCrate.Height)));
               
            }

            state = Keyboard.GetState();
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevState = state;
            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.R))
            {
                /*EnemyZombie a = new EnemyZombie(tZombie, new Vector2(random.Next(graphics.PreferredBackBufferWidth), random.Next(graphics.PreferredBackBufferHeight)), player, controller);
                if (!a.controller.CollidesWithWall(a.hitbox) && !a.controller.CollidesWithPlayer(a.hitbox) && !a.GetEnemyCollision(a.hitbox, Vector2.Zero))
                {
                    controller.enemyList.Add(a);
                }*/
                controller.GenerateEnemyZombie(tZombie, new Rectangle(tCrate.Width, tCrate.Height, graphics.PreferredBackBufferWidth - 2 * tCrate.Width, graphics.PreferredBackBufferHeight - 2 * tCrate.Height));
            }

            if (state.IsKeyDown(Keys.T) && prevState.IsKeyUp(Keys.T))
            {
                drawHitbox = !drawHitbox;
            }

            player.Update();
            controller.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            controller.Draw(spriteBatch);
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
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
