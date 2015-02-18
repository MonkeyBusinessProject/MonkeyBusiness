using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonkeyBusiness;
using Microsoft.Xna.Framework.Input;

namespace MonkeyBusiness.Objects
{
    class Player : InteractiveObject
    {
        public float speed = 0.1f;
        Vector2 target = new Vector2(-1, -1);
        KeyboardState lastkeyboardstate = Keyboard.GetState();
        private string direction;

        public Player(Texture2D texture, Vector2 position)
            : base(texture, position)
        {

        }

        #region Movement
        /// <summary>
        /// This function allows the player to move toward a target.
        /// </summary>
        /// <param name="target">Vector2. Stores the target destination.</param>
        public void GoToTarget()
        {
            Vector2 velocity = speed * Utillities.Normalize(this.target - this.center);
            this.SetVelocity(velocity);
        }

        private void SetTarget(Vector2 target)
        {
            this.target = target;
        }

        private void SetTarget(float X, float Y)
        {
            this.target = new Vector2(X, Y);
        }

        /// <summary>
        /// This function stops the player's movement if needed
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="viewport"></param>
        override protected void StopIfNeeded(GameTime gameTime, Viewport viewport)
        {
            //If in target
            if (Vector2.Distance(this.target, this.center) <= this.speed * gameTime.ElapsedGameTime.Milliseconds)
            {
                this.SetVelocity(Vector2.Zero);
                this.SetCenter(this.target);
            }
            
            //If halting time had passed or outside screen
            base.StopIfNeeded(gameTime, viewport);
        }

        protected override void IsOutsideScreen(Viewport viewport)
        {
            if (this.position.X < 0)
            {
                SetTarget(this.width / 2, this.target.Y);
            }
            if (this.position.Y < 0)
            {
                SetTarget(this.target.X, this.height / 2);
            }
            if (this.position.X > viewport.Width - this.width)
            {
                SetTarget(viewport.Width - this.width / 2, this.target.Y);
            }
            if (this.position.Y > viewport.Height - this.height)
            {
                SetTarget(this.target.X, viewport.Height - this.height / 2);
            }
            base.IsOutsideScreen(viewport);
            
        }

        #region Input

        /// <summary>
        /// This function handles mouse events and keyboard events
        /// </summary>
        public void HandleInput(bool isControlByMouse)
        {
            MouseState currentMouseState = Mouse.GetState();
            KeyboardState currentKeyboardState = Keyboard.GetState();
            if(isControlByMouse)
                HandleMouse(currentMouseState);
            else
                HandleKeyboard(currentKeyboardState,lastkeyboardstate);

            lastkeyboardstate = currentKeyboardState;
        }

        /// <summary>
        /// This function handles mouse events
        /// </summary>
        /// <param name="currentMouseState"></param>
        public void HandleMouse(MouseState currentMouseState)
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed)
                SetTarget(currentMouseState.X, currentMouseState.Y);
            if(target != new Vector2(-1, -1))
                GoToTarget();
        }

        /// <summary>
        /// This function handles keyboard events
        /// </summary>
        /// <param name="currentKeyboardState"></param>
        public void HandleKeyboard(KeyboardState currentKeyboardState, KeyboardState lastkeyboardstate)
        {
            //if(Utillities.KeyboardArrowPress(currentKeyboardState, lastkeyboardstate) != null)
                direction = Utillities.KeyboardArrowDown(currentKeyboardState);
            //creating movement
            if (Utillities.KeyboardArrowPressed(currentKeyboardState, lastkeyboardstate) == "Right")
            {
                SetVelocity(speed, this.GetVelocity().Y);
            }
            if (Utillities.KeyboardArrowPressed(currentKeyboardState, lastkeyboardstate) == "Left")
            {
                SetVelocity(-speed, this.GetVelocity().Y);
            }
            if (Utillities.KeyboardArrowPressed(currentKeyboardState, lastkeyboardstate) == "Up")
            {
                SetVelocity(this.GetVelocity().X, -speed);
            }
            if (Utillities.KeyboardArrowPressed(currentKeyboardState, lastkeyboardstate) == "Down")
            {
                SetVelocity(this.GetVelocity().X, speed);
            }
            //stopping movement
            if (Utillities.KeyboardArrowReleased(currentKeyboardState, lastkeyboardstate) == "Right" && !currentKeyboardState.IsKeyDown(Keys.Left))
            {
                direction = null;
                SetVelocity(0, this.GetVelocity().Y);
            }
            else if(currentKeyboardState.IsKeyDown(Keys.Left))
            {
                SetVelocity(-speed, this.GetVelocity().Y);
            }
            if (Utillities.KeyboardArrowReleased(currentKeyboardState, lastkeyboardstate) == "Left" && !currentKeyboardState.IsKeyDown(Keys.Right))
            {
                direction = null;
                SetVelocity(-0, this.GetVelocity().Y);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                SetVelocity(speed, this.GetVelocity().Y);
            }
            if (Utillities.KeyboardArrowReleased(currentKeyboardState, lastkeyboardstate) == "Up" && !currentKeyboardState.IsKeyDown(Keys.Down))
            {
                direction = null;
                SetVelocity(this.GetVelocity().X, -0);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                SetVelocity(this.GetVelocity().X, speed);
            }
            if (Utillities.KeyboardArrowReleased(currentKeyboardState, lastkeyboardstate) == "Down" && !currentKeyboardState.IsKeyDown(Keys.Up))
            {
                direction = null;
                SetVelocity(this.GetVelocity().X, 0);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                SetVelocity(this.GetVelocity().X, -speed);
            }
        }

        public void AnimateOneDirectionFromFourDirctions(GameTime gameTime)
        {
            if (direction == "Right")
            {
                this.AnimateRight(gameTime);
            }
            if (direction == "Left")
            {
                this.AnimateLeft(gameTime);
            }
            if (direction == "Down")
            {
                this.AnimateDown(gameTime);
            }
            if (direction == "Up")
            {
                this.AnimateUp(gameTime);
            }
            if (direction == null)
            {
                this.StopAnimation(gameTime);
            }
        }

        public void AnimateOneDirectionOnlyLeftAndRight(GameTime gameTime)
        {
            if (direction == "Right")
            {
                this.AnimateRight(gameTime);
            }
            if (direction == "Left")
            {
                this.AnimateLeft(gameTime);
            }
            if (direction == null)
            {
                this.StopAnimation(gameTime);
            }
        }

        #endregion
        
        #endregion

    }
}
