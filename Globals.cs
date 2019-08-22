using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace out_and_back
{
    class Globals
    {
        //The speed of the player
        public const float MAX_PLAYER_SPEED = 90;

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

        //Weapon speed
        public const float MAX_WEAPON_SPEED = 75;
    }
}
