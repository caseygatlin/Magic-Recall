using Microsoft.Xna.Framework;
using System;

namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// Causes the entity to move toward a target entity.
    /// </summary>
    class PursueEntityPattern : DeltaMovementPattern
    {
        private Entity target;
        internal PursueEntityPattern(Entity parent, Entity target) : base(parent)
        {
            this.target = target;
            target.Removed += OnTargetRemoved;
        }

        protected override Vector2 ComputeDelta(int deltaTime)
        {
            //Move toward the target entity
            float distance = Vector2.Distance(target.Position, current_position);
            float composite_speed = speed * deltaTime / 1000 / distance;
            float delta_x = composite_speed * (target.Position.X - current_position.X);
            float delta_y = composite_speed * (target.Position.Y - current_position.Y);

            //Failsafe: if you've reach the target exactly, stop
            if (target.Position == current_position)
            {
                CompleteMovement(null);
                return Vector2.Zero;
            }
            return new Vector2(delta_x, delta_y);
        }

        private void OnTargetRemoved(object sender, EventArgs e)
        {
            CompleteMovement(null);
        }
    }
}
