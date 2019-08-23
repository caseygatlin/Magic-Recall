using Microsoft.Xna.Framework;
using System;

namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// Helps determine the movement of a projectile.
    /// </summary>
    internal abstract class MovementPattern
    {
        protected Vector2 origin;
        protected float angle;
        protected float speed;
        private int lifetime = 0;
        private int timeflow = 1;

        /// <summary>
        /// Right now, I see no reason for this to be wildly available.
        /// </summary>
        public MovementPattern(Entity parent)
        {
            origin = parent.Position;
            angle = parent.Direction;
            speed = parent.Speed;
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


        public Vector2 getPosition()
        {
            return new Vector2(XParam(lifetime), YParam(lifetime));
        }

        public virtual void Update(int deltaTime)
        {
            lifetime += deltaTime * timeflow;
        }

        /// <summary>
        /// Creates a movement pattern that moves the object in a straight line.
        /// </summary>
        /// <param name="parent">The entity that is moving.</param>
        /// <param name="limit">How far this movement pattern should take the unit before ending.</param>
        /// <returns>A movement pattern that will move a projectile in a single direction.</returns>
        public static MovementPattern Straight(Entity parent, float limit)
        {
            return new StraightMovementPattern(parent, limit);
        }

        /// <summary>
        /// Creates a Yoyo Movement Pattern that causes an object to move back and forth.
        /// </summary>
        /// <param name="parent">The entity that is moving.</param>
        /// <param name="cycles">The amount of times the back and forth should occur. A non-positive number represents infinity.</param>
        /// <returns>The YoyoMovementPattern.</returns>
        public static MovementPattern Yoyo(Entity parent, int cycles)
        {
            return new YoyoMovementPattern(parent, cycles);
        }

        protected void resetTime()
        {
            lifetime = 0;
        }

        protected void reverseTime()
        {
            timeflow *= -1;
        }

        public event EventHandler MovementCompleted;

        protected virtual void CompleteMovement(EventArgs e)
        {
            EventHandler handle = MovementCompleted;
            MovementCompleted?.Invoke(this, e);
        }
    }
}
