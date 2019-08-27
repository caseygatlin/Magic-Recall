#define RUN_LEVEL

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
        GraphicsDeviceManager graphics;
        internal SpriteBatch spriteBatch;

        internal EnitityManager Entities;
#if RUN_LEVEL
        Level level;
#endif
        Player player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Entities = new EnitityManager(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary> 
        protected override void Initialize()
        {
            Entity.DefaultRemovalEvent = EntityRemoved;
            player = new Player(this, 0, new Vector2(250, 250));
#if RUN_LEVEL
            level = Level.Level1(this);
#endif
            base.Initialize();
        }

        public Vector2 getPlayerPos()
        {
            return player.Position;
        }

        /// <summary>
        /// Removes the entity from the game.
        /// </summary>
        /// <param name="sender">The entity that should be removed from the game. Should be of the Entity class.</param>
        /// <param name="e">The event arguments that talk about the removal event.</param>
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

            // TODO: Add your update logic here
#if RUN_LEVEL
            level.Update(gameTime);
#endif
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSalmon);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
#if RUN_LEVEL
            level.Draw(gameTime);
#endif
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
