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

        const int numberOfDollars = 30, scoreForDollar = 100, totalScores = scoreForDollar * numberOfDollars;
        int initialScores;
        Player player;
        private GameTime gameTime;
        List<DrawableObject> objects = new List<DrawableObject>();
        const int MonkeyInitialHeight = 88;

        static int minimumInterval = 900, maximumInterval = 1000;
        const int minInterval = 500;
        int timeFromLastDollar = 0;
        int timeToNextDollar = Utillities.rnd.Next(minimumInterval, maximumInterval);

        float minimumDollarSpeed = 0.05f, maximumDollarSpeed = 0.06f;
        const float maxSpeed = 0.2f;

        const float monkeySpeed = 0.2f;
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
            if (takenDollars.Count != 0)
            {
                if(maximumDollarSpeed < maxSpeed)
                    minimumDollarSpeed += 0.01f;
                if (minimumDollarSpeed < maxSpeed)
                    maximumDollarSpeed += 0.01f;
                if (minimumInterval > minInterval)
                    minimumInterval -= 10;
                if (maximumInterval > minInterval)
                    maximumInterval -= 10;
            }
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
            timeFromLastDollar += gameTime.ElapsedGameTime.Milliseconds;
            this.gameTime = gameTime;
            player.HandleInput(false);
            player.AnimateOneDirection(gameTime);
            player.SetVelocity(player.GetVelocity().X, 0);
            CheckCollisionWithMoney();
            CheckWinning();
            Utillities.UpdateAllObjects(objects, gameTime, viewport);
            

            if (timeFromLastDollar > timeToNextDollar)
            {
                Texture2D DollarTexture = Content.Load<Texture2D>("money");
                Vector2 position = Utillities.RandomPosition(viewport, DollarTexture.Bounds);
                position.Y = 0;
                InteractiveObject dollar = new InteractiveObject(DollarTexture, position, "dollar");
                float dollarSpeed = Utillities.rnd.Next((int)(minimumDollarSpeed * 1000), (int)(maximumDollarSpeed * 1000)) / 1000.0f;
                dollar.SetVelocity(0, dollarSpeed);
                objects.Add(dollar);
                timeToNextDollar = Utillities.rnd.Next(minimumInterval, maximumInterval);
                timeFromLastDollar = 0;
            }
        }


        public override void LoadContent()
        {
            //TODO: Load Content
            device = graphics.GraphicsDevice;
            backgroundTexture = Content.Load<Texture2D>("mallBackground");

            Texture2D MonkeyTexture = Content.Load<Texture2D>("monkey");
            Vector2 pos = new Vector2(viewport.Width / 2 - MonkeyTexture.Width / 2, viewport.Height - MonkeyTexture.Height - MonkeyInitialHeight);

            player = new Player(MonkeyTexture, pos);
            player.speed = monkeySpeed;
            player.LoadAnimation(Content.Load<Texture2D>("standby"));
            //objects.AddRange(Utillities.CreateListOfInteractiveObjectsInRandomPositions(numberOfDollars, DollarTexture, viewport, "dollar"));
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
        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, viewport.Width, viewport.Height);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);

        }
        #endregion
    }
}
