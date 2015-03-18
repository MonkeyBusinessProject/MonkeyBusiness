using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonkeyBusiness.Objects
{
    class StarWarsLine
    {
        #region Fields
        /// <summary>
        /// The text of the line
        /// </summary>
        public string text;

        /// <summary>
        /// The position and velocity of the line
        /// </summary>
        private Vector2 position;
        private Vector2 velocity = new Vector2(0, -0.5f);
        /// <summary>
        /// Thge height where the line will fade and the size in this height
        /// </summary>
        private float fadingHeight = 50;
        private float minimalSize = 0.3f;

        private Viewport viewport;
        private SpriteFont Font;

        /// <summary>
        /// Get the size of the line
        /// </summary>
        public Vector2 StringSize
        {
            get
            {
                if(text.Length==0)
                    return Font.MeasureString("O") * CalcResize();
                return Font.MeasureString(text) * CalcResize();
            }
        }

        /// <summary>
        /// Get the size of the line while it created
        /// </summary>
        public Vector2 StringOriginalSize
        {
            get
            {
                if (text.Length == 0)
                    return Font.MeasureString("O");
                return Font.MeasureString(text);
            }
        }
        #endregion

        #region Utillities

        /// <summary>
        /// Calculate the part of the distance travelled between initial location and target location.
        /// </summary>
        /// <returns></returns>
        private float partOfDistancePassed()
        {
            float distanceToPoint = this.position.Y - fadingHeight;
            float totalDistance = viewport.Height - fadingHeight;
            return distanceToPoint / totalDistance;
        }

        /// <summary>
        /// Calculate the trans of the font
        /// </summary>
        /// <returns></returns>
        private float CalcTrans()
        {
            return partOfDistancePassed();
        }

        /// <summary>
        /// Calc the resize of the font.
        /// </summary>
        /// <returns></returns>
        private float CalcResize()
        {
            return (1 - minimalSize) * partOfDistancePassed() + minimalSize;
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="viewport"></param>
        /// <param name="text"></param>
        /// <param name="Yposition"></param>
        public StarWarsLine(Viewport viewport, string text, float Yposition)
        {
            this.viewport = viewport;
            this.text = text;
            this.position.Y = Yposition;
        }

        /// <summary>
        /// Draw the line on screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <returns>Does the line arrive the target position</returns>
        public bool Draw(SpriteBatch spriteBatch)
        {
            this.position.X = viewport.Width / 2 - StringSize.X / 2;
            spriteBatch.DrawString(Font, text, position, Color.NavajoWhite * CalcTrans(), 0, Vector2.Zero, CalcResize(), SpriteEffects.None, 0);
            if (this.position.Y == fadingHeight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Move up
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            this.position = Vector2.Add(this.position, this.velocity);
        }

        /// <summary>
        /// Load the font of the line
        /// </summary>
        /// <param name="Content"></param>
        public void LoadContent(ContentManager Content)
        {
            Font = Content.Load<SpriteFont>("StarWarsFont");

            this.position.X = viewport.Width / 2 - StringSize.X / 2;
        }
    }
}
