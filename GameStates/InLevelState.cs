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
            AssetManager.Instance.DrawSprite(Vector2.Zero, AssetManager.Instance.background);
#if RUN_LEVEL
            level.Draw(gameTime);
#endif
        }


        public override void Update(Game1 game, GameTime gameTime)
        {
            if (Player.health <= 0)
            {
                game.paused = true;
                game.state = new GameStates.GameOverState(game);
            }
            else if (level.EnemiesLeft() <= 0)
            {
                game.wonGame = true;
                game.paused = true;
                game.state = new GameStates.GameOverState(game);
            }

#if RUN_LEVEL
            if (!game.paused)
                level.Update(gameTime);
#endif
        }

        

    }
    
}
