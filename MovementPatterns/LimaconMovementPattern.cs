using System;

namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// Creates a pattern that moves objects in a Limacon shape.
    /// </summary>
    class LimaconMovementPattern : MovementPattern
    {
        private readonly int a;
        private readonly int b;
        
        /// <summary>
        /// Creates a pattern that moves an object like a limacon.
        /// </summary>
        /// <param name="parent">The object moving according to this pattern.</param>
        /// <param name="a">Affects the size of the looping.</param>
        /// <param name="b">Affects how far out the looping goes.</param>
        public LimaconMovementPattern(Entity parent, int a, int b) : base(parent)
        {
            float x(int time) => (a + b * (float)Math.Cos(time)) * (float)Math.Cos(time);
            float y(int time) => (a + b * (float)Math.Cos(time)) * (float)Math.Sin(time);

            XParam = (int time) => x(time) * (float)Math.Cos(angle) - y(time) * (float)Math.Sin(angle) + origin.X;
            YParam = (int time) => x(time) * (float)Math.Sin(angle) + y(time) * (float)Math.Cos(angle) + origin.Y;
        }
    }
}
