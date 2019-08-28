
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace out_and_back
{
    /// <summary>
    /// A collective set of enemies to spawn all at once and how long the waves should wait for this one.
    /// </summary>
    internal class Wave
    {
        public int duration;//In seconds
        public LinkedList<Spawn> spawns;
        public Wave(int duration)
        {
            this.duration = duration;
            spawns = new LinkedList<Spawn>();
        }
    }
    /// <summary>
    /// Tracks what to spawn and where.
    /// </summary>
    internal class Spawn
    {
        public Func<Game1, float, Vector2, Enemy> what;
        public float direction;
        public Vector2 position;

        public Spawn(Func<Game1, float, Vector2, Enemy> what, float direction, Vector2 position)
        {
            this.what = what;
            this.direction = direction;
            this.position = position;
        }
    }

    /// <summary>
    /// Spawns enemies over time and shows how many have spawned.
    /// </summary>
    class Level : DrawableGameComponent
    {
        protected Game1 game;
        protected LinkedList<Wave> waves;
        protected int spawned_and_removed = 0;
        protected int current_wave_timer = 0;
        protected int total_to_spawn;
        protected const int OFF_SCREEN_OFFSET = 100;
        protected Level(Game1 game, LinkedList<Wave> waves) : base(game)
        {
            this.game = game;
            this.waves = waves;
            total_to_spawn = waves.Sum(x => x.spawns.Count());
        }
        public static Level Level1(Game1 game)
        {
            //Set up each wave of spawns
            LinkedList<Wave> waves = new LinkedList<Wave>();

            //Wave 0: Give the player a little bit of time to orient themselves
            waves.AddLast(new Wave(5));

            //Wave 1: A single slime
            waves.AddLast(new Wave(8));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH / 2, -OFF_SCREEN_OFFSET)));

            //Wave 2: Three slimes!
            waves.AddLast(new Wave(10));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH / 2, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT / 2)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT / 2)));

            //Wave 3: A single ghost down the middle
            waves.AddLast(new Wave(10));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH/2, -OFF_SCREEN_OFFSET)));

            //Wave 4: Two ghosts from left and right
            waves.AddLast(new Wave(8));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT*1/3)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 2 / 3)));

            //Wave 5: Two slimes from the top and a ghost across the bottom
            waves.AddLast(new Wave(8));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH / 2, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 6 / 7)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 1 / 3)));

            //Wave 6: Four ghosts from all around!
            waves.AddLast(new Wave(10));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, -OFF_SCREEN_OFFSET)));

            return new Level(game, waves);
        }

        public override void Update(GameTime gameTime)
        {
            //Tick down on the current wave's duration; once it's <0, spawn the next wave
            if (waves.Any())
            {
                current_wave_timer -= gameTime.ElapsedGameTime.Milliseconds;
                if (current_wave_timer < 0)
                {
                    //Get next wave and remove it from the list
                    Wave wave = waves.First.Value;
                    current_wave_timer = wave.duration * 1000;//Duration in seconds, timer in ms
                    waves.RemoveFirst();

                    //Spawn the current wave
                    foreach (Spawn s in wave.spawns)
                    {
                        Enemy e = s.what(game, s.direction, s.position);
                        e.Removed += EnemyRemoved;
                    }
                }
            }
        }
        private void EnemyRemoved(object sender, EventArgs e)
        {
            spawned_and_removed = MathHelper.Clamp(spawned_and_removed + 1, 0, total_to_spawn);
        }
        public override void Draw(GameTime gameTime)
        {
            AssetManager.Instance.DrawRectangle(new Rectangle(8, 8, 120, 20), Color.White);
            AssetManager.Instance.PrintString("Enemies left: " + (total_to_spawn - spawned_and_removed), new Vector2(10, 10), Color.Black);
        }
    }
}
