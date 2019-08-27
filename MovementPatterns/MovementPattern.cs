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
        protected bool paused = false;
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

        /// <summary>
        /// Retrieves the position of where this object should be at the current time.
        /// </summary>
        /// <returns>The position the thing should be at.</returns>
        public Vector2 getPosition()
        {
            return new Vector2(XParam(lifetime), YParam(lifetime));
        }

        public virtual void Update(int deltaTime)
        {
            if (paused) return;
            lifetime += deltaTime * timeflow;
        }

        /// <summary>
        /// While you could just set an entity's velocity to zero, this works just
        /// as well, if not better, since there's no math being performed when finding
        /// the location of the unit based on time.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static MovementPattern Stationary(Entity parent)
        {
            return new StationaryMovementPattern(parent);
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

        /// <summary>
        /// Creates a movement pattern that 
        /// </summary>
        /// <param name="parent">The entity that is using this movement pattern.</param>
        /// <param name="game">The game the entity exists in.</param>
        /// <returns></returns>
        public static MovementPattern YoyoFollow(Entity parent)
        {
            return new YoyoMovementPatternFollow(parent);
        }

        /// <summary>
        /// Creates a spiral movement pattern.
        /// </summary>
        /// <param name="parent">The entity owning this movement pattern.</param>
        /// <param name="game">The game this pattern is working in.</param>
        /// <returns>A Spiral Movement Pattern.</returns>
        public static MovementPattern Spiral(Entity parent)
        {
            return new SpiralMovementPattern(parent, 1);
        }

        /// <summary>
        /// Resets the lifetime of this pattern.
        /// </summary>
        protected void ResetTime()
        {
            lifetime = 0;
        }

        /// <summary>
        /// Reverses the flow of time for this movement pattern.
        /// </summary>
        protected void ReverseTime()
        {
            timeflow *= -1;
        }

        /// <summary>
        /// Causes the projectile to become paused.
        /// </summary>
        protected void ToggleTimePause()
        {
            paused = !paused;
        }

        public event EventHandler MovementCompleted;

        protected virtual void CompleteMovement(EventArgs e)
        {
            EventHandler handle = MovementCompleted;
            MovementCompleted?.Invoke(this, e);
        }
    }
}
