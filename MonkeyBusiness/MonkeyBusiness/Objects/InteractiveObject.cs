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
        float timer = 0f;
        float interval = 200f;
        int currentCol = 0;
        int currentRow = 0;
        const int singleWidth = 32;
        const int singleHeight = 33;
        bool isAnimate = false;
        #region spritesheet
        const int uprow = 1;
        const int leftrow = 2;
        const int rightrow = 3;
        const int downrow = 4;
        const int leftcol = 1;
        const int midcol = 2;
        const int rightcol = 3;
        #endregion
        double haltingTime;
        private string objectType = "interactiveObject";
        Texture2D animationTexture;
        public string type
        {
            get
            {
                return objectType;
            }
            set
            {
                objectType = value;
            }
        }
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
        public Vector2 center
        {
            get
            {
                return Utillities.PointToVector2(this.BoundingBox.Center);
            }
            set
            {
                SetPosition(value.X - 0.5f * this.width, value.Y - 0.5f * this.height);
            }
        }

        #endregion

        public InteractiveObject(Texture2D texture, Vector2 position)
            : base(texture, position)
        {

        }

        public InteractiveObject(Texture2D texture, Vector2 position, string objectType)
            : base(texture, position)
        {
            type = objectType;
        }

        #region Set Position and Velocity, get Velocity
        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void SetPosition(float X, float Y)
        {
            this.position = new Vector2(X, Y);
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        public void SetVelocity(float X, float Y)
        {
            this.velocity = new Vector2(X, Y);
        }

        public void SetCenter(Vector2 center)
        {
            this.center = center;
        }

        public void SetCenter(float X, float Y)
        {
            this.center = new Vector2(X, Y);
        }
        public Vector2 GetVelocity()
        {
            return this.velocity;
        }
        #endregion

        #region Movement

        /// <summary>
        /// This function makes the object move in a direction for specific time.
        /// </summary>
        /// <param name="velocity">The direction and speed</param>
        /// <param name="movementTime">Time until the object stops</param>
        /// <param name="gameTime"></param>
        public void MoveByVector(Vector2 velocity, double movementTime, GameTime gameTime)
        {
            haltingTime = gameTime.TotalGameTime.TotalSeconds + movementTime;
            SetVelocity(velocity);
        }


        #region Stoping
        /// <summary>
        /// Stop the object's movement if needed
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="viewport"></param>
        virtual protected void StopIfNeeded(GameTime gameTime, Viewport viewport)
        {
            if (HaltingTimePassed(gameTime))
                this.SetVelocity(Vector2.Zero);
            IsOutsideScreen(viewport);
        }

        /// <summary>
        /// Check if the object is outside the screen.
        /// </summary>
        /// <param name="viewport"></param>
        /// <returns></returns>
        virtual protected void IsOutsideScreen(Viewport viewport)
        {

            if (this.position.X < 0)
            {
                SetPosition(0, this.position.Y);
                SetVelocity(0, this.velocity.Y);
            }
            if (this.position.Y < 0)
            {
                SetPosition(this.position.X, 0);
                SetVelocity(this.velocity.X, 0);
            }
            if (this.position.X > viewport.Width - this.width)
            {
                SetPosition(viewport.Width - this.width, this.position.Y);
                SetVelocity(0, this.velocity.Y);
            }
            if (this.position.Y > viewport.Height - this.height)
            {
                SetPosition(this.position.X, viewport.Height - this.height);
                SetVelocity(this.velocity.X, 0);
            }
        }

        /// <summary>
        /// Check if the object is moving too much time.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        protected bool HaltingTimePassed(GameTime gameTime)
        {

            if (haltingTime != 0)
                if (gameTime.TotalGameTime.TotalSeconds > haltingTime)
                    return true;
            return false;
        }



        #endregion

        /// <summary>
        /// Update object's position
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="viewport"></param>
        virtual public void Update(GameTime gameTime, Viewport viewport)
        {
            StopIfNeeded(gameTime, viewport);

            this.position += this.velocity * gameTime.ElapsedGameTime.Milliseconds;
            //TODO: animate
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isAnimate )
            {
                Rectangle sourcerect = new Rectangle(singleWidth * currentCol, singleHeight * currentRow, singleWidth, singleHeight);
                spriteBatch.Draw(animationTexture, this.position, sourcerect, Color.White);
            }
            else
                base.Draw(spriteBatch);
        }

        public void LoadAnimation(Texture2D animationTexture)
        {
            this.animationTexture = animationTexture;
        }

        public void AnimateRight(GameTime gameTime)
        {
            isAnimate = true;
            currentRow = rightrow;
            currentCol = midcol;
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > interval)
            {
                currentCol++;

                if (currentCol > rightcol)
                {
                    currentCol = leftcol;
                }
                timer = 0f;
            }
        }
        public void AnimateLeft(GameTime gameTime)
        {
            isAnimate = true;
            currentRow = leftrow;
            currentCol = midcol;
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentCol++;

                if (currentCol > rightcol)
                {
                    currentCol = leftcol;
                }
                timer = 0f;
            }
        }
        public void AnimateUp(GameTime gameTime)
        {
            isAnimate = true;
            currentRow = uprow;
            currentCol = midcol;
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentCol++;

                if (currentCol > rightcol)
                {
                    currentCol = leftcol;
                }
                timer = 0f;
            }
        }
        public void AnimateDown(GameTime gameTime)
        {
            isAnimate = true;
            currentRow = downrow;
            currentCol = midcol;
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentCol++;

                if (currentCol > rightcol)
                {
                    currentCol = leftcol;
                }
                timer = 0f;
            }
        }
        #endregion
    }
}
