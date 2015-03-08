using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManager;
using Microsoft.Xna.Framework;
using MonkeyBusiness.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MonkeyBusiness.MiniGames
{

    class LevelLast : MiniGame
    {
        #region Fields

        //Timer
        Timer timer = new Timer();
        private int timeLimit = 25;


        const int numberOfDollars = 30, scoreForDollar = 100, totalScores = scoreForDollar * numberOfDollars;
        int initialScores;
        Player player;
        private GameTime gameTime;
        List<DrawableObject> objects = new List<DrawableObject>();
        const int MonkeyInitialHeight = 88;
        private SoundEffect moneycollect;
        private SoundEffect monkeySounds;
        static int minimumInterval = 900, maximumInterval = 1000;
        const int initialMinimumInterval = 900, initialMaximumInterval = 1000;
        const int minInterval = 500;
        int timeFromLastDollar = 0;
        int timeToNextDollar = Utillities.rnd.Next(minimumInterval, maximumInterval);

        float minimumDollarSpeed = 0.05f, maximumDollarSpeed = 0.06f, initialMinimumDollarSpeed = 0.05f, initialMaximumDollarSpeed = 0.06f;
        const float maxSpeed = 0.2f;

        const float monkeySpeed = 0.2f;
        const int chancesToBanana = 13;
        const int chancesToPoisonBanana = 8;


        private SoundEffect SEFBrownBananaCollect;

        private Song backgroundMusic;
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

        private void CheckColission()
        {
            CheckCollisionWithMoney();
            CheckCollisionWithBananas();
            CheckCollisionWithPoison();
        }

        private void CheckCollisionWithMoney()
        {
            List<DrawableObject> takenDollars = Utillities.GetColliadedObjects(player, objects, "dollar");
            manager.score.addScores(scoreForDollar * takenDollars.Count);
            Utillities.RemoveNodesFromList<DrawableObject>(objects, takenDollars);
            if (takenDollars.Count != 0)
            {
                moneycollect.Play();
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

        private void CheckCollisionWithBananas()
        {
            List<DrawableObject> takenBananas = Utillities.GetColliadedObjects(player, objects, "banana");
            Utillities.RemoveNodesFromList<DrawableObject>(objects, takenBananas);
            if (takenBananas.Count != 0)
            {
                monkeySounds.Play();
                player.speed *= 1.1f;
            }
        }

        private void CheckCollisionWithPoison()
        {
            List<DrawableObject> takenPoison = Utillities.GetColliadedObjects(player, objects, "poison");
            Utillities.RemoveNodesFromList<DrawableObject>(objects, takenPoison);
            if (takenPoison.Count != 0)
            {
                SEFBrownBananaCollect.Play();
                RestartLevel();
            }
        }

        private void DollarsDrawer()
        {
            if (timeFromLastDollar > timeToNextDollar)
            {
                Texture2D texture;
                InteractiveObject createdObject;
                string objectType;
                float dollarSpeed = Utillities.rnd.Next((int)(minimumDollarSpeed * 1000), (int)(maximumDollarSpeed * 1000)) / 1000.0f;
                if (Utillities.rnd.Next(0, chancesToBanana) == 0)
                {
                    texture = Content.Load<Texture2D>("Sprites/banana");
                    objectType = "banana";
                }
                else if (Utillities.rnd.Next(0, chancesToPoisonBanana) == 0)
                {
                    texture = Content.Load<Texture2D>("Sprites/poisonbanana");
                    objectType = "poison";
                }
                else
                {
                    texture = Content.Load<Texture2D>("Sprites/money");
                    objectType = "dollar";
                }

                Vector2 position = Utillities.RandomPosition(viewport, texture.Bounds);
                position.Y = 0;
                createdObject = new InteractiveObject(texture, position, objectType);
                createdObject.SetVelocity(0, dollarSpeed);
                objects.Add(createdObject);
                timeToNextDollar = Utillities.rnd.Next(minimumInterval, maximumInterval);
                timeFromLastDollar = 0;
            }
        }

        private void CheckWinning()
        {
            if (manager.score.scores == totalScores + initialScores)
                manager.SetNextMiniGameAsCurrent();
        }

        private void CheckIfTimePassed()
        {
            if (timer.seconds <= 0)
                RestartLevel();
        }

        private void RestartLevel()
        {
            manager.score.scores = initialScores;
            timer = new Timer();
            manager.RestartMiniGame();
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
            minimumDollarSpeed = initialMinimumDollarSpeed;
            maximumDollarSpeed = initialMaximumDollarSpeed;
            maximumInterval = initialMaximumInterval;
            minimumInterval = initialMinimumInterval;
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

            timer.Draw(spriteBatch);

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
            player.AnimateOneDirectionOnlyLeftAndRight(gameTime);
            player.SetVelocity(player.GetVelocity().X, 0);
            CheckColission();
            CheckWinning();
            CheckIfTimePassed();
            Utillities.UpdateAllObjects(objects, gameTime, viewport);

            DollarsDrawer();


            if (!timer.isWorking)
                timer = new Timer(manager.score.font, gameTime, timeLimit);
            else
                timer.Update(gameTime);

        }


        public override void LoadContent()
        {
            //TODO: Load Content
            device = graphics.GraphicsDevice;
            backgroundTexture = Content.Load<Texture2D>("mallBackground");

            Texture2D MonkeyTexture = Content.Load<Texture2D>("Sprites/monkey");
            Vector2 pos = new Vector2(viewport.Width / 2 - MonkeyTexture.Width / 2, viewport.Height - MonkeyTexture.Height - MonkeyInitialHeight);
            moneycollect = Content.Load<SoundEffect>("SoundFX/moneypop");
            monkeySounds = Content.Load<SoundEffect>("SoundFX/MonkeySounds");
            SEFBrownBananaCollect = Content.Load<SoundEffect>("SoundFX/BrownBananaCollect");
            player = new Player(MonkeyTexture, pos);
            player.speed = monkeySpeed;
            player.LoadAnimation(Content.Load<Texture2D>("sheet"));
            //objects.AddRange(Utillities.CreateListOfInteractiveObjectsInRandomPositions(numberOfDollars, DollarTexture, viewport, "dollar"));
            objects.Add(player);
            initialScores = manager.score.scores;

            backgroundMusic = Content.Load<Song>("BGM/LevelLastMusic");
            MediaPlayer.Play(backgroundMusic);
        }

        /// <summary>
        /// Unload content (if needed)
        /// </summary>
        public override void UnloadContent()
        {
            objects.Clear();
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
