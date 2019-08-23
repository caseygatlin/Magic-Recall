using System.Collections.Generic;
using Microsoft.Xna.Framework;
using out_and_back.MovementPatterns;

namespace out_and_back
{
    /// <summary>
    /// A special type of entity whose movement is controlled by a movement pattern
    /// rather than the default Entity movement system.
    /// </summary>
    abstract class MovementManagedEntity : Entity
    {
        protected MovementPattern pattern;

        /// <summary>
        /// TODO: Allow this to be filled up, and when the movement finishes, pop it and
        /// make pattern retrieve the top of the pattern queue.
        /// </summary>
        Queue<MovementPattern> patternQueue;

        public MovementManagedEntity(Game1 game, Team team, float direction, float speed, Vector2 position, float radius) : base(game, team, direction, speed, position, radius)
        {
        }

        protected override void Move(int deltaTime)
        {
            pattern.Update(deltaTime);
            Position = pattern.getPosition();
        }
    }
}
