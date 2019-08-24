using Microsoft.Xna.Framework;
using out_and_back.MovementPatterns;

namespace out_and_back
{
    /// <summary>
    /// Represents some sort of projectile in the game.
    /// </summary>
    class Projectile : MovementManagedEntity
    {
        int maxLifetime;
        int lifetime = 0;

        /// <summary>
        /// Basic constructor for a projectile entity.
        /// </summary>
        /// <param name="game">The game this projectile belongs to.</param>
        /// <param name="team">The team this projectile belongs to.</param>
        /// <param name="direction">The direction the projectile is moving in.</param>
        /// <param name="speed">The speed the projectile is moving at.</param>
        /// <param name="position">The starting position of the projectile.</param>
        /// <param name="radius">The size of the projectile's hitbox.</param>
        /// <param name="lifetime">The time, in milliseconds, that this object should exist.</param>
        public Projectile(Game1 game, Team team, float direction, float speed, Vector2 position, float radius, int lifetime = -1) : base(game, team, direction, speed, position, radius)
        {
            maxLifetime = lifetime;
            AddPattern(team == Team.Player ? MovementPattern.Yoyo(this, 1) : MovementPattern.Straight(this, float.PositiveInfinity));
            /*if (Pattern is YoyoMovementPattern)
            {
                AddPattern(MovementPattern.Straight(this, 100));
            }*/
        }

        protected override void PatternComplete(object sender, System.EventArgs e)
        {
            base.PatternComplete(sender, e);
            if (Pattern == null)
                Remove(null);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            lifetime += gameTime.ElapsedGameTime.Milliseconds;
            if (lifetime >= maxLifetime && maxLifetime > 0)
                Remove(null);
        }

        public override void Draw(GameTime gameTime)
        {
            //If the projectile is on the same team as the player, draw the fireball
            // TODO: if we have different attacks, probably make a class for each type so we know which sprite to load
            
            if (base.Team == Team.Player)
            {
                AssetManager.Instance.DrawCharWeapon(Position, Direction);
            }
            else
            
                AssetManager.Instance.PrintString("prj", Position, Team == Team.Enemy ? Color.Red : Color.Blue);
            
        }

        public override void HandleCollision(Entity other)
        {
            // Ignore other projectiles.
            if (other is Projectile) return;
            // Ignore objects of the same team.
            if (Team == other.Team || Team == Team.Player) return;
            // It's hit an enemy and should therefore despawn.
            Remove(null);
        }
    }
}
