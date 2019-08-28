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
        protected MovementPattern Pattern
        {
            get => patternQueue.Count > 0 ? patternQueue.Peek() : null;
        }

        /// <summary>
        /// TODO: Allow this to be filled up, and when the movement finishes, pop it and
        /// make pattern retrieve the top of the pattern queue.
        /// </summary>
        Queue<MovementPattern> patternQueue = new Queue<MovementPattern>();

        public MovementManagedEntity(Game1 game, Team team, float direction, float speed, Vector2 position, float radius) : base(game, team, direction, speed, position, radius)
        {
        }

        protected override void Move(int deltaTime)
        {
            Pattern?.Update(deltaTime);
            Position = Pattern?.getPosition() ?? Position; 
        }

        protected void AddPattern(MovementPattern pattern)
        {
            pattern.MovementCompleted += PatternComplete;
            patternQueue.Enqueue(pattern);
        }

        /// <summary>
        /// Move on to the next pattern.
        /// </summary>
        /// <param name="sender">The movement pattern that is being completed.</param>
        /// <param name="e"></param>
        protected virtual void PatternComplete(object sender, System.EventArgs e)
        {
            /*
             * ERROR
             * There's a scenario I don't understand that causes this method to
             * be called when patternQueue is empty. I'm not sure how, but calling
             * Dequeue() on an empty queue crashes the game. If this weren't a
             * prototype, I'd want to look into it more, but I think it should be
             * okay just to suppress/ignore the issue for now. - Aaron
             */
            if(patternQueue.Count > 0)
                patternQueue.Dequeue();
        }
    }
}
