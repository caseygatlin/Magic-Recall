using Microsoft.Xna.Framework;

namespace out_and_back
{
    /// <summary>
    /// Represents some sort of projectile in the game.
    /// </summary>
    class Projectile : Entity
    {
        const int MAX_LIFETIME = 3000;
        int lifetime = 0;

        /// <summary>
        /// Basic constructor for a projectile entity.
        /// </summary>
        /// <param name="game">The game this projectile belongs to.</param>
        /// <param name="team">The team this projectile belongs to.</param>
        /// <param name="direction">The direction the projectile is moving in.</param>
        /// <param name="speed">The speed the projectile is moving at.</param>
        /// <param name="position">The starting position of the projectile.</param>
        public Projectile(Game game, Team team, float direction, float speed, Vector2 position) : base(game, team, direction, speed, position)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            lifetime += gameTime.ElapsedGameTime.Milliseconds;
            if (lifetime >= MAX_LIFETIME)
                Remove(new System.EventArgs());
        }

        public override void Draw(GameTime gameTime)
        {
            AssetManager.Instance.PrintString("prj", Position, Team == Team.Enemy ? Color.Red : Color.Blue);
        }

        protected override void HandleCollision(Entity other)
        {
            // Ignore other projectiles.
            if (other is Projectile) return;
            // Ignore objects of the same team.
            if (Team == other.Team) return;
            // It's hit an entity - either Enemy or Player - and should therefore despawn.
            Remove(new System.EventArgs());
        }
    }
}
