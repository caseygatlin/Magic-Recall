using System;

namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// Causes the projectile to move indefinitely in a single line.
    /// </summary>
    class StraightMovementPattern : MovementPattern
    {
        internal StraightMovementPattern(Entity parent) : base(parent)
        {
            XParam = (int time) =>
            {
                return speed * (float)Math.Cos(angle) * time / 1000 + origin.X;
            };
            YParam = (int time) =>
            {
                return speed * (float)Math.Sin(angle) * time / 1000 + origin.Y;
            };
        }
    }
}
