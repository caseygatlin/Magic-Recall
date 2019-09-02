
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
        public Func<Game1, float, Vector2, Entity> what;
        public float direction;
        public Vector2 position;

        public Spawn(Func<Game1, float, Vector2, Entity> what, float direction, Vector2 position)
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

        internal int EnemiesLeft()
        {
            return total_to_spawn - spawned_and_removed;
        }

        protected Level(Game1 game, LinkedList<Wave> waves) : base(game)
        {
            this.game = game;
            this.waves = waves;

            total_to_spawn = waves.Skip(1).Sum(x => x.spawns.Count());//Wave 0 spawns only terrain features, waves 1+ spawn only enemies
        }
        public static Level Level1(Game1 game)
        {
            //Set up each wave of spawns
            LinkedList<Wave> waves = new LinkedList<Wave>();

            //Wave 0: Give the player a little bit of time to orient themselves; also add terrain features
            waves.AddLast(new Wave(5));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Crystal, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH / 2 - 100, Globals.SCREEN_HEIGHT / 2)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Crystal, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH / 2 + 100, Globals.SCREEN_HEIGHT / 2)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Crystal, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH / 2, Globals.SCREEN_HEIGHT / 2 - 100)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Crystal, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH / 2, Globals.SCREEN_HEIGHT / 2 + 100)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * 1 / 6, Globals.SCREEN_HEIGHT * 1 / 6)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * 5 / 6, Globals.SCREEN_HEIGHT * 1 / 6)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * 1 / 6, Globals.SCREEN_HEIGHT * 5 / 6)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * 5 / 6, Globals.SCREEN_HEIGHT * 5 / 6)));

            //Wave 1: A single slime
            waves.AddLast(new Wave(8));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH / 2, -OFF_SCREEN_OFFSET)));

            //Wave 2: Three slimes!
            waves.AddLast(new Wave(10));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH / 2, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT / 2)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT / 2)));

            //Wave 3: A single ghost down the middle
            waves.AddLast(new Wave(10));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH / 2, -OFF_SCREEN_OFFSET)));

            //Wave 4: Two ghosts from left and right
            waves.AddLast(new Wave(8));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 1 / 3)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 2 / 3)));

            //Wave 5: Two slimes from the top and a ghost across the bottom
            waves.AddLast(new Wave(8));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH / 2, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 6 / 7)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 1 / 3)));

            //Wave 6: Four ghosts from all around!
            waves.AddLast(new Wave(10));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, -OFF_SCREEN_OFFSET)));

            // Wave 7: Eyes see you
            //waves.AddLast(new Wave(5));
            //waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Eye, 0, new Vector2(0, 100)));

            return new Level(game, waves);
        }

        public static Level Level2(Game1 game)
        {
            //Set up each wave of spawns
            LinkedList<Wave> waves = new LinkedList<Wave>();

            //Wave 0: Give the player a little bit of time to orient themselves; also add terrain features
            waves.AddLast(new Wave(4));
            waves.Last.Value.spawns.AddLast(new Spawn(Brazier.BrazierRange, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .2f, Globals.SCREEN_HEIGHT * .3f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Brazier.BrazierTriple, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .75f, Globals.SCREEN_HEIGHT * .65f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .30f, Globals.SCREEN_HEIGHT * .30f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .20f, Globals.SCREEN_HEIGHT * .45f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .15f, Globals.SCREEN_HEIGHT * .45f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .10f, Globals.SCREEN_HEIGHT * .45f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .60f, Globals.SCREEN_HEIGHT * .20f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .60f, Globals.SCREEN_HEIGHT * .25f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .60f, Globals.SCREEN_HEIGHT * .30f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .60f, Globals.SCREEN_HEIGHT * .35f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .35f, Globals.SCREEN_HEIGHT * .60f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .35f, Globals.SCREEN_HEIGHT * .65f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .35f, Globals.SCREEN_HEIGHT * .70f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .35f, Globals.SCREEN_HEIGHT * .75f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .75f, Globals.SCREEN_HEIGHT * .50f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .80f, Globals.SCREEN_HEIGHT * .50f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .85f, Globals.SCREEN_HEIGHT * .50f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Tree, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .65f, Globals.SCREEN_HEIGHT * .65f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Crystal, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .3f, Globals.SCREEN_HEIGHT * .35f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Crystal, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .25f, Globals.SCREEN_HEIGHT * .4f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Crystal, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .70f, Globals.SCREEN_HEIGHT * .55f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Obstacle.Crystal, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .65f, Globals.SCREEN_HEIGHT * .60f)));

            //Wave 1: Four spiders from NE; pushes toward south-center and away from triple brazier
            waves.AddLast(new Wave(2));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH - OFF_SCREEN_OFFSET, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, 0)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, OFF_SCREEN_OFFSET)));

            //Wave 2: Four more spiders from S; pushes further away from triple; lots of spiders builds range a bit
            waves.AddLast(new Wave(2));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * .4f, Globals.SCREEN_HEIGHT + 1.3f * OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * .5f, Globals.SCREEN_HEIGHT + 1.3f * OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * .6f, Globals.SCREEN_HEIGHT + 1.2f * OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * .7f, Globals.SCREEN_HEIGHT + 1.0f * OFF_SCREEN_OFFSET)));

            //Wave 3: A single slime sneaking in while the spiders approach; pushes player to light triple brazier by accident
            waves.AddLast(new Wave(12));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.UP_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH * .8f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));

            //Wave 4: Tight cluster of ghosts in reverse chevron to demonstrate triple brazier; push from triple brazier
            waves.AddLast(new Wave(8));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .4f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .6f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + 1.5f * OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .45f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + 1.5f * OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .55f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + 2.0f * OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .5f)));

            //Wave 6: Slime rush on triple brazier with distractions; the player will lose the triple and want to relight it
            waves.AddLast(new Wave(10));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.UP_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH * .8f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * .6f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * .8f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .6f, Globals.SCREEN_HEIGHT - OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .8f, Globals.SCREEN_HEIGHT - OFF_SCREEN_OFFSET)));

            //Wave 7: Slime rush on range brazier to ensure they haven't forgotten about it
            waves.AddLast(new Wave(12));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .15f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.DOWN_RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET * .8f, Globals.SCREEN_HEIGHT * .25f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.DOWN_RIGHT_DIR, new Vector2(Globals.SCREEN_WIDTH * .15f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.DOWN_RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET * .8f, Globals.SCREEN_HEIGHT * .25f)));

            //Wave 8: Eight ghosts to push player towards the middle
            waves.AddLast(new Wave(12));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .15f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .85f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .15f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .85f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .15f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .85f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * .15f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * .85f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));

            //Wave 9: Assault on both braziers at once!
            waves.AddLast(new Wave(15));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.DOWN_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH * .8f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .85f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.DOWN_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH * .7f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.DOWN_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH * .9f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.UP_RIGHT_DIR, new Vector2(Globals.SCREEN_WIDTH * .2f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .15f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.UP_RIGHT_DIR, new Vector2(Globals.SCREEN_WIDTH * .1f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.UP_RIGHT_DIR, new Vector2(Globals.SCREEN_WIDTH * .8f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));

            //Wave 10: Two eyes?!?
            waves.AddLast(new Wave(20));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Eye, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT / 2)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Eye, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT / 2)));

            //Wave 11: Heavy attack on range brazier, and not much time
            waves.AddLast(new Wave(10));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .0f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .1f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .2f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .3f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .4f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .0f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .2f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .4f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .6f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .8f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET - 50, -OFF_SCREEN_OFFSET + 50)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET - 100, -OFF_SCREEN_OFFSET + 100)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET + 50, -OFF_SCREEN_OFFSET - 50)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET + 100, -OFF_SCREEN_OFFSET - 100)));

            //Wave 12: Heavy attack on triple brazier
            waves.AddLast(new Wave(10));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 0.6f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 0.7f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 0.8f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 0.9f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 1.0f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * 0.2f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * 0.4f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * 0.6f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * 0.8f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * 1.0f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET - 50, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET + 50)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET - 100, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET + 100)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET + 50, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET - 50)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET + 100, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET - 100)));

            //Wave 13: The end.
            waves.AddLast(new Wave(60));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Eye, Globals.DOWN_RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Eye, Globals.UP_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Eye, Globals.UP_RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Eye, Globals.DOWN_LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .0f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .1f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .2f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .3f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .4f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 0.6f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 0.7f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 0.8f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 0.9f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Slime, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * 1.0f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .15f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .50f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.LEFT_DIR, new Vector2(Globals.SCREEN_WIDTH + OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .85f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .15f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .50f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.RIGHT_DIR, new Vector2(-OFF_SCREEN_OFFSET, Globals.SCREEN_HEIGHT * .85f)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .15f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .40f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .60f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.DOWN_DIR, new Vector2(Globals.SCREEN_WIDTH * .85f, -OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * .15f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * .40f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * .60f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Ghost, Globals.UP_DIR, new Vector2(Globals.SCREEN_WIDTH * .85f, Globals.SCREEN_HEIGHT + OFF_SCREEN_OFFSET)));
            for (int i = 0; i < 32; i++)
            {
                float angle = i * MathHelper.TwoPi / 32;
                Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                Vector2 location = Globals.SCREEN_HEIGHT * direction + new Vector2(Globals.SCREEN_WIDTH / 2, Globals.SCREEN_HEIGHT / 2);
                waves.Last.Value.spawns.AddLast(new Spawn(Enemy.Spider, angle, location));
            }

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
                        Entity e = s.what(game, s.direction, s.position);
                        if (e is Enemy)
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
            AssetManager.Instance.DrawRectangle(new Rectangle(8, 8, 220, 40), Color.Black);
            AssetManager.Instance.PrintString("Enemies left: " + (total_to_spawn - spawned_and_removed), new Vector2(15, 20), Color.White);
        }
    }
}
