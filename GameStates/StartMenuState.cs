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

        public StartMenuState(Game1 game)
        {
            AssetManager.Initialize(game);
            assetManager = AssetManager.Instance;
            titleIcon = assetManager.titleIcon;
        }
        public override void Draw(Game1 game, GameTime gameTime)
        {
            Vector2 titlePos = new Vector2(Globals.SCREEN_WIDTH / 2, Globals.SCREEN_HEIGHT / 2);
            if (titleIcon != null)
                assetManager.DrawIcon(titleIcon, titlePos);
            else
                assetManager.PrintString("Magic Recall", titlePos, Color.White);

            assetManager.PrintStringCenter("Press Space", new Vector2(Globals.SCREEN_WIDTH / 5, Globals.SCREEN_HEIGHT * 5 / 8), Color.Wheat ,assetManager.retroFontLarge);
            assetManager.PrintStringCenter("to Start", new Vector2(Globals.SCREEN_WIDTH * 4 / 5, Globals.SCREEN_HEIGHT * 5 / 8), Color.Wheat, assetManager.retroFontLarge);
        }

        public override void Update(Game1 game, GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                game.state = new GameStates.InLevelState(game);
            }
        }
    }
}
