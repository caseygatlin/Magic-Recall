using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace out_and_back
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        internal GameStates.AbstractGameState state;

        GraphicsDeviceManager graphics;
        internal SpriteBatch spriteBatch;
        internal EnitityManager Entities;
        internal bool wonGame = false;
        private bool pauseKeyPressed = false;
        //Song song;

        public Vector2 Scale = Vector2.One;
        
        public bool paused
        {
            get;
            internal set;
        }

        internal Player Player
        {
            get;
            private set;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 450;
            graphics.PreferredBackBufferWidth = 800;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            paused = false;
            //song = Content.Load<Song>("Music");
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary> 
        protected override void Initialize()
        {
            Entities = new EnitityManager(this);
            Entity.DefaultRemovalEvent += EntityRemoved;
            base.Initialize(); 
        }

        private void EntityRemoved(object sender, System.EventArgs e)
        {
            Entities.RemoveEntity((Entity)sender);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            state = new GameStates.StartMenuState(this);
            //MediaPlayer.Play(song);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            if (Keyboard.GetState().IsKeyDown(Keys.F1) && !pauseKeyPressed)
            {
                pauseKeyPressed = true;
                paused = !paused;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.F1))
            {
                pauseKeyPressed = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F11))
            {
                graphics.ToggleFullScreen();
                if (graphics.IsFullScreen)
                {
                    graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                }
                else
                {
                    graphics.PreferredBackBufferHeight = Globals.SCREEN_HEIGHT;
                    graphics.PreferredBackBufferWidth = Globals.SCREEN_WIDTH;
                }
                graphics.ApplyChanges();
                Scale = new Vector2(graphics.PreferredBackBufferWidth / Globals.SCREEN_WIDTH, graphics.PreferredBackBufferHeight / Globals.SCREEN_HEIGHT);
            }

            state.Update(this, gameTime);
            Player = state.Player; //I couldn't figure out a better way to work through this
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.Target , Color.Black, .5f, 1);            
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale(Scale.X, Scale.Y, 1));
            state.Draw(this, gameTime);
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
