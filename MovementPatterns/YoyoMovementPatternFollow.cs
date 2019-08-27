using System;
using Microsoft.Xna.Framework;

namespace out_and_back.MovementPatterns
{
    class YoyoMovementPatternFollow : MovementPattern
    {
        bool limitReached = false;
        int maxDistance = 50;
        int multiplier = 1;
        Game1 game;
        private Vector2 destination;
        private float oldRange;

        /// <summary>
        /// Creates a Yoyo Movement Pattern.
        /// </summary>
        /// <param name="parent">The entity this yoyo is being created for.</param>
        /// <param name="cycles">The amount of times this pattern should be executed.</param>
        internal YoyoMovementPatternFollow(Entity parent) : base(parent)
        {
            game = (Game1)parent.Game;

            XParam = (int time) =>
            {
                return speed * multiplier * (float)Math.Cos(angle) * time / 1000 + origin.X;
            };
            YParam = (int time) =>
            {
                return speed * multiplier * (float)Math.Sin(angle) * time / 1000 + origin.Y;
            };
        }

        /// <summary>
        /// Updates the movement of this.
        /// </summary>
        /// <param name="deltaTime">The amount of time, in milliseconds, that has passed since last update.</param>
        public override void Update(int deltaTime)
        {
            Vector2 playerPos = game.getPlayerPos();
            base.Update(deltaTime);
            Vector2 currentPos = getPosition();


            float distance = Vector2.Distance(origin, currentPos);
            float playerDistance = Vector2.Distance(playerPos, currentPos);
            

            if (distance >= maxDistance && !limitReached)
            {
                destination = new Vector2(currentPos.X, currentPos.Y);
                limitReached = true;
                ReverseTime();             

                XParam = (int time) =>
                {
                    oldRange = time;

                    return Vector2.Lerp(playerPos, destination, 1f).X;
                };
                YParam = (int time) =>
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

                XParam = (int time) =>
                {
                    float newTimeInRange = time / oldRange;

                    return Vector2.Lerp(playerPos, destination, newTimeInRange).X;
                };
                YParam = (int time) =>
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
