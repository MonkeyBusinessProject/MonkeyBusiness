using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyBusiness.Objects
{
    class Timer
    {
        public int seconds = 0;
        private SpriteFont font;
        private Vector2 position = new Vector2(10, 30);
        private int finalGameTime;
        public bool isWorking = false;
        bool timeChanged = false;
        int timeToEnd;

        public Timer(SpriteFont font, GameTime gameTime, int timeLimit)
        {
            this.font = font;
            this.seconds = timeLimit;
            finalGameTime = gameTime.TotalGameTime.Seconds + timeLimit;
            isWorking = true;
        }

        public Timer()
        {
            isWorking = false;
        }


        /// <summary>
        /// Draw the time on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if(isWorking)
                spriteBatch.DrawString(font, seconds.ToString(), position, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            if (timeChanged)
            {
                timeChanged = false;
                finalGameTime = gameTime.TotalGameTime.Seconds + timeToEnd;
            }
            if(isWorking)
                this.seconds = finalGameTime - gameTime.TotalGameTime.Seconds;
        }

        public void ChangeTimerFinalTime(int timeLimit)
        {
            timeToEnd = timeLimit;
            timeChanged = true;
        }
    }
}
