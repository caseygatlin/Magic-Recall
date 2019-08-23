namespace out_and_back
{
    /// <summary>
    /// Utility class to check if two entities collide.
    /// </summary>
    class Collider
    {
        /// <summary>
        /// Checks if any two entities collide, and calls their respective
        /// collision functions.
        /// </summary>
        /// <param name="a">Entity a</param>
        /// <param name="b">Entity b</param>
        /// <returns>True if they do collide.</returns>
        public static bool DoCollide(Entity a, Entity b)
        {
            if (a == null || b == null) return false;
            if (a.CheckCollision(b))
            {
                a.HandleCollision(b);
                b.HandleCollision(a);
                return true;
            }
            return false;
        }
    }
}
