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

        public static Enemy Ghost(Game1 game, float direction, Vector2 position)
        {
            Enemy g = new Enemy(game, Team.Enemy, direction, 100, position, 30);
            g.AddPattern(MovementPattern.Straight(g, float.PositiveInfinity));
            g.setType(EnemyType.GHOST);
            g.SetAttackPattern(new AttackPatterns.FanAttackPattern(g, 3, 0, null, null, null));
            return g;

        }

        

        public static Enemy Slime(Game1 game, float direction, Vector2 position)
        {
            Enemy s = new Enemy(game, Team.Enemy, direction, 50, position, 30);
            s.AddPattern(MovementPattern.Straight(s, 200));
            s.setType(EnemyType.SLIME);
            return s;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            attackPattern.Update(gameTime);
        }

        protected override void Move(int deltaTime)
        {
            Pattern.Update(deltaTime);

            Pattern.MovementCompleted += Pattern_MovementCompleted;

            Position = Pattern.getPosition();

            //If an enemy goes waaaay out of bounds, we should kill it so it doesn't run away forever
            if (Position.X > Globals.SCREEN_WIDTH * 2 || Position.X < -Globals.SCREEN_WIDTH
                || Position.Y > Globals.SCREEN_HEIGHT * 2 || Position.Y < -Globals.SCREEN_HEIGHT)
            {
                Dispose();
                Remove(null);
            }
        }

        private void Pattern_MovementCompleted(object sender, System.EventArgs e)
        {
            if (type == EnemyType.SLIME)
                Dispose();
        }

        public override void Draw(GameTime gameTime)
        {
            //AssetManager.Instance.PrintString("^_^", Position, Team == Team.Enemy ? Color.Red : Color.Blue);
            if (type == EnemyType.GHOST)
                AssetManager.Instance.DrawGhost(Position);
            else if (type == EnemyType.SLIME)
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
