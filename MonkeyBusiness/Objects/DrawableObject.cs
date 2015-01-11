using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonkeyBusiness.Objects
{
    class DrawableObject
    {
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

        public DrawableObject(Texture2D texture, Vector2 position)
        {
            LoadTexture(texture);
            this.position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isAnimated)
                spriteBatch.Draw(texture, position, Color.White);
        }

        public void LoadTexture(Texture2D texture)
        {
            this.texture = texture;
            textureHeight = texture.Height;
            textureWidth = texture.Width;
        }
    }
}
