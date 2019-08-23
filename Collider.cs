namespace out_and_back
{
    class Collider
    {
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
