using System;
using Microsoft.Xna.Framework;

namespace out_and_back
{
    class Globals
    {
        //The speed of the player
        public const float MAX_PLAYER_SPEED = 200;

        //How many hits the player can take
        public const int MAX_PLAYER_HEALTH = 5;
        public const int INVINCIBILITY_DURATION = 3000;//ms

        //The collision radius for obstacles
        public const float OBSTACLE_RADIUS = 30;

        //Direction constants
        public const float PI = MathHelper.Pi;
        public const float UP_DIR = 3 * MathHelper.PiOver2;
        public const float DOWN_DIR = MathHelper.PiOver2;
        public const float LEFT_DIR = MathHelper.Pi;
        public const float RIGHT_DIR = 0;
        public const float UP_RIGHT_DIR = 7 * MathHelper.PiOver4;
        public const float DOWN_RIGHT_DIR = MathHelper.PiOver4;
        public const float UP_LEFT_DIR = 5 * MathHelper.PiOver4;
        public const float DOWN_LEFT_DIR = 3 * MathHelper.PiOver4;

        //Screen dimensions
        public const int SCREEN_WIDTH = 800;
        public const int SCREEN_HEIGHT = 450;

        //Weapon speed
        public const float MAX_WEAPON_SPEED = 150;


        //Returns the direction such that an object at point1 faces point2
        public static float getDirection(Vector2 point1, Vector2 point2)
        {
            double slope = (point2.Y - point1.Y) / (point2.X - point1.X);

            float direction = (float)Math.Atan(slope);

            if (point2.X < point1.X)
                direction = direction + PI;

            return direction;
        }
    }
}
