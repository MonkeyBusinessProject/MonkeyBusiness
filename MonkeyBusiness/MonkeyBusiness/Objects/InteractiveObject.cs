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

        #region Fields
        Vector2 velocity = Vector2.Zero;
        double haltingTime;
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

        #endregion

        public InteractiveObject(Texture2D texture, Vector2 position)
            : base(texture, position)
        {

        }

        #region Set Position and Velocity
        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }
        #endregion

        #region Special Movements

        public void MoveByVector(Vector2 velocity, double movementTime, GameTime gameTime)
        {
            haltingTime = gameTime.TotalGameTime.TotalSeconds + movementTime;
            SetVelocity(velocity);
        }

        #endregion
        virtual public void Update(GameTime gameTime)
        {
            this.position += this.velocity * gameTime.ElapsedGameTime.Milliseconds;

            if (haltingTime != 0)
                if (gameTime.TotalGameTime.TotalSeconds > haltingTime)
                    this.SetVelocity(Vector2.Zero);
            //TODO: animate
        }
    }
}
