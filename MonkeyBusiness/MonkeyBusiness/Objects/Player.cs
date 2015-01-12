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
        float speed = 0.1f;
        Vector2 target = new Vector2(-1, -1);

        public Player(Texture2D texture, Vector2 position)
            : base(texture, position)
        {

        }

        #region Movement
        /// <summary>
        /// This function allows the player to move toward a target.
        /// </summary>
        /// <param name="target">Vector2. Stores the target destination.</param>
        public void GoToTarget(Vector2 target)
        {
            Vector2 velocity = speed * Utillities.Normalize(this.target - this.center);
            this.SetVelocity(velocity);
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
            if (HaltingTimePassed(gameTime) || OutsideScreen(viewport))
            {
                this.SetVelocity(Vector2.Zero);
                this.target = this.center;
            }
        }

        #region Input

        /// <summary>
        /// This function handles mouse events and keyboard events
        /// </summary>
        public void HandleInput()
        {
            MouseState currentMouseState = Mouse.GetState();
            KeyboardState currentKeyboardState = Keyboard.GetState();
            HandleMouse(currentMouseState);
            HandleKeyboard(currentKeyboardState);
        }

        /// <summary>
        /// This function handles mouse events
        /// </summary>
        /// <param name="currentMouseState"></param>
        public void HandleMouse(MouseState currentMouseState)
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed)
                this.target = new Vector2(currentMouseState.X, currentMouseState.Y);
            if(target != new Vector2(-1, -1))
                GoToTarget(this.target);
        }

        /// <summary>
        /// This function handles keyboard events
        /// </summary>
        /// <param name="currentKeyboardState"></param>
        public void HandleKeyboard(KeyboardState currentKeyboardState)
        {
            //TODO: Handle Keyboard
        }

        #endregion
        
        #endregion

    }
}
