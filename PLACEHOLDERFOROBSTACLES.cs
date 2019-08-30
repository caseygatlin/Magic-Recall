using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace out_and_back
{
    class PLACEHOLDERFOROBSTACLES : Entity
    {
        Texture2D sprite;
        internal PLACEHOLDERFOROBSTACLES(Game1 game, Vector2 position, float radius) : base(game, Team.Enemy, 0, 0, position, radius)
        {}
        public static PLACEHOLDERFOROBSTACLES Crystal(Game1 game, float direction, Vector2 position)
        {
            PLACEHOLDERFOROBSTACLES p = new PLACEHOLDERFOROBSTACLES(game, position, 30);
            p.sprite = AssetManager.Instance.crystalSprite;
            return p;
        }
        public static PLACEHOLDERFOROBSTACLES Tree(Game1 game, float direction, Vector2 position)
        {
            PLACEHOLDERFOROBSTACLES p = new PLACEHOLDERFOROBSTACLES(game, position, 40);
            p.sprite = AssetManager.Instance.treeSprite;
            return p;
        }
        public static PLACEHOLDERFOROBSTACLES BrazierTriple(Game1 game, float direction, Vector2 position)
        {
            PLACEHOLDERFOROBSTACLES p = new PLACEHOLDERFOROBSTACLES(game, position, 30);
            p.sprite = AssetManager.Instance.brazierTripleUnlitSprite;
            return p;
        }
        public static PLACEHOLDERFOROBSTACLES BrazierRange(Game1 game, float direction, Vector2 position)
        {
            PLACEHOLDERFOROBSTACLES p = new PLACEHOLDERFOROBSTACLES(game, position, 30);
            p.sprite = AssetManager.Instance.brazierRangeUnlitSprite;
            return p;
        }
        public override void HandleCollision(Entity other)
        {
        }
        public override void Draw(GameTime gameTime)
        {
            if (sprite != null)
                AssetManager.Instance.DrawSprite(this, sprite);
            else
                AssetManager.Instance.PrintString("[]", Position, Color.Black);

        }
    }
}
