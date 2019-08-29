using System;
using Microsoft.Xna.Framework;

namespace out_and_back.MovementPatterns
{
    class YoyoMovementPatternFollow : ParameterizedMovementPattern
    {
        bool limitReached = false;
        readonly float maxDistance = 50;
        int multiplier = 1;
        Game1 game;
        private Vector2 destination;
        private float oldRange;

        /// <summary>
        /// Creates a Yoyo Movement Pattern.
        /// </summary>
        /// <param name="parent">The entity this yoyo is being created for.</param>
        /// <param name="cycles">The amount of times this pattern should be executed.</param>
        internal YoyoMovementPatternFollow(Entity parent, float maxDistance = 50) : base(parent)
        {
            game = (Game1)parent.Game;
            this.maxDistance = maxDistance;
            XParam = (float time) =>
            {
                return speed * multiplier * (float)Math.Cos(angle) * time + origin.X;
            };
            YParam = (float time) =>
            {
                return speed * multiplier * (float)Math.Sin(angle) * time + origin.Y;
            };
        }

        /// <summary>
        /// Updates the movement of this.
        /// </summary>
        /// <param name="deltaTime">The amount of time, in milliseconds, that has passed since last update.</param>
        public override void Update(int deltaTime)
        {
            checkForPaused();
            if (game.wonGame)
                return;
            Vector2 playerPos = game.Player.Position;
            base.Update(deltaTime);
            Vector2 currentPos = getPosition();


            float distance = Vector2.Distance(origin, currentPos);
            float playerDistance = Vector2.Distance(playerPos, currentPos);
            

            if (distance >= maxDistance && !limitReached)
            {
                destination = new Vector2(currentPos.X, currentPos.Y);
                limitReached = true;
                ReverseTime();             

                XParam = (float time) =>
                {
                    oldRange = time;

                    return Vector2.Lerp(playerPos, destination, 1f).X;
                };
                YParam = (float time) =>
                {
                    return Vector2.Lerp(playerPos, destination, 1f).Y;
                };
            }
            else if (limitReached && playerDistance < 50)
            {
                CompleteMovement(null);
            }
            else if (limitReached)
            {

                XParam = (float time) =>
                {
                    float newTimeInRange = time / oldRange;

                    return Vector2.Lerp(playerPos, destination, newTimeInRange).X;
                };
                YParam = (float time) =>
                {
                    float newTimeInRange = time / oldRange;

                    return Vector2.Lerp(playerPos, destination, newTimeInRange).Y;
                };
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
