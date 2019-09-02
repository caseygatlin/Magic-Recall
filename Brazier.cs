using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace out_and_back
{
    class Brazier : Obstacle
    {
        private enum Type
        {
            Range,
            Bonus
        }

        private bool isLit = false;
        private Texture2D litSprite;
        private Type type;

        internal Brazier(Game1 game, Vector2 position, float radius) : base(game, position, radius)
        {
        }

        public static Brazier BrazierTriple(Game1 game, float direction, Vector2 position)
        {
            Brazier p = new Brazier(game, position, Globals.OBSTACLE_RADIUS);
            p.sprite = AssetManager.Instance.brazierTripleUnlitSprite;
            p.litSprite = AssetManager.Instance.brazierTripleLitSprite;
            p.type = Type.Bonus;
            return p;
        }
        public static Obstacle BrazierRange(Game1 game, float direction, Vector2 position)
        {
            Brazier p = new Brazier(game, position, Globals.OBSTACLE_RADIUS);
            p.sprite = AssetManager.Instance.brazierRangeUnlitSprite;
            p.litSprite = AssetManager.Instance.brazierRangeLitSprite;
            p.type = Type.Range;
            return p;
        }

        //To change the state of isLit for the Brazier
        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            if (sprite != null)
                AssetManager.Instance.DrawSprite(this, isLit ? litSprite : sprite, depth: .8f);
            else
                AssetManager.Instance.PrintString("^_^", Position, Team == Team.Enemy ? Color.Red : Color.Blue);
        }

        public override void HandleCollision(Entity other)
        {
            base.HandleCollision(other);
            if(other.Team == Team.Player && other is Projectile weapon)
            {
                isLit = true;
                HandleToggle();
            }
            if (other is Enemy && ((Enemy)other).CanExtinguishBraziers)
            {
                isLit = false;
                HandleToggle();
            }
        }

        private void HandleToggle()
        {
            Player p = ((Game1)Game).Player;
            if (p == null) return;
            switch (type)
            {
                case Type.Range:
                    if (p.RangeBrazierLit != isLit)
                        p.ToggleRangeBrazierLit();
                    break;
                case Type.Bonus:
                    if (p.TripleBrazierLit != isLit)
                        p.ToggleTripleBrazierLit();
                    break;
                default:
                    throw new System.NotImplementedException($"Brazier type {type} has not been implemented.");
            }
        }
    }
}
