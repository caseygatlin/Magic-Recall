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
            //Move toward the target entity (speed * delta) * (difference/distance)
            return speed * deltaTime / 1000  * (target.Position - current_position) / Vector2.Distance(target.Position, current_position);
        }

        private void OnTargetRemoved(object sender, EventArgs e)
        {
            CompleteMovement(null);
        }
    }
}
