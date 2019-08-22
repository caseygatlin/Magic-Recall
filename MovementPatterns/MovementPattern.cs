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

        /// <summary>
        /// Right now, I see no reason for this to be wildly available.
        /// </summary>
        public MovementPattern(Projectile parent)
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
            lifetime += deltaTime;
        }

        /// <summary>
        /// Creates a movement pattern that moves the object in a straight line.
        /// </summary>
        /// <param name="speed">The speed at which the object moves.</param>
        /// <param name="angle">The starting angle of the object.</param>
        /// <param name="origin">The spawn location of the object.</param>
        /// <returns>A movement pattern that will move a projectile in a single direction.</returns>
        public static MovementPattern Straight(Projectile parent)
        {
            return new StraightMovementPattern(parent);
        }

        public static MovementPattern Yoyo(Projectile parent)
        {
            return new YoyoMovementPattern(parent);
        }

        protected void resetTime()
        {
            lifetime = 0;
        }

        public event EventHandler MovementCompleted;

        protected virtual void CompleteMovement(EventArgs e)
        {
            EventHandler handle = MovementCompleted;
            MovementCompleted?.Invoke(this, e);
        }
    }
}
