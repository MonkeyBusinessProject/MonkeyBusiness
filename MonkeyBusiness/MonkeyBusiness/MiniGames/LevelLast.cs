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


        const int numberOfDollars = 30, scoreForDollar = 100, totalScores = scoreForDollar * numberOfDollars; //creats the number of dollars that will "fall from the sky", sets the score for collecting dollars and how much is needed to win
        int initialScores; //defines the initial score based on previous levels
        Player player;
        private GameTime gameTime;
        List<DrawableObject> objects = new List<DrawableObject>(); //a list of all spawned objects
        const int MonkeyInitialHeight = 88; //the height in which the monkey is spawned on-screen
        
        private SoundEffect moneycollect;
        private SoundEffect monkeySounds;

        static int minimumInterval = 900, maximumInterval = 1000; //the minimum and maximum intervals between falling dollars
        const int initialMinimumInterval = 900, initialMaximumInterval = 1000; //the initial minimum and maximum intervals
        const int minInterval = 500;
        int timeFromLastDollar = 0; //the time from the last fallen dollar
        int timeToNextDollar = Utillities.rnd.Next(minimumInterval, maximumInterval);//the time untill another dollar falls, generated randomly between the minimum and maximum intervals

        float minimumDollarSpeed = 0.05f, maximumDollarSpeed = 0.06f, initialMinimumDollarSpeed = 0.05f, initialMaximumDollarSpeed = 0.06f; //the minimum and maximum speed of falling dollars
        const float maxSpeed = 0.2f;

        const float monkeySpeed = 0.2f; //the player movement speed
        const int chancesToBanana = 13; //the chance of a buff banana falling
        const int chancesToPoisonBanana = 8; //the chance of a posionous banana falling


        private SoundEffect SEFBrownBananaCollect; //the variable for loading sound effect for eating a brown banana

        private Song backgroundMusic; //the variable for background music
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
        //checks collision of the player with money/yellow bananas/brown bananas
        private void CheckColission()
        {
            CheckCollisionWithMoney();
            CheckCollisionWithBananas();
            CheckCollisionWithPoison();
        }
        //checks if the player collected a dollar. If so, the dollar is removed, score is given, a soundbyte is played, the speed of falling dollars is increased and the intervals between them is decreased.
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
        //checks to see if the player collected a yellow banana. If so, the banana is removed, a soundbyte is played, and the player gains momentary increased movement speed
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
        //checks to see if the player collected a brown banana. If so, the banana is removed, a sound byte is played and the player loses, restarting the level.
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
        //draws the falling dollars at random speeds and positions, and randomly draws yellow and brown bananas
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
        //checks to see if the player won based on accumulated score
        private void CheckWinning()
        {
            if (manager.score.scores == totalScores + initialScores)
                manager.SetNextMiniGameAsCurrent();
        }
        //checks to see if time has run out
        private void CheckIfTimePassed()
        {
            if (timer.seconds <= 0)
                RestartLevel();
        }
        //restarts the level
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
        /// makes the mouse visible
        /// sets the minimum and maximum speeds of falling dollars
        /// sets the minimum and maximum intervals between falling dollars
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
            timeFromLastDollar += gameTime.ElapsedGameTime.Milliseconds; //updates the time after the last fallen dollar
            this.gameTime = gameTime;
            player.HandleInput(false);
            player.AnimateOneDirectionOnlyLeftAndRight(gameTime); //animates the player only diagonally
            player.SetVelocity(player.GetVelocity().X, 0);
            CheckColission(); 
            CheckWinning();
            CheckIfTimePassed();
            Utillities.UpdateAllObjects(objects, gameTime, viewport);

            DollarsDrawer();//draws the dollars

            //if the timer is not working, creates a new one, otherwise updates it.
            if (!timer.isWorking)
                timer = new Timer(manager.score.font, gameTime, timeLimit);
            else
                timer.Update(gameTime);

        }


        public override void LoadContent()
        {
            //TODO: Load Content
            device = graphics.GraphicsDevice;
            //loads the textures for the background and the monkey
            backgroundTexture = Content.Load<Texture2D>("backgrounds/mallBackground");
            Texture2D MonkeyTexture = Content.Load<Texture2D>("Sprites/monkey");
            

            Vector2 pos = new Vector2(viewport.Width / 2 - MonkeyTexture.Width / 2, viewport.Height - MonkeyTexture.Height - MonkeyInitialHeight);
            //loads the soundbytes for collecting money and various bananas
            moneycollect = Content.Load<SoundEffect>("SoundFX/moneypop");
            monkeySounds = Content.Load<SoundEffect>("SoundFX/MonkeySounds");
            SEFBrownBananaCollect = Content.Load<SoundEffect>("SoundFX/BrownBananaCollect");
            //spawns the player and sets their speed
            player = new Player(MonkeyTexture, pos);
            player.speed = monkeySpeed;
            //animates the player using a spritesheet
            player.LoadAnimation(Content.Load<Texture2D>("backgrounds/sheet"));
            //objects.AddRange(Utillities.CreateListOfInteractiveObjectsInRandomPositions(numberOfDollars, DollarTexture, viewport, "dollar"));
            objects.Add(player);
            initialScores = manager.score.scores;
            //plays the background music
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
        //draws the background
        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, viewport.Width, viewport.Height);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);

        }
        #endregion
    }
}
