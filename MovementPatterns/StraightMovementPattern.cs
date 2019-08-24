using Microsoft.Xna.Framework;
using System;

namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// Causes the projectile to move indefinitely in a single line.
    /// </summary>
    class StraightMovementPattern : MovementPattern
    {
        private float maxDistance;

        internal StraightMovementPattern(Entity parent, float maxDistance = float.PositiveInfinity) : base(parent)
        {
            this.maxDistance = maxDistance;
            XParam = (int time) =>
            {
                return speed * (float)Math.Cos(angle) * time / 1000 + origin.X;
            };
            YParam = (int time) =>
            {
                return speed * (float)Math.Sin(angle) * time / 1000 + origin.Y;
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
