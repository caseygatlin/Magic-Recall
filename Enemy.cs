using Microsoft.Xna.Framework;
using out_and_back.MovementPatterns;

namespace out_and_back
{
    /// <summary>
    /// Represents an enemy in the game.
    /// </summary>
    class Enemy : MovementManagedEntity
    {
        AttackPatterns.AttackPattern attackPattern;

        /// <summary>
        /// Basic constructor for an enemy entity.
        /// </summary>
        /// <param name="game">The game this enemy belongs to.</param>
        /// <param name="team">The team this enemy belongs to.</param>
        /// <param name="direction">The direction the enemy is moving in.</param>
        /// <param name="speed">The speed the enemy is moving at.</param>
        /// <param name="position">The starting position of the enemy.</param>
        /// <param name="radius">The hitbox radius of the enemy.</param>
        internal Enemy(Game1 game, Team team, float direction, float speed, Vector2 position, float radius) : base(game, team, direction, speed, position, radius)
        {
        }

        //An enum for detecting differences between enemy types
        public enum EnemyType
        {
            GHOST,
            SLIME
        }

        private EnemyType type;
        public void setType(EnemyType enemyType)
        {
            type = enemyType;
        }

        //Ghost enemy. Moves in a straight line and shoots a spread of bullets
        public static Enemy Ghost(Game1 game, float direction, Vector2 position)
        {
            Enemy g = new Enemy(game, Team.Enemy, direction, 100, position, 30);
            g.AddPattern(MovementPattern.Straight(g, float.PositiveInfinity));
            g.SetAttackPattern(new AttackPatterns.FanAttackPattern(g, 2, direction + MathHelper.PiOver4, 0, null, null, null));
            g.setType(EnemyType.GHOST);
            return g;

        }

        
        //Slime enemy. Moves in a straight line, but stops every so often to 
        public static Enemy Slime(Game1 game, float direction, Vector2 position)
        {
            Enemy s = new Enemy(game, Team.Enemy, direction, 50, position, 30);
            s.AddPattern(MovementPattern.Straight(s, 10));
            s.setType(EnemyType.SLIME);
            return s;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (attackPattern != null)
                attackPattern.Update(gameTime);

        }

        protected override void Move(int deltaTime)
        {
            if (type == EnemyType.SLIME && Pattern == null)
            {
                Direction = Globals.getDirection(Position, currentGame.getPlayerPos());
                AddPattern(MovementPattern.Straight(this, 10));
            }

            Pattern.Update(deltaTime);

            if (Pattern != null)
                Position = Pattern.getPosition();

            //If an enemy goes waaaay out of bounds, we should kill it so it doesn't run away forever
            if (Position.X > Globals.SCREEN_WIDTH * 2 || Position.X < -Globals.SCREEN_WIDTH
                || Position.Y > Globals.SCREEN_HEIGHT * 2 || Position.Y < -Globals.SCREEN_HEIGHT)
            {
                Dispose();
                Remove(null);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //AssetManager.Instance.PrintString("^_^", Position, Team == Team.Enemy ? Color.Red : Color.Blue);
            if (type == EnemyType.GHOST)
                AssetManager.Instance.DrawSprite(this, AssetManager.Instance.ghostSprite);
            else if (type == EnemyType.SLIME)
                AssetManager.Instance.DrawSprite(this, AssetManager.Instance.slimeSprite);
            else
                AssetManager.Instance.PrintString("^_^", Position, Team == Team.Enemy ? Color.Red : Color.Blue);

        }

        public override void HandleCollision(Entity other)
        {
            // Ignore objects of the same team.
            if (Team == other.Team) return;
            if (other is Player || (other is Projectile && other.Team != Team))
            {
                Dispose();
                Remove(null);
            }
        }

        /// <summary>
        /// Sets this enemy's attack pattern.
        /// </summary>
        /// <param name="pattern">The attack pattern to use.</param>
        private void SetAttackPattern(AttackPatterns.AttackPattern pattern)
        {
            attackPattern = pattern;
        }
    }
}
