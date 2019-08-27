using Microsoft.Xna.Framework;
using System;

namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// Causes the projectile to move indefinitely in a single line.
    /// </summary>
    class StraightMovementPattern : ParameterizedMovementPattern
    {
        private float maxDistance;

        internal StraightMovementPattern(Entity parent, float maxDistance = float.PositiveInfinity) : base(parent)
        {
            this.maxDistance = maxDistance;
            XParam = (float time) =>
            {
                return speed * (float)Math.Cos(angle) * time + origin.X;
            };
            YParam = (float time) =>
            {
                return speed * (float)Math.Sin(angle) * time + origin.Y;
            };
        }

        public override void Update(int deltaTime)
        {
            base.Update(deltaTime);
            if (Vector2.Distance(origin, getPosition()) >= maxDistance)
                CompleteMovement(null);
        }
    }
}
