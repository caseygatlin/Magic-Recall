using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace out_and_back
{

    class Player : Entity
    {
        const int PLAYER_RADIUS = 30;
        const int PLAYER_WEAPON_RADIUS = 10;
        public int health;
        public bool isCasting = false;
        public int invincibility_time = 0;
        private const int WEP_SPAWN_DIST = 50;

        /// <summary>
        /// Basic constructor for a player entity.
        /// </summary>
        /// <param name="game">The game this player belongs to.</param>
        /// <param name="direction">The direction the player starts in.</param>
        /// <param name="position">The starting position of the player.</param>
        /// <param name="speed">The speed the player starts at, defaults to zero.</param>
        /// <param name="team">The team this player belongs to.</param>
        public Player(Game1 game, float direction, Vector2 position, float speed = 0, Team team = Team.Player) : base(game, team, direction, speed, position, PLAYER_RADIUS)
        {
            health = Globals.MAX_PLAYER_HEALTH;
        }

        //Detects input from WASD and assigns speed and direction
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

        //Keeps the player on the bounds of the viewport
        private void KeepOnScreen()
        {
            float bufferLR = (float)AssetManager.Instance.playerSprite.Width / 2;
            float bufferUD = (float)AssetManager.Instance.playerSprite.Height / 2;
            float screenHeight = GraphicsDevice.Viewport.Height;
            float screenWidth = GraphicsDevice.Viewport.Width;

            if (Position.X - bufferLR <= 0)
                Position = new Vector2(bufferLR, Position.Y);
            if (Position.Y + bufferUD >= screenHeight)
                Position = new Vector2(Position.X, screenHeight - bufferUD);
            if (Position.X + bufferLR >= screenWidth)
                Position = new Vector2(screenWidth - bufferLR, Position.Y);
            if (Position.Y - bufferUD <= 0)
                Position = new Vector2(Position.X, bufferUD);
        }

        //Throws out the attack
        private void CastWeapon(float mouseDirection, Vector2 playerPos)
        {
            Projectile weapon = new Projectile((Game1)Game, Team.Player, mouseDirection, Globals.MAX_WEAPON_SPEED, playerPos, PLAYER_WEAPON_RADIUS);
            weapon.AddPattern(MovementPatterns.MovementPattern.YoyoFollow(weapon));
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
            //Enable if you want player stationary while attacking
            /*
            if (!isCasting)
                MovementInput();
            else
                Speed = 0;
            */

            //Enable if you want player to move while attacking
            MovementInput();

            KeepOnScreen();

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && isCasting == false)
            {
                Game1 g = (Game1)Game;
                Vector2 mousePos = new Vector2(Mouse.GetState().X / g.Scale.X, Mouse.GetState().Y / g.Scale.Y);
                float castDirection = Globals.getDirection(Position, mousePos);

                float castPosMultX = (float)Math.Cos(castDirection);
                float castPosMultY = (float)Math.Sin(castDirection);
                float castPosX = WEP_SPAWN_DIST * castPosMultX;
                float castPosY = WEP_SPAWN_DIST * castPosMultY;
                Vector2 castPos = new Vector2(castPosX + Position.X, castPosY + Position.Y);

                CastWeapon(castDirection, castPos);
                isCasting = true;
            }


            //Draw the player
            //TODO: Remove once the real wizard invincibility sprite is in
            if (isInvincible())
            {
                AssetManager.Instance.DrawSprite(this, AssetManager.Instance.playerInvincibleSprite);
                AssetManager.Instance.DrawRectangle(new Rectangle((int)Position.X-10, (int)Position.Y-10, 20, 20), Color.Red);
            }
            else
                AssetManager.Instance.DrawSprite(this, AssetManager.Instance.playerSprite);
            //TODO: Use this instead of all the stuff above
            //AssetManager.Instance.DrawSprite(this, isInvincible ? AssetManager.Instance.playerInvincibleSprite : AssetManager.Instance.playerSprite);

            //Draw the player's health
            AssetManager.Instance.DrawRectangle(new Rectangle(Globals.SCREEN_WIDTH - 88, 8, 80, 40), Color.Black);
            AssetManager.Instance.DrawSprite(new Vector2(Globals.SCREEN_WIDTH - 90, 10), AssetManager.Instance.playerHealthIconSprite, 0.1f);
            AssetManager.Instance.PrintString("x" + health, new Vector2(Globals.SCREEN_WIDTH - 45, 20), Color.White);
        }

        public override void HandleCollision(Entity other)
        {
            //If invincible, return.
            if (isInvincible()) return;

            // It hits an entity on the same team, return.
            if (Team == other.Team) return;

            // It's hit an entity either enemy or projectile - and should therefore lose health.
            health--;
            if (health <= 0)
            {
                // TODO: end the game / bring up a game over UI
                Remove(null);
            }
            else
            {
                invincibility_time = Globals.INVINCIBILITY_DURATION;
            }
        }

        public bool isInvincible()
        {
            return invincibility_time > 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (isInvincible())
            {
                invincibility_time = Math.Max(invincibility_time - gameTime.ElapsedGameTime.Milliseconds, 0);
            }
        }
    }
}
