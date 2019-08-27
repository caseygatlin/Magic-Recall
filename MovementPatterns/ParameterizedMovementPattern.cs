using Microsoft.Xna.Framework;
using System;

namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// Helps determine the movement of a projectile.
    /// </summary>
    internal abstract class ParameterizedMovementPattern : MovementPattern
    {
        protected Vector2 origin;
        protected float angle;
        private int lifetime = 0;
        private int timeflow = 1;

        /// <summary>
        /// Right now, I see no reason for this to be wildly available.
        /// </summary>
        public ParameterizedMovementPattern(Entity parent) : base(parent)
        {
            origin = parent.Position;
            angle = parent.Direction;
        }

        /// <summary>
        /// The function that determines the movement in the X direction.
        /// The first parameter is the total time in milliseconds that the
        /// attack has been out, and the output is the x position at that time.
        /// </summary>
        protected Func<int, float> XParam;
        /// <summary>
        /// The function that determines the movement in the Y direction.
        /// The first parameter is the total time in milliseconds that the
        /// attack has been out, and the output is the y position at that time.
        /// </summary>
        protected Func<int, float> YParam;

        /// <summary>
        /// Retrieves the position of where this object should be at the current time.
        /// </summary>
        /// <returns>The position the thing should be at.</returns>
        public override Vector2 getPosition()
        {
            return new Vector2(XParam(lifetime), YParam(lifetime));
        }

        public override void Update(int deltaTime)
        {
            if (paused) return;
            lifetime += deltaTime * timeflow;
        }

        protected void ResetTime()
        {
            lifetime = 0;
        }

        protected void ReverseTime()
        {
            timeflow *= -1;
        }
    }
}
