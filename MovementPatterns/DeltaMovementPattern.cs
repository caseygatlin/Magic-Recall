using Microsoft.Xna.Framework;
using System;

namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// Helps determine the movement of a projectile.
    /// </summary>
    internal abstract class DeltaMovementPattern : MovementPattern
    {
        protected Vector2 current_position;
        public DeltaMovementPattern(Entity parent) : base(parent)
        {
            current_position = parent.Position;
        }

        /// <summary>
        /// Retrieves the position of where this object should be at the current time.
        /// </summary>
        /// <returns>The position the thing should be at.</returns>
        public sealed override Vector2 getPosition()
        {
            return new Vector2(current_position.X, current_position.Y);
        }

        /// <summary>
        /// Calls UpdateCurrentPosition if the game isn't paused.
        /// </summary>
        /// <param name="deltaTime"></param>
        public sealed override void Update(int deltaTime)
        {
            if (paused) return;
            UpdateCurrentPosition(deltaTime);
        }
        /// <summary>
        /// Computes where the current position should be for the next frame.
        /// </summary>
        /// <param name="deltaTime">The time since the previous frame (in milliseconds).</param>
        public abstract void UpdateCurrentPosition(int deltaTime);
    }
}
