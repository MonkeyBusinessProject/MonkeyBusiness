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
    // TODO: change class name
    // TODO: add your mini game to the manager
    class Level02 : MiniGame
    {
        #region Fields
        // TODO: add all objects
        Player player;
        InteractiveObject trashcan;
       
        const int numberOfTrashes = 10, scoreForTrash = 100, totalScores = scoreForTrash * numberOfTrashes;
        List<DrawableObject> objects = new List<DrawableObject>();


        #endregion

        #region gameplay

        private void CheckCollisionTrashToTrashcan()
        {
            List<DrawableObject> toRemove = new List<DrawableObject>();
            foreach (DrawableObject interactiveObject in objects)
            {
                if (interactiveObject is InteractiveObject)
                {
                    if (trashcan.BoundingBox.Intersects((interactiveObject as InteractiveObject).BoundingBox) && (interactiveObject as InteractiveObject).type == "trash")
                    {
                        manager.score.addScores(scoreForTrash);
                        toRemove.Add(interactiveObject);
                    }
                }
            }
            foreach (DrawableObject interactiveObject in toRemove)
            {
                objects.Remove(interactiveObject);
            }
        }


        private void CheckWinning()
        {
            if (manager.score.score == totalScores)
                manager.SetNextMiniGameAsCurrent();
        }

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
            CheckCollisionTrashToTrashcan();
            CheckWinning();
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

            Texture2D TrashTexture = Content.Load<Texture2D>("trash");
            Texture2D MonkeyTexture = Content.Load<Texture2D>("monkey");
            Texture2D CanTexture = Content.Load<Texture2D>("TrashCan");

            Vector2 monkeyPos = CreateRandomPosition();
            Vector2 canPos = new Vector2(viewport.Bounds.Center.X,viewport.Bounds.Center.Y);
            
            player = new Player(MonkeyTexture, monkeyPos);
            trashcan = new InteractiveObject(CanTexture, canPos);
            objects.AddRange(Utillities.CreateListOfInteractiveObjectsInRandomPositions(numberOfTrashes, TrashTexture, viewport, "Trash"));
           
            //TODO: Load to objects' list
            objects.Add(trashcan);
            objects.Add(player);
            
        }

        private Vector2 CreateRandomPosition()
        {
            Random rnd = new Random();
           
            return new Vector2(rnd.Next(0,viewport.Width), rnd.Next(0, viewport.Height));
        }
        #region trash creation
       
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
