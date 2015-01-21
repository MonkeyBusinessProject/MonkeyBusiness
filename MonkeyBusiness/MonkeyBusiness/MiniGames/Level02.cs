﻿using System;
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
    // TODO: change class name
    // TODO: add your mini game to the manager
    class Level02 : MiniGame
    {
        #region Fields
        // TODO: add all objects
        Player player;
        InteractiveObject trashcan;
        private Texture2D MonkeyTexture;
        private Texture2D CanTexture;
        List<InteractiveObject> trashes = new List<InteractiveObject>();
        List<DrawableObject> objects = new List<DrawableObject>();


        #endregion

        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="manager">The game state manager</param>
        public Level02(Manager manager)
            : base(manager)
        {

        }

        /// <summary>
        /// Initialization code.
        /// Add whatever you want.
        /// </summary>
        public override void Initialize()
        {
            manager.IsMouseVisible = true;//Or not...
        }

        /// <summary>
        /// Draw all objects on screen.
        /// </summary>
        public override void Draw()
        {
            UpdateGraphicDevices();
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            DrawScenery();
            Utillities.DrawAllObjects(objects, manager.score, spriteBatch);
            
            spriteBatch.End();
        }

        /// <summary>
        /// Update all objects' state
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Handle input
            //Example:          player.HandleInput();
            player.HandleInput();
            Utillities.UpdateAllObjects(objects, gameTime, viewport);
        }

        /// <summary>
        /// Load content
        /// Here you should:
        ///     1. Load objects' textures
        ///     2. Add all objects to the object list
        /// </summary>
        public override void LoadContent()
        {

            //TODO: Load Content
            device = graphics.GraphicsDevice;
            backgroundTexture = Content.Load<Texture2D>("background");
         

            MonkeyTexture = Content.Load<Texture2D>("monkey");
            CanTexture = Content.Load<Texture2D>("TrashCan");

            Vector2 monkeyPos = new Vector2();
            Vector2 canPos = new Vector2();

            trashcan = new InteractiveObject(CanTexture, canPos);
            player = new Player(MonkeyTexture, monkeyPos);
            // fix next line
            // CreateTrash(10, player.Width, GraphicsDevice.Viewport.Bounds.Width, player.Height, GraphicsDevice.Viewport.Bounds.Height);
            //TODO: Load to objects' list

            objects.Add(player);
            objects.Add(trashcan);
        }
        #region trash creation
        private void CreateTrash(int number, float minX, float maxX, float minY, float maxY)
        {
            for (int i = 0; i < number; i++)
            {
                trashes.Add(new InteractiveObject(Content.Load<Texture2D>(""), new Vector2()));
            }
        }
        #endregion
        /// <summary>
        /// Unload content (if needed)
        /// </summary>
        public override void UnloadContent()
        {
            try
            {
                UpdateGraphicDevices();
                graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
                objects.Clear();
            }
            catch (Exception ex)
            {

            }


        }
        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, viewport.Width, viewport.Height);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);
            
        }
    }
}
