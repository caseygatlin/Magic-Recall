using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace out_and_back
{
    class Player : Entity
    {


        public int health;

        /// <summary>
        /// Basic constructor for a projectile entity.
        /// </summary>
        /// <param name="game">The game this projectile belongs to.</param>
        /// <param name="team">The team this projectile belongs to.</param>
        /// <param name="direction">The direction the projectile is moving in.</param>
        /// <param name="speed">The speed the projectile is moving at.</param>
        /// <param name="position">The starting position of the projectile.</param>
        public Player(Game game, Team team, float direction, float speed, Vector2 position) : base(game, team, direction, speed, position)
        {
            health = Globals.MAX_PLAYER_HEALTH;
        }

        public void MovementInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    Direction = Globals.UP_LEFT_DIR;
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                    Direction = Globals.UP_RIGHT_DIR;
                else
                    Direction = Globals.UP_DIR;
                Speed = Globals.MAX_PLAYER_SPEED;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    Direction = Globals.UP_LEFT_DIR;
                else if (Keyboard.GetState().IsKeyDown(Keys.S))
                    Direction = Globals.DOWN_LEFT_DIR;
                else
                    Direction = Globals.LEFT_DIR;
                Speed = Globals.MAX_PLAYER_SPEED;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    Direction = Globals.DOWN_LEFT_DIR;
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                    Direction = Globals.DOWN_RIGHT_DIR;
                else
                    Direction = Globals.DOWN_DIR;
                Speed = Globals.MAX_PLAYER_SPEED;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    Direction = Globals.UP_RIGHT_DIR;
                else if (Keyboard.GetState().IsKeyDown(Keys.S))
                    Direction = Globals.DOWN_RIGHT_DIR;
                else
                    Direction = Globals.RIGHT_DIR;
                Speed = Globals.MAX_PLAYER_SPEED;
            }

            if (!Keyboard.GetState().IsKeyDown(Keys.W) &&
                !Keyboard.GetState().IsKeyDown(Keys.A) &&
                !Keyboard.GetState().IsKeyDown(Keys.S) &&
                !Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Speed = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            MovementInput();
            AssetManager.Instance.PrintString("Player", Position);
        }

        protected override void HandleCollision(Entity other)
        {
            // It hits an entity on the same team, return.
            if (Team == other.Team) return;

            // It's hit an entity - either Enemy or Player - and should therefore despawn.
            health--;
            if (health <= 0)
            {
                Dispose();
            }
            
        }
    }



}
