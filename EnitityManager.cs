using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace out_and_back
{
    class EnitityManager : DrawableGameComponent
    {
        public Player Player { get; private set; }
        LinkedList<Projectile> playerAttacks = new LinkedList<Projectile>();

        LinkedList<Entity> enemies = new LinkedList<Entity>();
        LinkedList<Projectile> enemyAttacks = new LinkedList<Projectile>();
        LinkedList<Entity> removalQueue = new LinkedList<Entity>();

        bool updating = false;

        public EnitityManager(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public void Add(Entity e)
        {
            switch(e)
            {
                case Projectile p:
                    if (p.Team == Team.Player)
                        playerAttacks.AddLast(p);
                    else
                        enemyAttacks.AddLast(p);
                    break;
                case Player pl:
                    Player = pl;
                    break;
                default:
                    enemies.AddLast(e);
                    break;
            }
        }

        public void RemoveEntity(Entity e)
        {
            if (updating)
            {
                removalQueue.AddLast(e);
                return;
            }
            switch(e)
            {
                case Projectile p:
                    if (p.Team == Team.Player)
                        playerAttacks.Remove(p);
                    else
                        enemyAttacks.Remove(p);
                    break;
                case Player pl:
                    Player = null;
                    break;
                default:
                    enemies.Remove(e);
                    break;
            }
        }

        private void AddEnemy(Entity enemy)
        {
            enemies.AddLast(enemy);
        }

        private void AddEnemyAttack(Projectile projectile)
        {
            enemyAttacks.AddLast(projectile);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Player?.Draw(gameTime);
            foreach (var patk in playerAttacks) patk.Draw(gameTime);
            foreach (var enemy in enemies) enemy.Draw(gameTime);
            foreach (var proj in enemyAttacks) proj.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            updating = true;
            base.Update(gameTime);
            Player?.Update(gameTime);
            foreach (var patk in playerAttacks) patk.Update(gameTime);
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime);
                Collider.DoCollide(enemy, Player);
                foreach (var patk in playerAttacks)
                    Collider.DoCollide(enemy, patk);
            }
            foreach (var proj in enemyAttacks)
            {
                proj.Update(gameTime);
                Collider.DoCollide(proj, Player);
            }
            updating = false;
            foreach (var entity in removalQueue)
                RemoveEntity(entity);
            if (removalQueue.Count > 0) removalQueue.Clear();
        }
    }
}
