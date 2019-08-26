using System.Collections.Generic;

namespace out_and_back.AttackPatterns
{
    /// <summary>
    /// An attack pattern that causes attacks to be created in a circle around the attacker.
    /// </summary>
    class FanAttackPattern : AttackPattern
    {
        float angleDiff;
        int count;

        public FanAttackPattern(Enemy parent, int count, int? start, int? wait, float? projSpd, int? projRad) : base(parent, start, wait, projSpd, projRad)
        {
            angleDiff = Microsoft.Xna.Framework.MathHelper.TwoPi / count;
            this.count = count;
        }

        protected override IEnumerable<Projectile> CreateProjectiles()
        {
            List<Projectile> projectiles = new List<Projectile>();
            float angle = 0;
            for (int i = 0; i < count; ++i)
            {
                Projectile p = new Projectile((Game1)parent.Game, parent.Team, angle, projSpd, parent.Position, projRad);
                angle += angleDiff;
                projectiles.Add(p);
            }

            return projectiles;
        }
    }
}
