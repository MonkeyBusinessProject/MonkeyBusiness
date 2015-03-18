using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonkeyBusiness.Objects
{
    public class AnimatedSprite
    {
        #region fields
        /// <summary>
        /// sets variables to get the texture atlas, the number of columns and rows in the atlas, and store which frame is currently presented
        /// and how many frames there are in total
        /// </summary>
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        float timer = 0f;
        float interval = 200f;
        #endregion

        #region constructor
        /// <summary>
        /// creates a new instance of the AnimatedSprite class
        /// gets and sets the values for the variables set in 'fields'
        /// </summary>
       
        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
        }
        #endregion

        #region basic functions
        /// <summary>
        /// update method constantly updating the current frame displayed
        /// if the last frame is reached, sets the next frame to be the first one
        /// </summary>
        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame == totalFrames)
                    currentFrame = 0;
                timer = 0;
            }
        }
        /// <summary>
        /// sets the width and height of each frame, and in which row and column the sprite is located at
        /// sets the rectangle of the sprite to be displayed, and the rectangle location in which it will be drawn
        /// then finally draws selected texture at the predetermined location (destinationRectangle) with the predetermined size (sourceRectangle)
        /// </summary>
        
        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int colWidth = Texture.Width / Columns;
            int colHeight = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(colWidth * column, colHeight * row, colWidth, colHeight);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, colWidth, colHeight);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
        #endregion
    }
}
