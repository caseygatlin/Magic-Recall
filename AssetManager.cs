﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace out_and_back
{
    class AssetManager
    {
        //The player and weapon sprites
        public Texture2D playerSprite;
        public Texture2D weaponSprite;
        public Texture2D ghostSprite;
        public Texture2D ghostAtkSprite;
        public Texture2D slimeSprite;

        //Background sprite
        public Texture2D background;
        public Texture2D backgroundSmaller;

        //Start menu sprites
        public Texture2D titleIcon;

        //Base sprite for simple rectangles (nice for UI)
        public Texture2D uiRect;

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
            ghostAtkSprite = game.Content.Load<Texture2D>("GhostAttack");
            slimeSprite = game.Content.Load<Texture2D>("Slime");
            background = game.Content.Load<Texture2D>("BG2");
            backgroundSmaller = game.Content.Load<Texture2D>("BG");
            titleIcon = game.Content.Load<Texture2D>("Title");
            uiRect = new Texture2D(game.GraphicsDevice, 1, 1);
            uiRect.SetData(new Color[] { Color.White });

            //In case we need to scale anything, use scale variable
            scale = new Vector2(targetX / (float)playerSprite.Width, targetX / (float)playerSprite.Width);
            targetY = playerSprite.Height * scale.Y;
        }

        private SpriteFont Font;
        private SpriteBatch batch;

        public void PrintString(string str, Vector2 position, Color color)
        {
            batch.DrawString(Font, str, position, color);
        }


        //Draws the loaded sprites to the screen
        public void DrawSprite(Entity parent, Texture2D sprite, float rotationCorr = 0f)
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
            batch.Draw(sprite, destRect, null, Color.White, spriteDir, origin: spriteOrigin, effects: SpriteEffects.None, layerDepth: 0f);
        }

        //Draws a stationary sprite to the screen at a position
        public void DrawIcon(Texture2D icon, Vector2 position)
        {
            Vector2 iconOrigin = new Vector2((float)icon.Width / 2, (float)icon.Height / 2);
            Rectangle destRect = new Rectangle((int)position.X, (int)position.Y, (int)icon.Width, (int)icon.Height);
            batch.Draw(icon, destRect, null, Color.White, 0, origin: iconOrigin, effects: SpriteEffects.None, layerDepth: 0f);
        }

        //Draws a rectangle of a given size and color; nice for UI.
        public void DrawRectangle(Rectangle rect, Color color)
        {
            batch.Draw(uiRect, rect, color);
        }
    }
}
