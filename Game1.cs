using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
        internal bool playMusic = false;

        //For tracking key presses
        private bool pauseKeyPressed = false;
        private bool muteKeyPressed = false;

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
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = Globals.SCREEN_HEIGHT,
                PreferredBackBufferWidth = Globals.SCREEN_WIDTH,
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            paused = false;
            Window.Title = "Magic Recall";
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
            AssetManager.Initialize(this);
            Mouse.SetCursor(MouseCursor.FromTexture2D(AssetManager.Instance.StaffFullAndOutRange, 0, 0));
            state = new GameStates.StartMenuState(this);
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
            //Exits the application at any time
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            /*---------- Pausing -----------*/
            //Tracks the F11 key so it doesn't pause and unpause repeatedly
            if (Keyboard.GetState().IsKeyUp(Keys.F1))
                pauseKeyPressed = false;

            //Pauses the game
            if (Keyboard.GetState().IsKeyDown(Keys.F1) && !pauseKeyPressed)
            {
                pauseKeyPressed = true;
                paused = !paused;
            }
            /*------------------------------*/



            /*---------- Music -----------*/
            //Tracks the M key so it doesn't mute and unmute repeatedly
            if (Keyboard.GetState().IsKeyUp(Keys.M))
                muteKeyPressed = false;

            if (Keyboard.GetState().IsKeyDown(Keys.M) && !muteKeyPressed)
            {
                playMusic = !playMusic;
                muteKeyPressed = true;
            }
            /*----------------------------*/



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
                Scale = new Vector2((float)graphics.PreferredBackBufferWidth / Globals.SCREEN_WIDTH, (float)graphics.PreferredBackBufferHeight / Globals.SCREEN_HEIGHT);
            }

            state.Update(this, gameTime);
            if (state.Player != Player)
            {
                Player = state.Player; //I couldn't figure out a better way to work through this
                Player.AttackStateChanged += Player_AttackStateChanged;
            }

            base.Update(gameTime);
        }

        private void Player_AttackStateChanged(object sender, System.EventArgs e)
        {
            var ascea = e as AttackStateChangeEventArgs;
            MouseCursor cursor;
            AssetManager am = AssetManager.Instance;
            switch (ascea.AttackState)
            {
                case AttackState.AbleInRange:
                    cursor = MouseCursor.FromTexture2D(am.StaffFullAndInRange, 0, 0);
                    break;
                case AttackState.AbleOutOfRange:
                    cursor = MouseCursor.FromTexture2D(am.StaffFullAndOutRange, 0, 0);
                    break;
                case AttackState.Unable:
                    cursor = MouseCursor.FromTexture2D(am.StaffEmpty, 0, 0);
                    break;
                default:
                    throw new System.NotImplementedException($"{ascea.AttackState} is not implemented");
            }

            Mouse.SetCursor(cursor);
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
