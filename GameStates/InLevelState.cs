#define RUN_LEVEL

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace out_and_back.GameStates
{

    class InLevelState : AbstractGameState
    {
        public Level level;
        private Game1 game;

        public InLevelState(Game1 currentGame)
        {
            game = currentGame;
            Player = new Player(game, 0, new Vector2(250, 250));
#if RUN_LEVEL
            level = Level.Level1(game);
#endif
        }

        public override void Draw(Game1 game, GameTime gameTime)
        {
            game.spriteBatch.Draw(AssetManager.Instance.background, Vector2.Zero, Color.White);
#if RUN_LEVEL
            level.Draw(gameTime);
#endif
        }


        public override void Update(Game1 game, GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                game.state = new GameStates.InLevelState(game);
                return;
            }
#if RUN_LEVEL
            level.Update(gameTime);
#endif
        }

        

    }
    
}
