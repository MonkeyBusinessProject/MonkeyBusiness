using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonkeyBusiness.Objects
{
    class InteractiveObject : DrawableObject
    {
        Vector2 velocity = Vector2.Zero;
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    this.width,
                    this.height);
            }
        }
        public Point center
        {
            get
            {
                return this.BoundingBox.Center;
            }
        }

        public InteractiveObject(Texture2D texture, Vector2 position)
            : base(texture, position)
        {

        }

        #region set position and velocity
        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }
        #endregion

        virtual public void Update(GameTime gameTime)
        {
            this.position += this.velocity * gameTime.ElapsedGameTime.Milliseconds;

            //TODO: animate
        }
    }
}
