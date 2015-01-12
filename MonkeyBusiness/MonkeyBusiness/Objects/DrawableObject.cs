﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonkeyBusiness.Objects
{
    class DrawableObject
    {
        #region Fields
        private Texture2D texture;
        protected Vector2 position;
        private int textureHeight, textureWidth;
        protected int height
        {
            get
            {
                return this.textureHeight;
            }
        }
        protected int width
        {
            get
            {
                return this.textureWidth;
            }
        }

        private bool isAnimated = false;
        private double animationInterval;
        #endregion

        public DrawableObject(Texture2D texture, Vector2 position)
        {
            LoadTexture(texture);
            this.position = new Vector2(position.X, position.Y);
            this.position = position;
        }

        /// <summary>
        /// Draws the object on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isAnimated)
                spriteBatch.Draw(texture, position, Color.White);
        }

        /// <summary>
        /// Load different texture
        /// </summary>
        /// <param name="texture">The new texture</param>
        public void LoadTexture(Texture2D texture)
        {
            this.texture = texture;
            textureHeight = texture.Height;
            textureWidth = texture.Width;
        }
    }
}
