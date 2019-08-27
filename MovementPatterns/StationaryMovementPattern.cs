namespace out_and_back.MovementPatterns
{
    /// <summary>
    /// A pattern for entities that don't move at all.
    /// </summary>
    class StationaryMovementPattern : ParameterizedMovementPattern
    {
        public StationaryMovementPattern(Entity parent) : base(parent)
        {
            XParam = (float time) => { return origin.X; };
            YParam = (float time) => { return origin.Y; };
            // Make the update function even faster.
            paused = true;
        }
    }
}
