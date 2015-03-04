using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonkeyBusiness.Objects
{
    public class Timer
    {
        public int seconds;
        private SpriteFont font;

        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="font"></param>
        public Timer(SpriteFont font)
        {
            this.font = font;
        }

        /// <summary>
        /// Draw the score on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, seconds.ToString(), new Vector2(10, 30), Color.White);
        }

        /// <summary>
        /// Add X points to the score
        /// </summary>
        /// <param name="toAdd">Number of points to add</param>
        
    }
}
