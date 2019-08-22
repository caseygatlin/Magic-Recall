using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace out_and_back
{
    class AssetManager
    {
        //The player sprite
        public Texture2D playerSprite;
        public Texture2D weaponSprite;

        //In case we need to scale anything, change targetX value to adjust scale
        public float targetX = 128;
        public float targetY;
        public Vector2 scale;


        private static AssetManager instance;
        public static AssetManager Instance
        {
            get => instance;
        }

        /// <summary>
        /// Initializes the asset manager. If the asset manager already exists,
        /// then it will return the manager.
        /// </summary>
        /// <param name="game">The game that this manager belongs to.</param>
        /// <returns>The singleton instance of the asset manager.</returns>
        public static AssetManager Initialize(Game1 game)
        {
            if (instance != null) return instance;
            instance = new AssetManager(game);
            return instance;
            
        }

        private AssetManager(Game1 game)
        {
            Font = game.Content.Load<SpriteFont>("defaultFont");
            batch = game.spriteBatch;

            //Loads up the player sprite
            playerSprite = game.Content.Load<Texture2D>("Wizard");

            //In case we need to scale anything, use scale variable
            scale = new Vector2(targetX / (float)playerSprite.Width, targetX / (float)playerSprite.Width);
            targetY = playerSprite.Height * scale.Y;
        }

        private SpriteFont Font;
        private SpriteBatch batch;

        public void PrintString(string str, Vector2 position, Color color, float rotation = 0)
        {
            batch.DrawString(
                Font,
                str,
                position,
                color);
        }

        
        //Draws the loaded character sprite to the screen
        public void DrawCharSprite(Vector2 playerPos)
        {
            batch.Draw(playerSprite, playerPos, Color.White);
        }


        //Draws the loaded weapon sprite to the screen
        // TODO: Load the weapon sprite in The AssetManager function
        public void DrawCharWeapon(Vector2 projectilePos)
        {
            batch.Draw(weaponSprite, projectilePos, Color.White);
        }

    }
}
