using System;
using Microsoft.Xna.Framework;

namespace out_and_back
{
    class Brazier : Obstacle
    {
        private bool isLit = false;

        internal Brazier(Game1 game, Vector2 position, float radius) : base(game, position, radius)
        {
        }

        public static Brazier BrazierTriple(Game1 game, float direction, Vector2 position)
        {
            Brazier p = new Brazier(game, position, Globals.OBSTACLE_RADIUS);
            p.sprite = AssetManager.Instance.brazierTripleUnlitSprite;
            return p;
        }
        public static Obstacle BrazierRange(Game1 game, float direction, Vector2 position)
        {
            Brazier p = new Brazier(game, position, Globals.OBSTACLE_RADIUS);
            p.sprite = AssetManager.Instance.brazierRangeUnlitSprite;
            return p;
        }

        //To change the state of isLit for the Brazier
        public override void Update(GameTime gameTime)
        {
        }



    }
}
