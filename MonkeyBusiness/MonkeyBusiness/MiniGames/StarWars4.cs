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
    class StarWars4 : MiniGame
    {
        #region fields
        private Song bgm;
        KeyboardState keyboard;
        public StarWarsText starWarsText;
        string introductionText = "Thanks to your help \n Chimp has become a success \n and an internet sensation. \n He went on to perform \n in television shows like \n Banana Night Live \n Good Morning Jungle \n and even during the \n Bananapalooza concert. \n\n Now, a few years later \n Chimp owns his very own mall. \n\n You must finally help him \n gather his rent \n from the various shops \n in the mall. \n\n Have fun!";

        public StarWars4(Manager manager)
            : base(manager)
        {

        }
        #endregion

        #region gameplay functions

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
        /// </summary>
        public override void Draw()
        {
            UpdateGraphicDevices();
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            DrawScenery(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Update all objects' state
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            starWarsText.Update(gameTime);
            CheckWinning();
        }

        /// <summary>
        /// Load content
        /// Here you should:
        ///     1. Load objects' textures
        ///     2. Add all objects to the object' list
        /// </summary>
        public override void LoadContent()
        {
            starWarsText = new StarWarsText(viewport, introductionText);
            starWarsText.LoadContent(Content);
            bgm = Content.Load<Song>("BGM/starwars");
            MediaPlayer.Play(bgm);
        }

        /// <summary>
        /// Unload content (if needed)
        /// </summary>
        public override void UnloadContent()
        {

        }

        #endregion

        #region useful functions


        private void DrawScenery(SpriteBatch spriteBatch)
        {
            if (starWarsText.Draw(spriteBatch))
                manager.SetNextMiniGameAsCurrent();
            Rectangle screenRectangle = new Rectangle(0, 0, viewport.Width, viewport.Height);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);
        }


        #endregion
    }
}