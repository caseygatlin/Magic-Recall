﻿using Microsoft.Xna.Framework;
using out_and_back.MovementPatterns;

namespace out_and_back
{
    /// <summary>
    /// Represents an enemy in the game.
    /// </summary>
    class Enemy : MovementManagedEntity
    {
        /// <summary>
        /// Basic constructor for an enemy entity.
        /// </summary>
        /// <param name="game">The game this enemy belongs to.</param>
        /// <param name="team">The team this enemy belongs to.</param>
        /// <param name="direction">The direction the enemy is moving in.</param>
        /// <param name="speed">The speed the enemy is moving at.</param>
        /// <param name="position">The starting position of the enemy.</param>
        /// <param name="size">The hitbox size of the enemy.</param>
        internal Enemy(Game1 game, Team team, float direction, float speed, Vector2 position, float radius) : base(game, team, direction, speed, position, radius)
        {
        }
        public static Enemy Ghost(Game1 game, float direction, Vector2 position)
        {
            Enemy g = new Enemy(game, Team.Enemy, direction, 100, position, 30);
            g.pattern = MovementPattern.Straight(g, float.PositiveInfinity);
            return g;
        }

        protected override void Move(int deltaTime)
        {
            pattern.Update(deltaTime);
            Position = pattern.getPosition();
        }

        public override void Draw(GameTime gameTime)
        {
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
    }
}
