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
        Vector2 target;
        bool isMovingToTarget = false;

        public Player(Texture2D texture, Vector2 position)
            : base(texture, position)
        {

        }

        #region Movement

        public void GoToTarget(Vector2 target)
        {
            this.target = target;
            Vector2 velocity = speed * Utillities.Normalize(this.target - Utillities.PointToVector2(this.center));
            if (velocity.Length() == 0 || this.BoundingBox.Contains(Utillities.Vector2ToPoint(this.target)))
            {
                isMovingToTarget = false;
                velocity = Vector2.Zero;
            }
            this.SetVelocity(velocity);
        }

        #region Input

        public void HandleInput()
        {
            MouseState currentMouseState = Mouse.GetState();
            KeyboardState currentKeyboardState = Keyboard.GetState();
            HandleMouse(currentMouseState);
            HandleKeyboard(currentKeyboardState);
        }

        public void HandleMouse(MouseState currentMouseState)
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                this.target = new Vector2(currentMouseState.X, currentMouseState.Y);
                isMovingToTarget = true;
            }
            GoToTarget(target);
        }

        public void HandleKeyboard(KeyboardState currentKeyboardState)
        {
            //TODO: Handle Keyboard
        }

        #endregion
        
        #endregion
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


    }
}
