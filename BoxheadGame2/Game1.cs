#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

#endregion Using Statements

namespace BoxheadGame2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D tPlayer, tCrate, tBullet, tZombie, tZombieAttack, tSpawner;
        private SpriteFont font;
        private Player player;
        private Controller controller;
        public static Random random = new Random();
        private LevelEditor levelEditor;
        private Gameplay gamePlay;

        private KeyboardState state, prevState;

        private enum Screen { Menu, Editor, Game }

        private static Screen screen = Screen.Editor;

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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        ///
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tBullet = Content.Load<Texture2D>("bullet");
            tPlayer = Content.Load<Texture2D>("spr_Player");
            tCrate = Content.Load<Texture2D>("crate");
            font = Content.Load<SpriteFont>("tahoma");
            tZombie = Content.Load<Texture2D>("Zombie");
            tZombieAttack = Content.Load<Texture2D>("Zombie_attack");
            tSpawner = Content.Load<Texture2D>("spawn");

            player = new Player(tPlayer, new Vector2(200, 100));
            controller = new Controller(player);
            LoadControllerContent();
            gamePlay = new Gameplay(graphics, controller, player);
            gamePlay.LoadTextures(tPlayer, tCrate, tZombie, tSpawner, font);
            LoadGameplayContent();
            /*
            camera = new Camera(player, graphics);
            player.camera = camera;
            player.controller = controller;
            player.tBullet = tBullet;*/

            levelEditor = new LevelEditor(graphics, spriteBatch);
            levelEditor.LoadTextures(tPlayer, tCrate, tZombie, tSpawner, font);

            state = Keyboard.GetState();
        }

        public void LoadControllerContent()
        {
            controller.tBullet = tBullet;
            controller.tPlayer = tPlayer;
            controller.tCrate = tCrate;
            controller.font = font;
            controller.tZombie = tZombie;
            controller.tZombieAttack = tZombieAttack;
        }

        public void LoadGameplayContent()
        {
            gamePlay.tBullet = tBullet;
            gamePlay.tPlayer = tPlayer;
            gamePlay.tCrate = tCrate;
            gamePlay.font = font;
            gamePlay.tZombie = tZombie;
            gamePlay.tZombieAttack = tZombieAttack;
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

            if (state.IsKeyDown(Keys.D8) && prevState.IsKeyUp(Keys.D8))
            {
                gamePlay.ResetLevel();
                gamePlay.LoadTextures(tPlayer, tCrate, tZombie, tSpawner, font);
                gamePlay.LoadLevel();
                screen = Screen.Game;
            }

            if (state.IsKeyDown(Keys.D9) && prevState.IsKeyUp(Keys.D9))
            {
                levelEditor.Reset();
                screen = Screen.Editor;
            }

            switch (screen)
            {
                case Screen.Game:
                    {
                        gamePlay.Update();
                        break;
                    }
                case Screen.Editor:
                    {
                        levelEditor.Update();
                        break;
                    }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (screen)
            {
                case Screen.Game:
                    {
                        gamePlay.Draw(spriteBatch);
                        break;
                    }
                case Screen.Editor:
                    {
                        levelEditor.Draw();
                        break;
                    }
            }
            base.Draw(gameTime);
        }
    }
}