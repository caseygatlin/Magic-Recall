using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace out_and_back
{
    class Obstacle : Entity
    {
        Vector2 playerPos;
        public bool AbsorbsPlayerProjectiles { get; protected set; }

        protected Texture2D sprite;
        internal Obstacle(Game1 game, Vector2 position, float radius) : base(game, Team.Enemy, 0, 0, position, radius)
        {
            AbsorbsPlayerProjectiles = false;
        }

        public static Obstacle Crystal(Game1 game, float direction, Vector2 position)
        {
            Obstacle p = new Obstacle(game, position, Globals.OBSTACLE_RADIUS);
            p.sprite = AssetManager.Instance.crystalSprite;
            p.AbsorbsPlayerProjectiles = true;
            return p;
        }
        public static Obstacle Tree(Game1 game, float direction, Vector2 position)
        {
            Obstacle p = new Obstacle(game, position, Globals.OBSTACLE_RADIUS);
            p.sprite = AssetManager.Instance.treeSprite;
            return p;
        }

        public override void HandleCollision(Entity other)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            if (sprite != null)
                AssetManager.Instance.DrawSprite(this, sprite, 0, depth: .8f);   //Obstacles should be drawn in front of the background but behind everything else
            else
                AssetManager.Instance.PrintString("[]", Position, Color.Black);

        }

    }
}
