using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace out_and_back.GameStates
{
    class StartMenuState : AbstractGameState
    {
        AssetManager assetManager;
        Texture2D titleIcon;

        public StartMenuState()
        {
            assetManager = AssetManager.Instance;
            titleIcon = assetManager.titleIcon;
        }
        public override void Draw(Game1 game, GameTime gameTime)
        {
            Vector2 titlePos = new Vector2(Globals.SCREEN_WIDTH / 2, Globals.SCREEN_HEIGHT * 1 / 3);
            if (titleIcon != null)
                assetManager.DrawIcon(titleIcon, titlePos);
            else
                assetManager.PrintString("Magic Recall", titlePos, Color.White);
        }

        public override void Update(Game1 game, GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                
            }
        }
    }
}
