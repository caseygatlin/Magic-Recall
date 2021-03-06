﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace out_and_back
{
    class AssetManager
    {
        //The player and weapon sprites
        public Texture2D playerSprite;
        public Texture2D playerInvincibleSprite;
        public Texture2D playerHealthIconSprite;
        public Texture2D weaponSprite;
        public Texture2D ghostSprite;
        public Texture2D ghostAtkSprite;
        public Texture2D slimeSprite;
        public Texture2D eyeSprite;
        public Texture2D eyeAtkSprite;
        public Texture2D spiderSprite;

        // Cursor images (in Content/Cursors)
        public Texture2D StaffFullAndInRange;
        public Texture2D StaffFullAndOutRange;
        public Texture2D StaffEmpty;

        //Background/Environment sprites
        public Texture2D background;
        public Texture2D backgroundSmaller;

        //Terrain sprites
        public Texture2D crystalSprite;
        public Texture2D treeSprite;
        public Texture2D brazierTripleLitSprite;
        public Texture2D brazierRangeLitSprite;
        public Texture2D brazierTripleUnlitSprite;
        public Texture2D brazierRangeUnlitSprite;

        //Start menu sprites
        public Texture2D titleIcon;

        //Game Over sprite
        public Texture2D gameOverIcon;

        //Base sprite for simple rectangles (nice for UI)
        public Texture2D uiRect;

        //Base sprite for slightly transparent rectangles
        public Texture2D uiOverlay;

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
            defualtFont = game.Content.Load<SpriteFont>("defaultFont");
            retroFontSmall = game.Content.Load<SpriteFont>("RetroFontSmall");
            retroFontLarge = game.Content.Load<SpriteFont>("RetroFontLarge");
            batch = game.spriteBatch;

            /*----------- Loads up the sprites ----------*/
            //Player sprites
            playerSprite = game.Content.Load<Texture2D>("Wizard");
            playerInvincibleSprite = game.Content.Load<Texture2D>("WizardInvincible");
            playerHealthIconSprite = game.Content.Load<Texture2D>("WizardHealth");

            //Enemy sprites
            ghostSprite = game.Content.Load<Texture2D>("Ghost");
            slimeSprite = game.Content.Load<Texture2D>("Slime");
            eyeSprite = game.Content.Load<Texture2D>("Eyeball");
            spiderSprite = game.Content.Load<Texture2D>("Spider");

            //Projectile attacks
            ghostAtkSprite = game.Content.Load<Texture2D>("GhostAttack");
            eyeAtkSprite = game.Content.Load<Texture2D>("EyeBlast");
            weaponSprite = game.Content.Load<Texture2D>("FireAttack(Forward)");

            //Background sprites
            background = game.Content.Load<Texture2D>("BG16-9");
            backgroundSmaller = game.Content.Load<Texture2D>("BG");

            //Terrain sprites
            crystalSprite = game.Content.Load<Texture2D>("Crystal");
            treeSprite = game.Content.Load<Texture2D>("Tree");
            brazierTripleLitSprite = game.Content.Load<Texture2D>("BrazierTripleLit");
            brazierRangeLitSprite = game.Content.Load<Texture2D>("BrazierRangeLit");
            brazierTripleUnlitSprite = game.Content.Load<Texture2D>("BrazierTripleUnlit");
            brazierRangeUnlitSprite = game.Content.Load<Texture2D>("BrazierRangeUnlit");

            // Cursor sprites
            game.Content.RootDirectory = "Content/Cursors";
            StaffEmpty = game.Content.Load<Texture2D>("empty");
            StaffFullAndInRange = game.Content.Load<Texture2D>("full_in_range");
            StaffFullAndOutRange = game.Content.Load<Texture2D>("full_out_range");

            //UI
            game.Content.RootDirectory = "Content";
            titleIcon = game.Content.Load<Texture2D>("Title");
            gameOverIcon = game.Content.Load<Texture2D>("GameOver");
            uiRect = new Texture2D(game.GraphicsDevice, 1, 1);
            uiRect.SetData(new Color[] { Color.White });
            uiOverlay = game.Content.Load<Texture2D>("DarkOverlay");
            /*------------------------------------------*/

            //In case we need to scale anything, use scale variable
            scale = new Vector2(targetX / playerSprite.Width, targetX / playerSprite.Width);
            targetY = playerSprite.Height * scale.Y;
        }

        public SpriteFont defualtFont;
        public SpriteFont retroFontSmall;
        public SpriteFont retroFontLarge;
        private SpriteBatch batch;

        public void PrintString(string str, Vector2 position, Color color)
        {
            //batch.DrawString(retroFontSmall, str, position, color);
            batch.DrawString(retroFontSmall, str, position, color, 0f, Vector2.Zero, 1f, effects: SpriteEffects.None, layerDepth: .1f);
        }

        public void PrintStringCenter(string str, Vector2 position, Color color, SpriteFont font)
        {
            Vector2 stringSize = font.MeasureString(str);
            Vector2 centerPos = new Vector2(position.X - stringSize.X / 2, position.Y - stringSize.Y / 2);
            batch.DrawString(font, str, centerPos, color, 0, Vector2.Zero, 1f, effects: SpriteEffects.None, layerDepth: .1f);
        }

        //Draws the loaded sprites to the screen
        public void DrawSprite(Entity parent, Texture2D sprite, float rotationCorr = 0f, float depth = .3f)
        {
            //If the sprite needs rotation correction, apply it, otherwise set sprite rotation to zero
            float spriteDir = 0f;
            if (rotationCorr != 0)
                spriteDir = parent.Direction + rotationCorr;

            //Set the pivot around where the sprite rotates
            Vector2 spriteOrigin = new Vector2((float)sprite.Width / 2, (float)sprite.Height / 2);

            //Sets the location where the sprite is drawn, relative to the entity's position
            Rectangle destRect = new Rectangle((int)parent.Position.X, (int)parent.Position.Y, (int)sprite.Width, (int)sprite.Height);

            //Draws the sprite
            batch.Draw(sprite, destRect, null, Color.White, spriteDir, origin: spriteOrigin, effects: SpriteEffects.None, layerDepth: depth);
        }
        public void DrawSprite(Vector2 position, Texture2D sprite, float depth = .3f)
        {
            batch.Draw(sprite, position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth);
        }

        //Draws a stationary sprite to the screen at a position
        public void DrawIcon(Texture2D icon, Vector2 position, float depth = .3f)
        {
            Vector2 iconOrigin = new Vector2((float)icon.Width / 2, (float)icon.Height / 2);
            Rectangle destRect = new Rectangle((int)position.X, (int)position.Y, icon.Width, icon.Height);
            batch.Draw(icon, destRect, null, Color.White, 0, origin: iconOrigin, effects: SpriteEffects.None, layerDepth: depth);
        }

        //Draws a rectangle of a given size and color; nice for UI.
        public void DrawRectangle(Rectangle rect, Color color)
        {
            batch.Draw(uiRect, rect, null, color, 0, Vector2.Zero, SpriteEffects.None, .2f );
        }

        public void DrawDarkOverlay()
        {
            Rectangle rect = new Rectangle(0, 0, Globals.SCREEN_WIDTH, Globals.SCREEN_HEIGHT);
            batch.Draw(uiOverlay, rect, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, .2f );
        }
    }
}
