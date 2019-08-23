﻿using System;
using Microsoft.Xna.Framework;

namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// Causes a projectile to move in a back and forth pattern.
    /// </summary>
    class YoyoMovementPattern : MovementPattern
    {
        bool limitReached = false;
        int maxDistance = 20;
        int multiplier = 1;
        int cycles;
        int cycleCount = 0;

        /// <summary>
        /// Creates a Yoyo Movement Pattern.
        /// </summary>
        /// <param name="parent">The entity this yoyo is being created for.</param>
        /// <param name="cycles">The amount of times this pattern should be executed.</param>
        internal YoyoMovementPattern(Entity parent, int cycles = 1) : base(parent)
        {
            this.cycles = cycles;
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
        /// Updates the movement of this.
        /// </summary>
        /// <param name="deltaTime">The amount of time, in milliseconds, that has passed since last update.</param>
        public override void Update(int deltaTime)
        {
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
                if (cycles > 0 && ++cycleCount == cycles)
                    CompleteMovement(null);
                else
                {
                    limitReached = false;
                    reverseTime();
                }
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
