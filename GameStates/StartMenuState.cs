using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;




namespace out_and_back.GameStates
{
    class StartMenuState : AbstractGameState
    {
        AssetManager assetManager;
        Texture2D titleIcon;
        SoundEffect songPt1;
        SoundEffect songPt2;
        SoundEffectInstance instance;
        private bool inTransition = false;
        bool playMusic;
        

        public StartMenuState(Game1 game)
        {
            AssetManager.Initialize(game);
            assetManager = AssetManager.Instance;
            titleIcon = assetManager.titleIcon;
            songPt1 = game.Content.Load<SoundEffect>("TitleMusicPt1");
            songPt2 = game.Content.Load<SoundEffect>("TitleMusicPt2");
            instance = songPt1.CreateInstance();
            instance.Play();
            inTransition = false;
            playMusic = game.playMusic;
        }
        public override void Draw(Game1 game, GameTime gameTime)
        {
            Vector2 titlePos = new Vector2(Globals.SCREEN_WIDTH / 2, Globals.SCREEN_HEIGHT / 2);
            if (titleIcon != null)
                assetManager.DrawIcon(titleIcon, titlePos);
            else
                assetManager.PrintString("Magic Recall", titlePos, Color.White);

            assetManager.PrintStringCenter("Press Space", new Vector2(Globals.SCREEN_WIDTH / 5, Globals.SCREEN_HEIGHT * 5 / 8), Color.Wheat ,assetManager.retroFontLarge);
            assetManager.PrintStringCenter("to Start", new Vector2(Globals.SCREEN_WIDTH * 4 / 5, Globals.SCREEN_HEIGHT * 5 / 8), Color.Wheat, assetManager.retroFontLarge);
        }

        public override void Update(Game1 game, GameTime gameTime)
        {
            playMusic = game.playMusic;
            if (!playMusic)
                instance.Volume = 0f;
            else
                instance.Volume = 0.9f;


            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                instance.Stop();
                instance.Dispose();
                game.state = new GameStates.InLevelState(game);
                inTransition = true;
            }
            if (instance.State == SoundState.Stopped && !inTransition)
            {
                instance = songPt2.CreateInstance();
                instance.IsLooped = true;
                instance.Play();
            }
        }
    }
}
