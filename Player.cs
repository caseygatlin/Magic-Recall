using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace out_and_back
{

    class Player : Entity
    {

        public int health;
        public bool isCasting = false;

        /// <summary>
        /// Basic constructor for a player entity.
        /// </summary>
        /// <param name="game">The game this player belongs to.</param>
        /// <param name="direction">The direction the player starts in.</param>
        /// <param name="position">The starting position of the player.</param>
        /// <param name="size">The size of the player's hitbox.</param>
        /// <param name="speed">The speed the player starts at, defaults to zero.</param>
        /// <param name="team">The team this player belongs to.</param>
        public Player(Game1 game, float direction, Vector2 position, Vector2 size, float speed = 0, Team team = Team.Player) : base(game, team, direction, speed, position, size)
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

        //Throws out the attack
        private void CastWeapon(float mouseDirection, Vector2 playerPos)
        {
            Projectile weapon = new Projectile((Game1)Game, Team.Player, mouseDirection, Globals.MAX_WEAPON_SPEED, playerPos, new Vector2(10, 10));
            weapon.Removed += Weapon_Removed;
        }


        //Allows the player to cast again after the attack is done
        private void Weapon_Removed(object sender, System.EventArgs e)
        {
            isCasting = false;
        }

        //Main drawing loop for the player
        public override void Draw(GameTime gameTime)
        {
            MovementInput();

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && isCasting == false)
            {
                double slope = (Mouse.GetState().Y - Position.Y) / (Mouse.GetState().X - Position.X);

                float castDirection = (float)System.Math.Atan(slope);

                if (Mouse.GetState().X < Position.X)
                    castDirection = castDirection + Globals.PI;

                CastWeapon(castDirection, Position);
                isCasting = true;
            }


            //AssetManager.Instance.PrintString("Player", Position);
            AssetManager.Instance.DrawCharSprite(Position);
        }

        public override void HandleCollision(Entity other)
        {
            // It hits an entity on the same team, return.
            if (Team == other.Team) return;

            // It's hit an entity either enemy or projectile - and should therefore lose health.
            health--;
            if (health <= 0)
            {
                // TODO: end the game / bring up a game over UI
                Remove(null);
            }

        }
    }



}
