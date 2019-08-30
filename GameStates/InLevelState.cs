#define RUN_LEVEL

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace out_and_back.GameStates
{

    class InLevelState : AbstractGameState
    {
        public Level level;
        private Game1 game;
        SoundEffect songPt1;
        SoundEffect songPt2;
        SoundEffectInstance instance;
        private bool playMusic;
        private bool inTransition = false;
        
        //private float time = 0;

        public InLevelState(Game1 currentGame)
        {
            game = currentGame;
            Player = new Player(game, 0, new Vector2(Globals.SCREEN_WIDTH/2, Globals.SCREEN_HEIGHT/2));
#if RUN_LEVEL
            level = Level.Level1(game);
#endif

            songPt1 = game.Content.Load<SoundEffect>("Level1MusPt1");
            songPt2 = game.Content.Load<SoundEffect>("Level1MusPt2");
            instance = songPt1.CreateInstance();
            instance.Play();
            playMusic = game.playMusic;
            inTransition = false;
        }

        public override void Draw(Game1 game, GameTime gameTime)
        {
            AssetManager.Instance.DrawSprite(Vector2.Zero, AssetManager.Instance.background);         

#if RUN_LEVEL
            level.Draw(gameTime);
#endif
        }


        public override void Update(Game1 game, GameTime gameTime)
        {
            playMusic = game.playMusic;
            if (!playMusic)
                instance.Volume = 0f;
            else
                instance.Volume = 0.7f;

            if (instance.State == SoundState.Stopped && !inTransition)
            {
                instance = songPt2.CreateInstance();
                instance.IsLooped = true;
                instance.Play();
            }

            if (Player.health <= 0)
            {
                instance.Stop();
                instance.Dispose();
                game.paused = true;
                game.state = new GameStates.GameOverState(game);
                inTransition = true;
            }
            else if (level.EnemiesLeft() <= 0)
            {
                instance.Stop();
                instance.Dispose();
                game.wonGame = true;
                game.paused = true;
                game.state = new GameStates.GameOverState(game);
                inTransition = true;
            }

#if RUN_LEVEL
            if (!game.paused)
                level.Update(gameTime);
#endif
        }

        

    }
    
}
