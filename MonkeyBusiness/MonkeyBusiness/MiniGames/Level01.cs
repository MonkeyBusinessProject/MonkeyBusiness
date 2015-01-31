using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManager;
using Microsoft.Xna.Framework;
using MonkeyBusiness.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonkeyBusiness;

namespace MonkeyBusiness.MiniGames
{
    class Level01 : MiniGame
    {
        const int numberOfDollars = 5, scoreForDollar = 100, totalScores = scoreForDollar * numberOfDollars;
        Player player;
        List<DrawableObject> objects = new List<DrawableObject>();

        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="manager">The game state manager</param>
        public Level01(Manager manager) : base(manager)
        {
        }

        #region gameplay

        private void CheckCollision()
        {
            List<DrawableObject> toRemove = new List<DrawableObject>();
            foreach (DrawableObject interactiveObject in objects)
            {
                if (interactiveObject is InteractiveObject)
                {
                    if(player.BoundingBox.Intersects((interactiveObject as InteractiveObject).BoundingBox) && (interactiveObject as InteractiveObject).type == "dollar"){
                        manager.score.addScores(scoreForDollar);
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
        
        #region basic functions

        /// <summary>
        /// Initialization code.
        /// Add whatever you want.
        /// </summary>
        public override void Initialize() {
            manager.IsMouseVisible = true;
        }

        /// <summary>
        /// Draw all objects on screen.
        /// </summary>
        public override void Draw()
        {
            UpdateGraphicDevices();
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            Utillities.DrawAllObjects(objects, manager.score, spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// Update all objects' state
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            player.HandleInput();
            CheckCollision();
            CheckWinning();

            Utillities.UpdateAllObjects(objects, gameTime, viewport);
        }

        /// <summary>
        /// Load content
        /// Here you should:
        ///     1. Load objects' textures
        ///     2. Add all objects to the object' list
        /// </summary>
        public override void LoadContent()
        {
            Texture2D SpriteTexture = Content.Load<Texture2D>("monkey");
            Vector2 pos = new Vector2(100, 100);

            player = new Player(SpriteTexture, pos);

            Texture2D DollarTexture = Content.Load<Texture2D>("money");
            objects.AddRange(Utillities.CreateListOfInteractiveObjectsInRandomPositions(numberOfDollars, DollarTexture, viewport, "dollar"));

            //Load to objects' list
            objects.Add(player);
        }

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

        #endregion
    }
}
