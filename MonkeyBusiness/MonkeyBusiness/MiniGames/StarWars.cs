using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManager;
using Microsoft.Xna.Framework;
using MonkeyBusiness.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonkeyBusiness.MiniGames
{
    class StarWars : MiniGame
    {
        public StarWarsText starWarsText;
        string introductionText = "Once Upon a Time\nIn the far Monkey Planet\nLived a young monkey\nnamed Chimp\nChimp was a cuerl criminal\nthat rubbed malls everyday\n\nOne day, he tried to brek\nto the most secure mall\nin the country\nBut things didn't go well\nto our poor Chimp\n\n\n\nYour Mission is to help Chimp\nto break the mall\nand steal all the money,\nbut without touching\nthe bombs and alarms\nGood Luck!!!!";


        public StarWars(Manager manager)
            : base(manager)
        {

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