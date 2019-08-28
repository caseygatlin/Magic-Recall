using Microsoft.Xna.Framework;
using System;

namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// Causes the projectile to move indefinitely in a single line.
    /// </summary>
    class StraightMovementPattern : DeltaMovementPattern
    {
        private float maxDistance;
        private float angle;
        private Vector2 starting_position;

        internal StraightMovementPattern(Entity parent, float maxDistance = float.PositiveInfinity) : base(parent)
        {
            this.maxDistance = maxDistance;
            angle = parent.Direction;
            starting_position = parent.Position;
        }
        protected override Vector2 ComputeDelta(int deltaTime)
        {
            if (Vector2.Distance(starting_position, current_position) >= maxDistance)
            {
                CompleteMovement(null);
                return Vector2.Zero;
            }

            return speed * deltaTime / 1000 * new Vector2((float) Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}
