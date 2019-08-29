using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace out_and_back.GameStates
{
    class GameOverState : AbstractGameState
    {
        AssetManager assetManager;
        Game1 game;

        public GameOverState(Game1 currentGame)
        {
            game = currentGame;
            assetManager = AssetManager.Instance;
        }

        public override void Draw(Game1 game, GameTime gameTime)
        {
            game.spriteBatch.Draw(AssetManager.Instance.background, Vector2.Zero, Color.White);
            assetManager.PrintStringCenter("Game Over", new Vector2(Globals.SCREEN_WIDTH / 2, Globals.SCREEN_HEIGHT / 3), Color.White, assetManager.retroFontLarge);
            assetManager.PrintStringCenter("Press space to restart", new Vector2(Globals.SCREEN_WIDTH / 2, Globals.SCREEN_HEIGHT * 3 / 4), Color.White, assetManager.retroFontSmall);
        }

        public override void Update(Game1 game, GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                game.paused = false;
                game.state = new GameStates.InLevelState(game);
            }
        }
    }
}
