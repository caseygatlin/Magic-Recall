using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace out_and_back.MovementPatterns
{
    class SpiralMovementPattern : ParameterizedMovementPattern
    {
        private float a;
        private float t;

        private double current_r;

        public SpiralMovementPattern(Entity parent, float a, float t) : base(parent)
        {
            XParam = (int time) =>
            {
                float theta = speed * time;
                return (int)(getR(theta) * Math.Cos(theta));
            };
            YParam = (int time) =>
            {
                float theta = speed * time;
                return (int)(getR(time) * Math.Sin(time));
            };
        }

        private double getR(float theta)
        {
            current_r = Math.Pow(a, theta * t) - 1;
            return current_r;
        }
    }
}
