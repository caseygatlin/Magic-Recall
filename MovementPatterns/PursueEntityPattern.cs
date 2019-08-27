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

        public override void UpdateCurrentPosition(int deltaTime)
        {
            //Move toward the target entity
            float distance = Vector2.Distance(target.Position, current_position);
            float composite_speed = speed * deltaTime / 1000 / distance;
            current_position.X += composite_speed * (target.Position.X - current_position.X);
            current_position.Y += composite_speed * (target.Position.Y - current_position.Y);

            //Failsafe: if you reach the target exactly, stop
            if (target.Position == current_position)
                CompleteMovement(null);
        }

        private void OnTargetRemoved(object sender, EventArgs e)
        {
            CompleteMovement(null);
        }
    }
}
