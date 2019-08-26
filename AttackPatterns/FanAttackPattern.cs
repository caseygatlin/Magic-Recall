using System.Collections.Generic;

namespace out_and_back.AttackPatterns
{
    /// <summary>
    /// An attack pattern that causes attacks to be created in a circle around the attacker.
    /// </summary>
    class FanAttackPattern : AttackPattern
    {
        float angleDiff;
        float angleOffset;
        int count;

        /// <summary>
        /// Creates an attack pattern that shoots n attacks, evenly spaced, in a circle around the entity.
        /// </summary>
        /// <param name="parent">The enemy launching the attack.</param>
        /// <param name="count">The number of projectiles to create.</param>
        /// <param name="angleOffset">The offset, in radians, that the attack should go in. Defaults to zero.</param>
        /// <param name="start">How long in milliseconds the entity should wait before creating attacks. See <see cref="AttackPattern.DEFAULT_START_TIME"/>.</param>
        /// <param name="wait">How long in milliseconds the entity should wait before attacking again. See <see cref="AttackPattern.DEFAULT_WAIT_TIME"/>.</param>
        /// <param name="projSpd">How fast the created projectiles should be moving. See <see cref="AttackPattern.DEFAULT_PROJECTILE_SPEED"/>.</param>
        /// <param name="projRad">The radius of the projectile. See <see cref="AttackPattern.DEFAULT_PROJECTILE_RADIUS"/>.</param>
        public FanAttackPattern(Enemy parent, int count, float? angleOffset, int? start, int? wait, float? projSpd, int? projRad) : base(parent, start, wait, projSpd, projRad)
        {
            angleDiff = Microsoft.Xna.Framework.MathHelper.TwoPi / count;
            this.count = count;
            this.angleOffset = angleOffset ?? 0;
        }

        protected override IEnumerable<Projectile> MakeProjectiles()
        {
            List<Projectile> projectiles = new List<Projectile>();
            float angle = angleOffset;
            for (int i = 0; i < count; ++i)
            {
                Projectile p = new Projectile((Game1)parent.Game, parent.Team, angle, projSpd, parent.Position, projRad,-1, Projectile.ProjectileType.GHOST_FLAME);
                angle += angleDiff;
                projectiles.Add(p);
            }

            return projectiles;
        }
    }
}
