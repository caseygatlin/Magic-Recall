using System;
using Microsoft.Xna.Framework;

namespace out_and_back.MovementPatterns
{
    class YoyoMovementPattern : MovementPattern
    {
        bool limitReached = false;
        int maxDistance = 20;
        int multiplier = 1;

        internal YoyoMovementPattern(Projectile parent) : base(parent)
        {
            XParam = (int time) =>
            {
                return speed * multiplier * (float)Math.Cos(angle) * time / 1000 + origin.X;
            };
            YParam = (int time) =>
            {
                return speed * multiplier * (float)Math.Sin(angle) * time / 1000 + origin.Y;
            };
        }

        public override void Update(int deltaTime)
        {
            base.Update(deltaTime);
            Vector2 currentPos = getPosition();
            float distance = Vector2.Distance(origin, currentPos);
            if (distance >= maxDistance && !limitReached)
            {
                limitReached = true;
                origin = currentPos;
                multiplier = -1;
                resetTime();
            }
            else if (limitReached && distance >= maxDistance)
            {
                CompleteMovement(null);
            }
        }
    }
}
