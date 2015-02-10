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

    class LevelLast : MiniGame
    {
        #region Fields

        const int numberOfDollars = 10, scoreForDollar = 100, totalScores = scoreForDollar * numberOfDollars, numberOfAlarms = 10;
        int initialScores;
        Player player;
        private GameTime gameTime;
        List<DrawableObject> objects = new List<DrawableObject>();
        #endregion

        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="manager">The game state manager</param>
        public LevelLast(Manager manager)
            : base(manager)
        {

        }

        #region gameplay

        private void CheckCollisionWithMoney()
        {
            List<DrawableObject> takenDollars = Utillities.GetColliadedObjects(player, objects, "dollar");
            manager.score.addScores(scoreForDollar * takenDollars.Count);
            Utillities.RemoveNodesFromList<DrawableObject>(objects, takenDollars);
        }


        private void CheckWinning()
        {
            if (manager.score.scores == totalScores + initialScores)
                manager.SetNextMiniGameAsCurrent();
        }

        #endregion

        #region basic functions

        /// <summary>
        /// Initialization code.
        /// Add whatever you want.
        /// </summary>
        ///

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
            this.gameTime = gameTime;
            player.HandleInput();
            CheckCollisionWithMoney();
            CheckWinning();
            Utillities.UpdateAllObjects(objects, gameTime, viewport);

        }


        public override void LoadContent()
        {
            //TODO: Load Content
            device = graphics.GraphicsDevice;
            backgroundTexture = Content.Load<Texture2D>("mallBackground");

            Texture2D MonkeyTexture = Content.Load<Texture2D>("monkey");
            Vector2 pos = new Vector2(100, 100);

            player = new Player(MonkeyTexture, pos);

            Texture2D DollarTexture = Content.Load<Texture2D>("money");
            objects.AddRange(Utillities.CreateListOfInteractiveObjectsInRandomPositions(numberOfDollars, DollarTexture, viewport, "dollar"));
            objects.Add(player);
            initialScores = manager.score.scores;

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

        #region useful functions
        private Vector2 CreateRandomPosition()
        {
            Random rnd = new Random();

            return new Vector2(rnd.Next(0, viewport.Width), rnd.Next(0, viewport.Height));
        }

        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, viewport.Width, viewport.Height);
            spriteBatch.Draw(mallBackground, screenRectangle, Color.White);

        }
        #endregion
    }
}
