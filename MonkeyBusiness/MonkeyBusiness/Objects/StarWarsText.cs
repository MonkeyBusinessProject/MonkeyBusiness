using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonkeyBusiness.Objects
{
    class StarWarsText
    {
        #region Fields
        public string text;
        public string[] linesText;
        public StarWarsLine[] lines;

        private Vector2 velocity;
        private float fadingVelocity;
        private float resizingVelocity;

        private Viewport viewport;
        private SpriteFont Font;
        #endregion

        #region Utillities

        private void SplitString()
        {
            this.linesText = text.Split('\n');
        }

        private void DrawLine(int index)
        {
            if (index < linesText.Length)
            {
                
            }
        }

        #endregion


        public StarWarsText(Viewport viewport, string text)
        {
            this.viewport = viewport;
            this.text = text;
            SplitString();
        }

        public bool Draw(SpriteBatch spriteBatch)
        {
            bool isFinished = true;
            for (int i = 0; i < lines.Length; i++)
            {
                isFinished = lines[i].Draw(spriteBatch);
            }
            if (isFinished)
                return true;
            else
                return false;
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].Update(gameTime);
            }
        }

        public void LoadContent(ContentManager Content)
        {
            Font = Content.Load<SpriteFont>("StarWarsFont");
            lines = new StarWarsLine[linesText.Length];
            float heightCounter = 0;
            for (int i = 0; i < linesText.Length; i++)
            {
                lines[i] = new StarWarsLine(viewport, linesText[i], viewport.Height + heightCounter);
                lines[i].LoadContent(Content);
                heightCounter += lines[i].StringOriginalSize.Y;
            }
        }
    }
}
