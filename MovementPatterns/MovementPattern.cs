using Microsoft.Xna.Framework;
using System;

namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// Helps determine the movement of projectiles and enemies.
    /// </summary>
    internal abstract class MovementPattern
    {
        protected float speed;
        protected bool paused = false;

        public MovementPattern(Entity parent)
        {
            speed = parent.Speed;
        }

        public abstract Vector2 getPosition();
        public abstract void Update(int deltaTime);

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
        public static MovementPattern Straight(Entity parent, float limit = float.PositiveInfinity)
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

        public static MovementPattern YoyoFollow(Entity parent, Game1 game)
        {
            return new YoyoMovementPatternFollow(parent, game);
        }

        public static MovementPattern Spiral(Entity parent, Game1 game)
        {
            return new SpiralMovementPattern(parent, 2.5f, 0.1f);
        }

        public static MovementPattern PursueEntity(Entity parent, Entity target)
        {
            return new PursueEntityPattern(parent, target);
        }

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
