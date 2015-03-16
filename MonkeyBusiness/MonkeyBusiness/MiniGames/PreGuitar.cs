using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManager;
using Microsoft.Xna.Framework;
using MonkeyBusiness.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonkeyBusiness.MiniGames
{
    
    class PreGuitar : MiniGame
    {
        //variable for keyboard input and constructor
        KeyboardState keyboard;
        public PreGuitar(Manager manager)
            : base(manager)
        {

        }
        //checks if the player pressed the spacebar, which then transfers him to the next minigame
        private void CheckWinning()
        {
            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Space))
                manager.SetNextMiniGameAsCurrent();
        }
        /// <summary>
        /// Initialization code.
        /// 
        /// </summary>
        public override void Initialize()
        {

        }

        /// <summary>
        /// Draw the background
        /// </summary>
        public override void Draw()
        {
            UpdateGraphicDevices();
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            DrawScenery();
            spriteBatch.End();
        }

        /// <summary>
        /// Update all objects' state
        /// continually checks if the spacebar has been pressed
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            CheckWinning();
        }

        /// <summary>
        /// Load content
        /// Here you should:
        ///     1. Load objects' textures
        ///     2. Add all objects to the object' list
        /// </summary>
        public override void LoadContent()
        {
            //loads the background texture
            device = graphics.GraphicsDevice;
            backgroundTexture = Content.Load<Texture2D>("backgrounds/guitarPrescreen");
        }

        /// <summary>
        /// Unload content (if needed)
        /// </summary>
        public override void UnloadContent()
        {

        }
        //creates the background
        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, viewport.Width, viewport.Height);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);

        }
    }
}