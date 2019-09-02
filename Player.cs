using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace out_and_back
{

    class Player : Entity
    {
        private LinkedList<Vector2> positionsList = new LinkedList<Vector2>();
        const int PLAYER_RADIUS = 30;
        const int PLAYER_WEAPON_RADIUS = 10;
        public int health;
        public bool IsCasting
        {
            get => primaryProjectile != null;
        }
        public int invincibility_time = 0;
        private const int WEP_SPAWN_DIST = 50;
        private const int DEFAULT_RANGE = 50;
        private const float DEFAULT_WEAPON_SPEED = Globals.MAX_WEAPON_SPEED;
        private float rangeModifier = 1;
        private Projectile primaryProjectile = null;

        private const int BONUS_FIREBALL_COUNT = 3;
        private const int NORMAL_FIREBALL_COUNT = 1;
        private int castAmount = NORMAL_FIREBALL_COUNT;

        /// <summary>
        /// How far the player's attack can go.
        /// </summary>
        public float AttackRange
        {
            get => DEFAULT_RANGE * (RangeBrazierLit ? rangeModifier : 1);
        }

        /// <summary>
        /// How fast the player's attack goes.
        /// </summary>
        public float AttackSpeed
        {
            get => DEFAULT_WEAPON_SPEED * (RangeBrazierLit ? rangeModifier : 1);
        }

        public bool RangeBrazierLit
        {
            get; private set;
        } = false;
        public bool TripleBrazierLit
        {
            get; private set;
        } = false;

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
            float screenHeight = Globals.SCREEN_HEIGHT;
            float screenWidth = Globals.SCREEN_WIDTH;

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
            for (int i = 0; i < castAmount; ++i)
            {
                float direction = mouseDirection + FanAttackModifier(i);
                
                Projectile weapon = new Projectile((Game1)Game, Team.Player, direction, AttackSpeed, playerPos, PLAYER_WEAPON_RADIUS);
                weapon.AddPattern(MovementPatterns.MovementPattern.YoyoFollow(weapon, AttackRange));
                // Todo: Set this to check the brazier
                if (i == 0)
                {
                    weapon.Removed += Weapon_Removed;
                    primaryProjectile = weapon;
                }
            }
        }

        private float FanAttackModifier(int iteration)
        {
            if (iteration == 0) return 0;
            else return MathHelper.PiOver4 * (iteration % 2 == 0 ? iteration - 1 : -iteration);
        }

        //Allows the player to cast again after the attack is done
        private void Weapon_Removed(object sender, System.EventArgs e)
        {
            primaryProjectile = null;
        }

        //Main drawing loop for the player
        public override void Draw(GameTime gameTime)
        {
            //Draw the player
            AssetManager.Instance.DrawSprite(this, IsInvincible() ? AssetManager.Instance.playerInvincibleSprite : AssetManager.Instance.playerSprite);

            //Draw the player's health
            AssetManager.Instance.DrawRectangle(new Rectangle(Globals.SCREEN_WIDTH - 80, 8, 72, 40), Color.Black);
            AssetManager.Instance.DrawSprite(new Vector2(Globals.SCREEN_WIDTH - 87, 10), AssetManager.Instance.playerHealthIconSprite, 0.1f);
            AssetManager.Instance.PrintString($"x{health}", new Vector2(Globals.SCREEN_WIDTH - 35, 30), Color.White);
        }

        public override void HandleCollision(Entity other)
        {
            //If it's hitting an obstacle
            if (other is Obstacle)
            {
                //Move the player to their last position out of the collision area (a rectangle in this case)
                while (Math.Abs(Position.X - other.Position.X) <= Globals.OBSTACLE_RADIUS && Math.Abs(Position.Y - other.Position.Y) <= Globals.OBSTACLE_RADIUS)
                {
                    positionsList.RemoveFirst();
                    Position = positionsList.First.Value;
                }
                return;
            }

            //If invincible, return.
            if (IsInvincible()) return;

            // It hits an entity on the same team, return.
            if (Team == other.Team) return;

            // It's hit an entity either enemy or projectile - and should therefore lose health.
            health--;
            if (health <= 0)
            {
                Remove(null);
            }
            else
            {
                invincibility_time = Globals.INVINCIBILITY_DURATION;
            }
        }

        public bool IsInvincible()
        {
            return invincibility_time > 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MovementInput();
            KeepOnScreen();

            // Projectile firing code
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && IsCasting == false)
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
            }

            if (IsInvincible())
                invincibility_time = Math.Max(invincibility_time - gameTime.ElapsedGameTime.Milliseconds, 0);

            //Retains a list of the player's positions from the last 10 frames
            if (positionsList.Count < 10)
            {
                positionsList.AddFirst(Position);
            }
            else
            {
                positionsList.RemoveLast();
                positionsList.AddFirst(Position);
            }
        }

        /// <summary>
        /// If the player has the range brazier lit, increases the range the player can
        /// fire by 10% of the base range.
        /// </summary>
        public void IncreaseRange()
        {
            if (RangeBrazierLit)
                rangeModifier += .1f;
        }

        /// <summary>
        /// Toggles whether or not the player can get extra range by defeating enemies.
        /// </summary>
        public void ToggleRangeBrazierLit()
        {
            RangeBrazierLit = !RangeBrazierLit;
        }

        /// <summary>
        /// Causes the player to toggle whether or not they have the Bonus Brazier lit.
        /// </summary>
        public void ToggleTripleBrazierLit()
        {
            TripleBrazierLit = !TripleBrazierLit;
            castAmount = TripleBrazierLit ? BONUS_FIREBALL_COUNT : NORMAL_FIREBALL_COUNT;
        }
    }
}
