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

        int[] numberOfDollars = {5, 5, 10, 1}, numberOfAlarms = {3, 7, 10, 30};
        const int scoreForDollar = 100;
        int totalScores; //sets the number of dollars to spawn, score for each dollar, how much score is needed to win, and number of alarms to spawn
            
        float alarmspeed = 0.5f, playerSpeed = 0.2f; //the movement speed of the player and alarms
        int initialScores;
        Player player;
        //declaring variables for sounds
        private SoundEffect alarmhit;
        private SoundEffect moneycollect;
        private Song backgroundMusic;

        List<DrawableObject> objects = new List<DrawableObject>(); //a list that will contain all the spawned objects
        bool isDead = false;

        Vector2 playerInitial = new Vector2(75, 75); //spawn point for the player
        static Rectangle initialSafeZone = new Rectangle(0, 0, 150, 150); //the place and dimensions of the safe zone
        Rectangle safeZone = initialSafeZone;

        //Timer
        Timer timer = new Timer();
        private int[] timeLimit = {15, 25, 25, 100000};
        
        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="manager">The game state manager</param>
        public Level01(Manager manager) : base(manager)
        {
        }
        #endregion

        #region gameplay
         /// <summary>
         /// A function that checks of there has been a collision between the player and any other objects
         /// if the player hits a dollar object, a soundbyte plays indicating collection and said dollar is removed from the screen, increasing score
         /// if the player hits an alarm object, the player loses
         /// </summary>
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
                alarmhit.Play();
                Die();
            }
        }
        
        private void CheckIfTimePassed() //a function that checks of a predetermined amount of time has passed before the player won. If so, the player loses
        {
            if (timer.seconds <= 0 && !isDead)
                Die();
        }

        private void Die() //a function that handles loses.
        {
            isDead = true;
            timer.ChangeTimerFinalTime(2);
            player.LoadTexture(diedMonkey);
        }

        private void RestartLevel() //a function that restarts the level upon losing, including the score and the timer
        {
            
            manager.score.scores = initialScores;
            timer = new Timer();
            manager.RestartMiniGame();
        }

        private void CheckWinning() //the function checks if a certain score (the score for collecting all the dollar objects) has been reached. If so, the level ends and the player is then transferred to the next level
        {
            if (manager.score.scores == totalScores + initialScores)
                manager.SetNextMiniGameAsCurrent();
        }

        #endregion
        
        #region basic functions

        
        public override void Initialize() {
            manager.IsMouseVisible = true; //makes the mouse visible
            totalScores = scoreForDollar * numberOfDollars[manager.diff];
        }

        /// <summary>
        /// Draw all objects on screen.
        /// </summary>
        public override void Draw()
        {
            UpdateGraphicDevices();
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            DrawScenery(); //draws the background for the level

            spriteBatch.Draw(safeZoneTexture, safeZone, Color.White); //draws the safezone

            Utillities.DrawAllObjects(objects, manager.score, spriteBatch); //draws all the objects (player, dollars, alarms, and any other object that needs to be drawn, also draws score
            timer.Draw(spriteBatch); //draws the timer
            spriteBatch.End();
        }

        /// <summary>
        /// Update all objects' state
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (!timer.isWorking) //if the timer is not working, a new timer is created, otherwise it is updated based on the elapsed game time
                timer = new Timer(manager.score.font, gameTime, timeLimit[manager.diff]);
            else
                timer.Update(gameTime);
            if (!isDead) //if the player hasn't lost due to hitting an alarm or the timer running out, they are able to play, and the game continues to check for colliding with objects, winning, and/or the timer running out
            {
                if(timer.seconds != timeLimit[manager.diff])
                    player.HandleInput(true);
                CheckIfTimePassed();
                CheckCollision();
                CheckWinning();

                if(!safeZone.Contains(Utillities.Vector2ToPoint(player.center))){ //if the player exits the safe zone, the safe zone is removed from the level
                    foreach (InteractiveObject alarm in Utillities.GetObjectsFromType(objects, "alarm"))
                    {
                        safeZone = Rectangle.Empty;
                        alarm.SetSafeZone(safeZone);
                    }
                }

                Utillities.UpdateAllObjects(objects, gameTime, viewport); //updates all the objects on the screen
            }
            else //if the player lost, a left button click restarts the level
            {
                while (Mouse.GetState().LeftButton == ButtonState.Pressed);
                while (Mouse.GetState().LeftButton == ButtonState.Released);
                RestartLevel();
            }
        }

        /// <summary>
        /// Load content
        /// loads all the content
        /// </summary>
        public override void LoadContent()
        {
            //loads all the textures for the level, and sets the spawn point for the player
            Texture2D SpriteTexture = Content.Load<Texture2D>("Sprites/monkey");
            Vector2 pos = playerInitial;
            Texture2D DollarTexture = Content.Load<Texture2D>("Sprites/money");
            Texture2D AlarmTexture = Content.Load<Texture2D>("Sprites/alarm");
            diedMonkey = Content.Load<Texture2D>("Sprites/monkeydeath");
            backgroundTexture = Content.Load<Texture2D>("backgrounds/firstlevelbg");
            //loads the sounds for collision with alarms and money
            alarmhit = Content.Load<SoundEffect>("SoundFX/alarmhit");
            moneycollect = Content.Load<SoundEffect>("SoundFX/moneypop");
            //creats the player character and sets its speed
            player = new Player(SpriteTexture, pos);
            player.speed = playerSpeed;

            //spawns the dollar and alarm objects in random positions on the screen outside the safe zone
            objects.AddRange(Utillities.CreateListOfInteractiveObjectsInRandomPositionsOutsideSafeZone(numberOfDollars[manager.diff], DollarTexture, viewport, "dollar", safeZone));
            List<InteractiveObject> alarms = Utillities.CreateListOfInteractiveObjectsInRandomPositionsWithVelocityOutsideSafeZoneAndSomeOtherUnimportantWordsThatTheirPurposeIsToCreateTheLongesterFunctionNameEver111111111111(numberOfAlarms[manager.diff], AlarmTexture, viewport, "alarm", -alarmspeed, alarmspeed, safeZone);
           
            foreach (InteractiveObject alarm in alarms)
            {
                alarm.SetSafeZone(safeZone);
                alarm.SetElastic(true);
            }
            objects.AddRange(alarms);
            //loads the background music and plays it
            backgroundMusic = Content.Load<Song>("BGM/Level1Music");
            MediaPlayer.Play(backgroundMusic);

            //Load objects to objects' list
            objects.Add(player);
            initialScores = manager.score.scores;
            
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

        //function that draws the background
        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, viewport.Width, viewport.Height);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);

        }
        #endregion
    }
}
