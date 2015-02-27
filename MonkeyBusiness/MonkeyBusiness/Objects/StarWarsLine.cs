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
        public string text;

        private Vector2 position;
        private Vector2 velocity = new Vector2(0, -0.5f);
        private float fadingHeight = 50;
        private float minimalSize = 0.3f;

        private Viewport viewport;
        private SpriteFont Font;

        public Vector2 StringSize
        {
            get
            {
                if(text.Length==0)
                    return Font.MeasureString("O") * CalcResize();
                return Font.MeasureString(text) * CalcResize();
            }
        }

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

        private float partOfDistancePassed()
        {
            float distanceToPoint = this.position.Y - fadingHeight;
            float totalDistance = viewport.Height - fadingHeight;
            return distanceToPoint / totalDistance;
        }
        private float CalcTrans()
        {
            return partOfDistancePassed();
        }

        private float CalcResize()
        {
            return (1 - minimalSize) * partOfDistancePassed() + minimalSize;
        }

        #endregion


        public StarWarsLine(Viewport viewport, string text, float Yposition)
        {
            this.viewport = viewport;
            this.text = text;
            this.position.Y = Yposition;
        }

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

        public void Update(GameTime gameTime)
        {
            this.position = Vector2.Add(this.position, this.velocity);
        }

        public void LoadContent(ContentManager Content)
        {
            Font = Content.Load<SpriteFont>("StarWarsFont");

            this.position.X = viewport.Width / 2 - StringSize.X / 2;
        }
    }
}
