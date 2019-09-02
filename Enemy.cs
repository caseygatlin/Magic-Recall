using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using out_and_back.MovementPatterns;
using System.Linq;

namespace out_and_back
{
    /// <summary>
    /// Represents an enemy in the game.
    /// </summary>
    class Enemy : MovementManagedEntity
    {
        AttackPatterns.AttackPattern attackPattern;
        Texture2D sprite;
        public bool CanExtinguishBraziers { get; private set; }
        private Game1 game;

        /// <summary>
        /// Basic constructor for an enemy entity.
        /// </summary>
        /// <param name="game">The game this enemy belongs to.</param>
        /// <param name="team">The team this enemy belongs to.</param>
        /// <param name="direction">The direction the enemy is moving in.</param>
        /// <param name="speed">The speed the enemy is moving at.</param>
        /// <param name="position">The starting position of the enemy.</param>
        /// <param name="radius">The hitbox radius of the enemy.</param>
        internal Enemy(Game1 game, Team team, float direction, float speed, Vector2 position, float radius) : base(game, team, direction, speed, position, radius)
        {
            CanExtinguishBraziers = false;
            this.game = game;
        }

        //Ghost enemy. Moves in a straight line and shoots a small spread of bullets.
        public static Enemy Ghost(Game1 game, float direction, Vector2 position)
        {
            Enemy g = new Enemy(game, Team.Enemy, direction, 100, position, 30);
            g.AddPattern(MovementPattern.Straight(g));
            g.SetAttackPattern(new AttackPatterns.FanAttackPattern(g, 2, direction + MathHelper.PiOver2, 0, null, null, null, Projectile.ProjectileType.GHOST_FLAME));
            g.sprite = AssetManager.Instance.ghostSprite;
            return g;
        }


        //Spider enemy. Pursues the player.
        public static Enemy Spider(Game1 game, float direction, Vector2 position)
        {
            Enemy s = new Enemy(game, Team.Enemy, direction, 80, position, 30);
            s.AddPattern(MovementPattern.PursueEntity(s, game.Player));
            s.sprite = AssetManager.Instance.spiderSprite;
            return s;
        }

        //Slime enemy. Pursues the nearest brazier.
        public static Enemy Slime(Game1 game, float direction, Vector2 position)
        {
            Enemy s = new Enemy(game, Team.Enemy, direction, 50, position, 30);
            Entity target_brazier = game.Entities.Braziers.First();
            foreach(Entity brazier in game.Entities.Braziers)
            {
                if(Vector2.Distance(position, brazier.Position) < Vector2.Distance(position, target_brazier.Position))
                {
                    target_brazier = brazier;
                }
            }
            s.AddPattern(MovementPattern.PursueEntity(s, target_brazier));
            s.sprite = AssetManager.Instance.slimeSprite;
            s.CanExtinguishBraziers = true;
            return s;
        }

        public static Enemy Eye(Game1 game, float direction, Vector2 position)
        {
            Enemy e = new Enemy(game, Team.Enemy, direction, 25, position, 30);
            var eyeAttackPattern = new AttackPatterns.FanAttackPattern(e, 3, null, null, 2000, null, null, Projectile.ProjectileType.EYE_BLAST);
            eyeAttackPattern.SetMovementPattern(MovementPattern.Spiral);
            e.SetAttackPattern(eyeAttackPattern);
            e.AddPattern(MovementPattern.Straight(e, 400));
            e.AddPattern(MovementPattern.Stationary(e));
            e.AddPattern(MovementPattern.Straight(e));
            e.sprite = AssetManager.Instance.eyeSprite;
            return e;
        }

        public override void Update(GameTime gameTime)
        {
            if (game.paused)
                return;
            base.Update(gameTime);
            if (attackPattern != null)
                attackPattern.Update(gameTime);

        }

        protected override void Move(int deltaTime)
        {
            base.Move(deltaTime);

            //If an enemy goes waaaay out of bounds, we should kill it so it doesn't run away forever
            if (Position.X > Globals.SCREEN_WIDTH * 2 || Position.X < -Globals.SCREEN_WIDTH
                || Position.Y > Globals.SCREEN_HEIGHT * 2 || Position.Y < -Globals.SCREEN_HEIGHT)
            {
                Dispose();
                Remove(null);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //AssetManager.Instance.PrintString("^_^", Position, Team == Team.Enemy ? Color.Red : Color.Blue);
            if (sprite != null)
                AssetManager.Instance.DrawSprite(this, sprite);
            else
                AssetManager.Instance.PrintString("^_^", Position, Team == Team.Enemy ? Color.Red : Color.Blue);

        }

        public override void HandleCollision(Entity other)
        {
            // Ignore objects of the same team.
            if (Team == other.Team) return;

            //Ignore invincible players
            if (other is Player && ((Player)other).IsInvincible()) return;

            //Die on player or projectiles from the opposite team
            if (other is Player || (other is Projectile && other.Team != Team))
            {
                if (other is Projectile playerProjectile)
                {
                    ((Game1)Game).Player.IncreaseRange();
                }
                Remove(null);
            }
        }

        /// <summary>
        /// Sets this enemy's attack pattern.
        /// </summary>
        /// <param name="pattern">The attack pattern to use.</param>
        private void SetAttackPattern(AttackPatterns.AttackPattern pattern)
        {
            attackPattern = pattern;
        }
    }
}
