using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonkeyBusiness.Objects
{
    class DrawableObject
    {
        #region Fields
        /// <summary>
        /// Object's texture
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Position, height and width
        /// </summary>
        protected Vector2 position;
        
        protected int height
        {
            get
            {
                return texture.Height;
            }
        }
        protected int width
        {
            get
            {
                return texture.Width;
            }
        }

        /// <summary>
        /// Animation fields
        /// </summary>
        private bool isAnimated = false;
        private double animationInterval;
        #endregion

        public DrawableObject(Texture2D texture, Vector2 position)
        {
            LoadTexture(texture);
            this.position = position;
        }

        /// <summary>
        /// Draws the object on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        virtual public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        /// <summary>
        /// Load different texture
        /// </summary>
        /// <param name="texture">The new texture</param>
        public void LoadTexture(Texture2D texture)
        {
            this.texture = texture;
        }
    }
}
