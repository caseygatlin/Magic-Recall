using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace out_and_back.AttackPatterns
{
    abstract class AttackPattern
    {
        /// <summary>
        /// The default amount of time an entity waits until it begins attacking in milliseconds.
        /// </summary>
        const int DEFAULT_START_TIME = 0;
        /// <summary>
        /// The default amount of time an entity waits to attack again in milliseconds.
        /// </summary>
        const int DEFAULT_WAIT_TIME = 1000;

        /// <summary>
        /// The default projectile speed.
        /// </summary>
        const float DEFAULT_PROJECTILE_SPEED = 50;

        const float DEFAULT_PROJECTILE_RADIUS = 10;

        protected int start;
        protected int wait;
        protected float projSpd;
        protected float projRad;
        private bool started = false;
        protected Enemy parent { get; private set; }

        protected int time;

        /// <summary>
        /// Creates an attack pattern.
        /// </summary>
        /// <param name="parent">The entity doing the attacking.</param>
        /// <param name="start">How long after the pattern starts the attack should begin in milliseconds. Defaults to 0.</param>
        /// <param name="wait">How long after the attack the entity should wait before the attack happens again in milliseconds. Defaults to 1000.</param>
        /// <param name="projSpd">How fast the attack projectiles should be moving. Defaults to 50.</param>
        /// <param name="projRad">The projectile radius. Defaults to 10.</param>
        public AttackPattern(Enemy parent, int? start, int? wait, float? projSpd, int? projRad)
        {
            this.parent = parent;
            this.start = start ?? DEFAULT_START_TIME;
            this.wait = wait ?? DEFAULT_WAIT_TIME;
            this.projSpd = projSpd ?? DEFAULT_PROJECTILE_SPEED;
            this.projRad = projRad ?? DEFAULT_PROJECTILE_RADIUS;
        }

        public virtual void Update(GameTime elapsedTime)
        {
            time += elapsedTime.ElapsedGameTime.Milliseconds;
            if (time >= start && started == false)
            {
                GenerateProjectiles();
                started = true;
                time = 0;
            }
            else if (time >= wait)
            {
                GenerateProjectiles();
                time = 0;
            }
        }

        private void GenerateProjectiles()
        {
            foreach (Projectile proj in MakeProjectiles())
                ((Game1)parent.Game).Entities.Add(proj);
            CreateProjectiles(new EventArgs());
        }

        protected abstract IEnumerable<Projectile> MakeProjectiles();

        public event EventHandler ProjectilesCreated;

        protected virtual void CreateProjectiles(EventArgs e)
        {
            var handler = ProjectilesCreated;
            handler?.Invoke(this, e);
        }
    }
}
