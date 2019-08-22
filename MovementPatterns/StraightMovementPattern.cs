using System;
using Microsoft.Xna.Framework;

namespace out_and_back.MovementPatterns
{
    class StraightMovementPattern : MovementPattern
    {
        public StraightMovementPattern(Projectile parent) : base(parent)
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
