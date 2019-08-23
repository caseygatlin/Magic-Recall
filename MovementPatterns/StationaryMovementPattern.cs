namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// A pattern for entities that don't move at all.
    /// </summary>
    class StationaryMovementPattern : MovementPattern
    {
        public StationaryMovementPattern(Entity parent) : base(parent)
        {
            XParam = (int time) => { return origin.X; };
            YParam = (int time) => { return origin.Y; };
            // Make the update function even faster.
            paused = true;
        }
    }
}
