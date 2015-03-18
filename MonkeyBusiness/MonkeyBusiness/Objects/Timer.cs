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
        /// <summary>
        /// Store the seconds left
        /// </summary>
        public int seconds = 0;
        private SpriteFont font;
        /// <summary>
        /// Stores the position of the timer
        /// </summary>
        private Vector2 position = new Vector2(10, 30);
        /// <summary>
        /// Stores the final time
        /// </summary>
        private int finalGameTime;
        /// <summary>
        /// Stores the timer state.
        /// </summary>
        public bool isWorking = false;
        /// <summary>
        /// Does the timer's final time changed.
        /// </summary>
        bool timeChanged = false;
        /// <summary>
        /// Stores the time to end
        /// </summary>
        int timeToEnd;

        /// <summary>
        /// Constructor, creates a working timer
        /// </summary>
        /// <param name="font"></param>
        /// <param name="gameTime"></param>
        /// <param name="timeLimit"></param>
        public Timer(SpriteFont font, GameTime gameTime, int timeLimit)
        {
            this.font = font;
            this.seconds = timeLimit;
            finalGameTime = gameTime.TotalGameTime.Seconds + timeLimit;
            isWorking = true;
        }

        /// <summary>
        /// Constructor, create an unworking timer
        /// </summary>
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

        /// <summary>
        /// Update the timer's time
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            ///if the time changed, change the final time
            if (timeChanged)
            {
                timeChanged = false;
                finalGameTime = gameTime.TotalGameTime.Seconds + timeToEnd;
            }
            ///if the timer is working, tick
            if(isWorking)
                this.seconds = finalGameTime - gameTime.TotalGameTime.Seconds;
        }
        /// <summary>
        /// change final time
        /// </summary>
        /// <param name="timeLimit"></param>
        public void ChangeTimerFinalTime(int timeLimit)
        {
            timeToEnd = timeLimit;
            timeChanged = true;
        }
    }
}
