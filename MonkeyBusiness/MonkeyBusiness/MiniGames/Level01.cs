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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MonkeyBusiness.MiniGames
{
    class Level01 : MiniGame
    {
        #region fields
        Texture2D diedMonkey;
        Texture2D safeZoneTexture;

        const int numberOfDollars = 5, scoreForDollar = 100, totalScores = scoreForDollar * numberOfDollars, numberOfAlarms = 10;
        float alarmspeed = 0.5f, playerSpeed = 0.2f;
        int initialScores;
        Player player;
        private SoundEffect alarmhit;
        private SoundEffect moneycollect;
        
        private Song backgroundMusic;
        List<DrawableObject> objects = new List<DrawableObject>();
        bool isDead = false;

        Vector2 playerInitial = new Vector2(75, 75);
        static Rectangle initialSafeZone = new Rectangle(0, 0, 150, 150);
        Rectangle safeZone = initialSafeZone;

        //Timer
        Timer timer = new Timer();
        private int timeLimit = 25;
        
        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="manager">The game state manager</param>
        public Level01(Manager manager) : base(manager)
        {
        }
        #endregion

        #region gameplay

        private void CheckCollision()
        {
            List<DrawableObject> takenDollars = Utillities.GetColliadedObjects(player, objects, "dollar");
            manager.score.addScores(scoreForDollar * takenDollars.Count);
            if (takenDollars.Count != 0)
            {
                moneycollect.Play();
            }
            Utillities.RemoveNodesFromList<DrawableObject>(objects, takenDollars);
            List<DrawableObject> collidedAlarms = Utillities.GetColliadedObjects(player, objects, "alarm");
            if (collidedAlarms.Count != 0)
            {
                Die();
            }
        }

        private void CheckIfTimePassed()
        {
            if (timer.seconds <= 0 && !isDead)
                Die();
        }

        private void Die()
        {
            isDead = true;
            timer.ChangeTimerFinalTime(2);
            player.LoadTexture(diedMonkey);
        }

        private void RestartLevel()
        {
            alarmhit.Play();
            manager.score.scores = initialScores;
            timer = new Timer();
            manager.RestartMiniGame();
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

            spriteBatch.Draw(safeZoneTexture, safeZone, Color.White);

            Utillities.DrawAllObjects(objects, manager.score, spriteBatch);
            timer.Draw(spriteBatch);

            DrawScenery();
            spriteBatch.End();
        }

        /// <summary>
        /// Update all objects' state
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (!timer.isWorking)
                timer = new Timer(manager.score.font, gameTime, timeLimit);
            else
                timer.Update(gameTime);
            if (!isDead)
            {
                if(timer.seconds != timeLimit)
                    player.HandleInput(true);
                CheckIfTimePassed();
                CheckCollision();
                CheckWinning();

                if(!safeZone.Contains(Utillities.Vector2ToPoint(player.center))){
                    foreach (InteractiveObject alarm in Utillities.GetObjectsFromType(objects, "alarm"))
                    {
                        safeZone = Rectangle.Empty;
                        alarm.SetSafeZone(safeZone);
                    }
                }

                Utillities.UpdateAllObjects(objects, gameTime, viewport);
            }
            else
            {
                while (Mouse.GetState().LeftButton == ButtonState.Pressed);
                while (Mouse.GetState().LeftButton == ButtonState.Released);
                RestartLevel();
            }
        }

        /// <summary>
        /// Load content
        /// Here you should:
        ///     1. Load objects' textures
        ///     2. Add all objects to the object' list
        /// </summary>
        public override void LoadContent()
        {
            Texture2D SpriteTexture = Content.Load<Texture2D>("Sprites/monkey");
            Vector2 pos = playerInitial;
            backgroundTexture = Content.Load<Texture2D>("backgrounds/firstlevelbg");

            alarmhit = Content.Load<SoundEffect>("SoundFX/alarmhit");
            moneycollect = Content.Load<SoundEffect>("SoundFX/moneypop");
            player = new Player(SpriteTexture, pos);
            player.speed = playerSpeed;

            Texture2D DollarTexture = Content.Load<Texture2D>("Sprites/money");
            objects.AddRange(Utillities.CreateListOfInteractiveObjectsInRandomPositionsOutsideSafeZone(numberOfDollars, DollarTexture, viewport, "dollar", safeZone));

            Texture2D AlarmTexture = Content.Load<Texture2D>("Sprites/alarm");

            List<InteractiveObject> alarms = Utillities.CreateListOfInteractiveObjectsInRandomPositionsWithVelocityOutsideSafeZoneAndSomeOtherUnimportantWordsThatTheirPurposeIsToCreateTheLongesterFunctionNameEver111111111111(numberOfAlarms, AlarmTexture, viewport, "alarm", -alarmspeed, alarmspeed, safeZone);
            foreach (InteractiveObject alarm in alarms)
            {
                alarm.SetSafeZone(safeZone);
                alarm.SetElastic(true);
            }
            objects.AddRange(alarms);

            backgroundMusic = Content.Load<Song>("BGM/Level1Music");
            MediaPlayer.Play(backgroundMusic);

            //Load to objects' list
            objects.Add(player);
            initialScores = manager.score.scores;
            diedMonkey = Content.Load<Texture2D>("Sprites/alfredo");
            safeZoneTexture = Content.Load<Texture2D>("Sprites/safeZone");

        }

        /// <summary>
        /// Unload content (if needed)
        /// </summary>
        public override void UnloadContent()
        {
            objects.Clear();
            isDead = false;
            safeZone = initialSafeZone;
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
