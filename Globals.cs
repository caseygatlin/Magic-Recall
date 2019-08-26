using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace out_and_back
{
    class Globals
    {
        //The speed of the player
        public const float MAX_PLAYER_SPEED = 200;

        //How many hits the player can take
        public const int MAX_PLAYER_HEALTH = 1;

        //Direction constants
        public const float PI = 3.14f;
        public const float UP_DIR = 3 * PI / 2;
        public const float DOWN_DIR = PI / 2;
        public const float LEFT_DIR = PI;
        public const float RIGHT_DIR = 0;
        public const float UP_RIGHT_DIR = 7 * PI / 4;
        public const float DOWN_RIGHT_DIR = PI / 4;
        public const float UP_LEFT_DIR = 5 * PI / 4;
        public const float DOWN_LEFT_DIR = 3 * PI / 4;

        //Screen dimensions
        public const int SCREEN_WIDTH = 800;
        public const int SCREEN_HEIGHT = 480;

        //Weapon speed
        public const float MAX_WEAPON_SPEED = 150;


        //Returns the direction such that an object at point1 faces point2
        public static float getDirection(Vector2 point1, Vector2 point2)
        {
            double slope = (point2.Y - point1.Y) / (point2.X - point1.X);

            float direction = (float)System.Math.Atan(slope);

            if (point2.X < point1.X)
                direction = direction + Globals.PI;

            return direction;
        }
    }
}
