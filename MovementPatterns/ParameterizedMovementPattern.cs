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
        private int timeflow = 1;

        public float Lifetime
        {
            get;
            private set;
        } = 0;

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
        protected Func<float, float> XParam;
        /// <summary>
        /// The function that determines the movement in the Y direction.
        /// The first parameter is the total time in milliseconds that the
        /// attack has been out, and the output is the y position at that time.
        /// </summary>
        protected Func<float, float> YParam;

        /// <summary>
        /// Retrieves the position of where this object should be at the current time.
        /// </summary>
        /// <returns>The position the thing should be at.</returns>
        public override Vector2 getPosition()
        {
            return new Vector2(XParam(Lifetime / 1000), YParam(Lifetime / 1000));
        }

        public override void Update(int deltaTime)
        {
            checkForPaused();
            if (paused) return;
            Lifetime += deltaTime * timeflow;
        }

        protected void ResetTime()
        {
            Lifetime = 0;
        }

        protected void ReverseTime()
        {
            timeflow *= -1;
        }

        /// <summary>
        /// Rotates the graph of the function by the parent's direction.
        /// </summary>
        /// <param name="x">The x function.</param>
        /// <param name="y">The y function.</param>
        protected void Rotate(Func<float, float> x, Func<float, float> y)
        {
            XParam = (float time) => x(time) * (float)Math.Cos(angle) - y(time) * (float)Math.Sin(angle) + origin.X;
            YParam = (float time) => x(time) * (float)Math.Sin(angle) + y(time) * (float)Math.Cos(angle) + origin.Y;
        }
    }
}
