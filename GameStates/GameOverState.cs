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
            assetManager.DrawSprite(Vector2.Zero, AssetManager.Instance.background);

            if (!game.wonGame)
                assetManager.DrawIcon(assetManager.gameOverIcon, new Vector2(Globals.SCREEN_WIDTH / 2, Globals.SCREEN_HEIGHT / 2), 0.1f);
            else
                assetManager.PrintStringCenter("Enemies Defeated: ALL OF THEM", new Vector2(Globals.SCREEN_WIDTH / 2, Globals.SCREEN_HEIGHT / 3), Color.White, assetManager.retroFontLarge);

            assetManager.PrintStringCenter("Press Space to restart", new Vector2(Globals.SCREEN_WIDTH / 2, Globals.SCREEN_HEIGHT * 3 / 4), Color.White, assetManager.retroFontSmall);
            assetManager.DrawDarkOverlay();
        }

        public override void Update(Game1 game, GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                game.Entities.ClearAll();
                bool previously_won = game.wonGame;
                game.wonGame = false;
                game.paused = false;
                game.paused_nonPlyr = false;
                game.state = new GameStates.InLevelState(game, (previously_won ? Level.Level2(game) : Level.Level1(game)));
            }
        }
    }
}
