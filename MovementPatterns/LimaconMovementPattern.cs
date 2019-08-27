using Microsoft.Xna.Framework;
using System;

namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// Creates a pattern that moves objects in a Limacon shape.
    /// </summary>
    class LimaconMovementPattern : ParameterizedMovementPattern
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
            this.a = a;
            this.b = b;

            float x(int time) => (this.a + this.b * (float)Math.Cos((float)time / 1000)) * (float)Math.Cos((float)time / 1000);
            float y(int time) => (this.a + this.b * (float)Math.Cos((float)time / 1000)) * (float)Math.Sin((float)time / 1000);

            XParam = (int time) => x(time) * (float)Math.Cos(angle) - y(time) * (float)Math.Sin(angle) + origin.X;
            YParam = (int time) => x(time) * (float)Math.Sin(angle) + y(time) * (float)Math.Cos(angle) + origin.Y;
        }

        public override void Update(int deltaTime)
        {
            base.Update(deltaTime);
            // TODO: At 2Pi, the limacon is done.
        }
    }
}
