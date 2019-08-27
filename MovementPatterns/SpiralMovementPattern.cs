using System;

namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// A movement pattern that will move an entity in a spiral shape.
    /// </summary>
    class SpiralMovementPattern : ParameterizedMovementPattern
    {
        private float t;

        /// <summary>
        /// Creates a spiral movement pattern.
        /// </summary>
        /// <param name="parent">The object whose movement is defined by this pattern.</param>
        /// <param name="t">How tight the loop should be. Lower is tighter.</param>
        public SpiralMovementPattern(Entity parent, float t) : base(parent)
        {
            this.t = t;

            float x(int time) => (float)time / 1000 * 1/t *
                    (float)Math.Cos((float)time / 1000) * speed;
            float y(int time) => (float)time / 1000 * 1/t *
                    (float)Math.Sin((float)time / 1000) * speed;

            XParam = (int time) => x(time) * (float)Math.Cos(angle) - y(time) * (float)Math.Sin(angle) + origin.X;
            YParam = (int time) => x(time) * (float)Math.Sin(angle) + y(time) * (float)Math.Cos(angle) + origin.Y;
        }
    }
}
