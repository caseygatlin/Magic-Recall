using Microsoft.Xna.Framework;

namespace out_and_back.GameStates
{
    abstract class AbstractGameState
    {
        public abstract void Draw(Game1 game, GameTime gameTime);

        public abstract void Update(Game1 game, GameTime gameTime);

    }
}
