using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonkeyBusiness.Objects
{
    public class Score
    {
        public int scores;
        public SpriteFont font;
        private Vector2 position = new Vector2(10, 10);

        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="font"></param>
        public Score(SpriteFont font)
        {
            this.font = font;
        }

        /// <summary>
        /// Draw the score on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, scores.ToString(), position, Color.White);
        }

        /// <summary>
        /// Add X points to the score
        /// </summary>
        /// <param name="toAdd">Number of points to add</param>
        public void addScores(int toAdd)
        {
            scores += toAdd;
        }
    }
}
