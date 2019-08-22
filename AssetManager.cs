using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace out_and_back
{
    class AssetManager
    {
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
        }

        private SpriteFont Font;
        private SpriteBatch batch;

        public void PrintString(string str, Vector2 position, Color color, float rotation = 0)
        {
            batch.DrawString(
                Font,
                str,
                position,
                color);
        }
    }
}
