using System;
using Microsoft.Xna.Framework;

namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// Causes a projectile to move in a back and forth pattern.
    /// </summary>
    class YoyoMovementPattern : MovementPattern
    {
        bool paused = false;
        bool limitReached = false;
        int maxDistance = 20;
        int multiplier = 1;

        internal YoyoMovementPattern(Entity parent) : base(parent)
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

        /// <summary>
        /// Updates the movement of this.sssssss
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void Update(int deltaTime)
        {
            if (paused) return;
            base.Update(deltaTime);
            Vector2 currentPos = getPosition();
            float distance = Vector2.Distance(origin, currentPos);
            if (distance >= maxDistance && !limitReached)
            {
                limitReached = true;
                reverseTime();
            }
            else if (limitReached && distance < 0.1)
            {
                CompleteMovement(null);
            }
        }

        /// <summary>
        /// Causes the yoyo to start/stop moving.
        /// </summary>
        public void TogglePause()
        {
            if (!limitReached) paused = !paused;
        }
    }
}
