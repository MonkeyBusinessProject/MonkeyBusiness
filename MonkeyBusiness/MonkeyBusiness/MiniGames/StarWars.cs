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
    class StarWars : MiniGame
    {
        private Song bgm;
        KeyboardState keyboard;
        public StarWarsText starWarsText;
        string introductionText = "Once Upon a Time\nIn the far away Planet Monkey\nLived a young monkey\nnamed Chimp.\nChimp was a criminal\nwho robbed malls everyday.\n\nOne day, when he tried to break\ninto the most secure mall\nin the country,\nthings got out of control\nfor our poor Chimp.\n\n\n\nYour Mission is to help Chimp\n break into the mall\nand steal all the money,\nwithout touching /nthe bombs and alarms.\nGood Luck!";


        public StarWars(Manager manager)
            : base(manager)
        {

        }
        private void CheckWinning()
        {
            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Space))
                manager.SetNextMiniGameAsCurrent();
        }

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

        private void DrawScenery(SpriteBatch spriteBatch)
        {
            if (starWarsText.Draw(spriteBatch))
                manager.SetNextMiniGameAsCurrent();
        }
    }
}