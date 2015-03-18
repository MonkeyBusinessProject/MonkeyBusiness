using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonkeyBusiness.Objects
{
    class StarWarsText
    {
        #region Fields
        /// <summary>
        /// The text
        /// </summary>
        public string text;
        /// <summary>
        /// The text, seperated to lines
        /// </summary>
        public string[] linesText;
        /// <summary>
        /// Array of line objects
        /// </summary>
        public StarWarsLine[] lines;

        /// <summary>
        /// The velocity of the text, and the changing speed of trans and size.
        /// </summary>
        private Vector2 velocity;
        private float fadingVelocity;
        private float resizingVelocity;

        private Viewport viewport;
        private SpriteFont Font;
        #endregion

        #region Utillities

        /// <summary>
        /// Split the text to lines
        /// </summary>
        private void SplitString()
        {
            this.linesText = text.Split('\n');
        }
        #endregion

        /// <summary>
        /// Constructor, and split the text to lines
        /// </summary>
        /// <param name="viewport"></param>
        /// <param name="text"></param>
        public StarWarsText(Viewport viewport, string text)
        {
            this.viewport = viewport;
            this.text = text;
            SplitString();
        }

        /// <summary>
        /// Draw all the lines
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <returns>Does all the lines arrive the target position</returns>
        public bool Draw(SpriteBatch spriteBatch)
        {
            bool isFinished = true;
            for (int i = 0; i < lines.Length; i++)
            {
                isFinished = lines[i].Draw(spriteBatch);
            }
            if (isFinished)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Update every line
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Load font and all the lines
        /// </summary>
        /// <param name="Content"></param>
        public void LoadContent(ContentManager Content)
        {
            Font = Content.Load<SpriteFont>("StarWarsFont");
            lines = new StarWarsLine[linesText.Length];
            float heightCounter = 0;
            for (int i = 0; i < linesText.Length; i++)
            {
                lines[i] = new StarWarsLine(viewport, linesText[i], viewport.Height + heightCounter);
                lines[i].LoadContent(Content);
                heightCounter += lines[i].StringOriginalSize.Y;
            }
        }
    }
}
