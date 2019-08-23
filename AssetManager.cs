using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace out_and_back
{
    class AssetManager
    {
        //The player and weapon sprites
        public Texture2D playerSprite;
        public Texture2D weaponSprite;
        public Texture2D ghostSprite;

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

            //Loads up the sprites
            playerSprite = game.Content.Load<Texture2D>("Wizard");
            weaponSprite = game.Content.Load<Texture2D>("FireAttack(Forward)");
            ghostSprite = game.Content.Load<Texture2D>("Ghost");

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

        
        //Draws the loaded sprites to the screen
        public void DrawCharSprite(Vector2 playerPos)
        {
            //Centers the character sprite around the player position
            float spritePosX = playerPos.X - (float)playerSprite.Width / 2;
            float spritePosY = playerPos.Y - (float)playerSprite.Height /2;
            Vector2 spritePos = new Vector2(spritePosX, spritePosY);

            //Draws the player sprite
            batch.Draw(playerSprite, spritePos, Color.White);
        }


        //Draws the loaded weapon sprite to the screen
        public void DrawCharWeapon(Vector2 projectilePos, float projectileDir)
        {
            //Centers the rotation position of the weapon sprite
            Vector2 spriteOrigin = new Vector2((float)weaponSprite.Width / 2, (float)weaponSprite.Height / 2);

            //Adjusts for the orientation of the sprite png
            float weaponDir = projectileDir + Globals.PI / 2 - .2f;

            //The rectangle that the sprite is drawn within
            Rectangle destRect = new Rectangle((int)projectilePos.X, (int)projectilePos.Y, (int)weaponSprite.Width, (int)weaponSprite.Height);

            //Draws the sprite
            batch.Draw(weaponSprite, destRect, null, Color.White, weaponDir, origin: spriteOrigin, effects: SpriteEffects.None, layerDepth: 0f);
        }

        public void DrawGhost(Vector2 ghostPos)
        {
            //Centers the rotation position of the ghost
            Vector2 spriteOrigin = new Vector2((float)ghostSprite.Width / 2, (float)ghostSprite.Height / 2);
            
            //The rectangle that the sprite is drawn within
            Rectangle destRect = new Rectangle((int)ghostPos.X, (int)ghostPos.Y, (int)ghostSprite.Width, (int)ghostSprite.Height);

            //Draws the sprite
            batch.Draw(ghostSprite, destRect, null, Color.White, 0, origin: spriteOrigin, effects: SpriteEffects.None, layerDepth: 0f);
        }

    }
}
