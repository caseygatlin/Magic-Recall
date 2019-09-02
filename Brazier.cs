using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace out_and_back
{
    class Brazier : Obstacle
    {
        private bool isLit = false;
        private Texture2D litSprite;

        internal Brazier(Game1 game, Vector2 position, float radius) : base(game, position, radius)
        {
        }

        public static Brazier BrazierTriple(Game1 game, float direction, Vector2 position)
        {
            Brazier p = new Brazier(game, position, Globals.OBSTACLE_RADIUS);
            p.sprite = AssetManager.Instance.brazierTripleUnlitSprite;
            p.litSprite = AssetManager.Instance.brazierTripleLitSprite;
            p.AbsorbsPlayerProjectiles = true;
            return p;
        }
        public static Obstacle BrazierRange(Game1 game, float direction, Vector2 position)
        {
            Brazier p = new Brazier(game, position, Globals.OBSTACLE_RADIUS);
            p.sprite = AssetManager.Instance.brazierRangeUnlitSprite;
            p.litSprite = AssetManager.Instance.brazierRangeLitSprite;
            p.AbsorbsPlayerProjectiles = true;
            return p;
        }

        //To change the state of isLit for the Brazier
        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            if (sprite != null)
                AssetManager.Instance.DrawSprite(this, (isLit ? litSprite : sprite), depth: .8f);
            else
                AssetManager.Instance.PrintString("^_^", Position, Team == Team.Enemy ? Color.Red : Color.Blue);
        }

        public override void HandleCollision(Entity other)
        {
            base.HandleCollision(other);
            if(other.Team == Team.Player && other is Projectile)
            {
                isLit = true;
            }
            if (other is Enemy && ((Enemy)other).CanExtinguishBraziers)
            {
                isLit = false;
            }
        }
    }
}
