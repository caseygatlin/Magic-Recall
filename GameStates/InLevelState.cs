using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace out_and_back.GameStates
{

    class InLevelState : AbstractGameState
    {
        public Level level;
        public Player player;
        private Game1 game;

        public InLevelState(Game1 currentGame)
        {
            game = currentGame;
            player = new Player(game, 0, new Vector2(250, 250));
            level = Level.Level1(game);
        }

        public override Vector2 getPlayerPos()
        {
            return player.Position;
        }

        public override void Draw(Game1 game, GameTime gameTime)
        {
            game.spriteBatch.Draw(AssetManager.Instance.background, Vector2.Zero, Color.White);
            level.Draw(gameTime);
        }


        public override void Update(Game1 game, GameTime gameTime)
        {
            level.Update(gameTime);
        }

        

    }
    
}
