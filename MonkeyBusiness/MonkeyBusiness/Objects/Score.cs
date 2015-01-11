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
        public int score;
        private SpriteFont font;

        public Score(SpriteFont font)
        {
            this.font = font;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, score.ToString(), new Vector2(10, 10), Color.White);
        }

        public void addScores(int toAdd)
        {
            score += toAdd;
        }
    }
}
