using System;
using Microsoft.Xna.Framework;

namespace out_and_back
{
    /// <summary>
    /// An abstract entity.
    /// </summary>
    abstract class Entity : DrawableGameComponent
    {
        Vector2 movement;
        internal Game1 currentGame;

        public Team Team { get; private set; }

        /// <summary>
        /// The default event handler that should be called 
        /// </summary>
        public static EventHandler DefaultRemovalEvent;

        private bool shouldDespawn;
        /// <summary>
        /// Whether or not this entity should be removed from the game.
        /// </summary>
        public bool ShouldDespawn
        {
            get => shouldDespawn;
            protected set => shouldDespawn = value;
        }

        protected float hitRadius = 10;

        /// <summary>
        /// Creates an entity with the specified direction and speed values and the given position.
        /// </summary>
        /// <param name="game">The game instance that this entity belongs to.</param>
        /// <param name="team">The team for the entity (good or bad).</param>
        /// <param name="direction">The direction the entity should be moving in.</param>
        /// <param name="speed">The speed of the entity.</param>
        /// <param name="position">The position on the world map where the entity is.</param>
        /// <param name="radius">The hitbox radius of the entity.</param>
        public Entity(Game1 game, Team team, float direction, float speed, Vector2 position, float radius) : base(game)
        {
            Team = team;
            Direction = direction;
            Speed = speed;
            Position = position;
            hitRadius = radius;
            game.Entities.Add(this);
            currentGame = game;
            Removed += DefaultRemovalEvent;
        }

        /// <summary>
        /// The direction this entity is facing/moving, stored in radians.
        /// </summary>
        public float Direction
        {
            get => movement.X;
            set => movement.X = value;
        }

        /// <summary>
        /// The speed of this entity.
        /// </summary>
        public float Speed
        {
            get => movement.Y;
            set => movement.Y = value;
        }

        /// <summary>
        /// The position of this entity. It can only be set initially in the
        /// constructor, and is modified within the update function.
        /// </summary>
        public Vector2 Position
        {
            get; protected set;
        }

        /// <summary>
        /// Updates the entity and moves it. You may want to have any derived
        /// class that overrides this update method to call this update method
        /// as well, as it helps with movement.
        /// </summary>
        /// <param name="gameTime">The time tracking sent by the game's update loop.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Move(gameTime.ElapsedGameTime.Milliseconds);
        }

        /// <summary>
        /// Causes the entity to move. By default, this uses the entitiy's
        /// orientation and speed values (movement.x and movement.y, 
        /// respectively).
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last frame.
        /// This should be obtained from gameTime.ElapsedGameTime.Milliseconds.</param>
        protected virtual void Move(int deltaTime)
        {
            Vector2 deltas = new Vector2(
                Speed * (float)Math.Cos(Direction) * deltaTime / 1000,
                Speed * (float)Math.Sin(Direction) * deltaTime / 1000
            );
            Position += deltas;
        }

        /// <summary>
        /// Checks if this entity is colliding with another.
        /// </summary>
        /// <param name="other">The other entity to check against.</param>
        public bool CheckCollision(Entity other)
        {
            return Vector2.Distance(Position, other.Position) <= hitRadius + other.hitRadius;
        }

        /// <summary>
        /// Has this unit determine what it should do when it collides with 
        /// another entity. This code should not call the other entity's
        /// HandleCollision method, as during the game loop, the other entity
        /// will find this unit as intersecting with it. It should only define
        /// the behavior of intersecting with that other entity.
        /// </summary>
        /// <param name="other">The other entity colliding with this object.</param>
        public abstract void HandleCollision(Entity other);
        
        /// <summary>
        /// This is triggered when some event causes this entity to be removed
        /// from the game.
        /// </summary>
        public event EventHandler Removed;

        protected virtual void Remove(EventArgs e)
        {
            // If the unit is already marked for despawning, don't fire the
            // event again
            if (shouldDespawn)
                return;
            // Mark that the unit is despawning
            shouldDespawn = true;
            EventHandler handler = Removed;
            handler?.Invoke(this, e);
        }
    }
}
