using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManager;
using Microsoft.Xna.Framework;
using MonkeyBusiness.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MonkeyBusiness.MiniGames
{
    class StarWars1 : MiniGame
    {
        #region fields
        //variable for the background music and keyboard state, and the text for the on-screen scrolling story
        private Song bgm;
        KeyboardState keyboard;
        public StarWarsText starWarsText;
        string introductionText = "Once Upon a Time \n In the far away Planet Monkey \n Lived a young monkey \n named Chimp.\n Chimp was a criminal \n who robbed malls everyday. \n\n One day, when he tried to break \n into the most secure mall \n in the country,\n things got out of control \n for our poor Chimp.\n\n\n Your Mission is to help Chimp \n break into the mall \n and steal all the money,\n without touching \n the bombs and alarms.\n Good Luck!";

        public StarWars1(Manager manager)
            : base(manager)
        {

        }
        #endregion

        #region gameplay functions
        //checks if the player has pressed the spacebar, and if so skips the story and continues to the next minigame
        private void CheckWinning()
        {
            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Space))
                manager.SetNextMiniGameAsCurrent();
        }

        #endregion

        #region basic functions
        /// <summary>
        /// Initialization code.
        /// Add whatever you want.
        /// </summary>

        public override void Initialize()
        {

        }

        /// <summary>
        /// Draw all objects on screen.
        /// draws the background and the scrolling text on-screen
        /// </summary>
        public override void Draw()
        {
            UpdateGraphicDevices();
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            DrawScenery();
            DrawText(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Update all objects' state
        /// updates the scrolling text
        /// </summary>
        /// <param name="gameTime"></param>
        /// 
        public override void Update(GameTime gameTime)
        {
            starWarsText.Update(gameTime);
            CheckWinning();
        }

        /// <summary>
        ///loads the text for the scrolling story, the background music and the background
        /// </summary>
        public override void LoadContent()
        {
            starWarsText = new StarWarsText(viewport, introductionText);
            starWarsText.LoadContent(Content);
            bgm = Content.Load<Song>("BGM/starwars");
            MediaPlayer.Play(bgm);
            backgroundTexture = Content.Load<Texture2D>("backgrounds/starwarsbg");
        }

        /// <summary>
        /// Unload content (if needed)
        /// </summary>
        public override void UnloadContent()
        {

        }

        #endregion

        #region useful functions

        //if the whole text has scrolled, continues on to the next minigame
        private void DrawText(SpriteBatch spriteBatch)
        {
            if (starWarsText.Draw(spriteBatch))
                manager.SetNextMiniGameAsCurrent();
            
        }
        //draws the next minigame
        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, viewport.Width, viewport.Height);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);
        }


    
        #endregion
    }
}