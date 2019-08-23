using Microsoft.Xna.Framework;
using out_and_back.MovementPatterns;

namespace out_and_back
{
    /// <summary>
    /// Represents some sort of projectile in the game.
    /// </summary>
    class Projectile : Entity
    {
        int maxLifetime;
        int lifetime = 0;

        MovementPattern pattern;

        /// <summary>
        /// Basic constructor for a projectile entity.
        /// </summary>
        /// <param name="game">The game this projectile belongs to.</param>
        /// <param name="team">The team this projectile belongs to.</param>
        /// <param name="direction">The direction the projectile is moving in.</param>
        /// <param name="speed">The speed the projectile is moving at.</param>
        /// <param name="position">The starting position of the projectile.</param>
        /// <param name="lifetime">The time, in milliseconds, that this object should exist.</param>
        public Projectile(Game1 game, Team team, float direction, float speed, Vector2 position, int lifetime = -1) : base(game, team, direction, speed, position)
        {
            maxLifetime = lifetime;
            pattern = team == Team.Player ? MovementPattern.Yoyo(this) : MovementPattern.Straight(this);
            if (pattern is YoyoMovementPattern)
            {
                pattern.MovementCompleted += YoyoMovementCompleted;
            }
            aabb = new Rectangle((int)position.X, (int)position.Y, 10, 10);
        }

        private void YoyoMovementCompleted(object sender, System.EventArgs e)
        {
            Remove(null);
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            pattern.Update(gameTime.ElapsedGameTime.Milliseconds);
            Position = pattern.getPosition();
            lifetime += gameTime.ElapsedGameTime.Milliseconds;
            if (lifetime >= maxLifetime && maxLifetime > 0)
                Remove(null);
        }

        public override void Draw(GameTime gameTime)
        {
            AssetManager.Instance.PrintString("prj", Position, Team == Team.Enemy ? Color.Red : Color.Blue);
        }

        public override void HandleCollision(Entity other)
        {
            // Ignore other projectiles.
            if (other is Projectile) return;
            // Ignore objects of the same team.
            if (Team == other.Team) return;
            // It's hit an entity - either Enemy or Player - and should therefore despawn.
            Remove(null);
        }
    }
}
