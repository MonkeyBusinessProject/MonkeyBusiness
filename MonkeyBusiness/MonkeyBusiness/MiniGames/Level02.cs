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
    // TODO: change class name
    // TODO: add your mini game to the manager
    class Level02 : MiniGame
    {
        #region Fields

        Player player;
        InteractiveObject trashcan;

        const int numberOfTrashes = 10, scoreForTrash = 100, totalScores = scoreForTrash * numberOfTrashes, trashMovementTime = 1; //sets the number of trashes to spawn, how much score is given for each trash that is cleaned and how much score is needed to win, and how long a trash moves after collision
        int initialScores;
        List<DrawableObject> objects = new List<DrawableObject>(); //list of all spawned objects
        private GameTime gameTime;
        private SoundEffect trashInCan; //sound for collision between trash and trashcan
        private SoundEffect trashKick; //sound for collision between player and trash

        private Song backgroundMusic;


        //Timer
        Timer timer = new Timer();
        private int timeLimit = 60;

        #endregion

        #region gameplay
        //checks to see if a trash hits the trashcan, if so score is given to the player, the trash is removed and a soundbyte is played
        private void CheckCollisionTrashToTrashcan()
        {
            List<DrawableObject> trashInTrashCan = Utillities.GetColliadedObjects(trashcan, objects, "trash");
            manager.score.addScores(trashInTrashCan.Count * scoreForTrash);
            if (trashInTrashCan.Count != 0)
            {
                trashInCan.Play();
            }
            Utillities.RemoveNodesFromList<DrawableObject>(objects, trashInTrashCan);
        }
        //checks to see if the player hits a trash, if so the trash then gets momentum in the player's movement direction at the moment of collision, for a set period of time, and a soundbyte is played
        private void CheckCollisionPlayerWithTrash()
        {
            List<DrawableObject> trashColiddadWithPlayer = Utillities.GetColliadedObjects(player, objects, "trash");
            foreach (DrawableObject trash in trashColiddadWithPlayer)
            {
                Vector2 direction = new Vector2((trash as InteractiveObject).center.X - player.center.Y, (trash as InteractiveObject).center.Y - player.center.Y);
                (trash as InteractiveObject).MoveByVector(player.GetVelocity() * 2, trashMovementTime, gameTime);
                trashKick.Play();
            }
        }
        //checks to see if all trash has been removed based on the total score achieved
        private void CheckWinning()
        {
            if (manager.score.scores == totalScores + initialScores)
                manager.SetNextMiniGameAsCurrent();
        }

        //checks to see if time has run out before winning
        private void CheckIfTimePassed()
        {
            if (timer.seconds <= 0)
                RestartLevel();
        }
        // restarts the level and resets the score and timer
        private void RestartLevel()
        {
            manager.score.scores = initialScores;
            timer = new Timer();
            manager.RestartMiniGame();
        }

        #endregion

        #region basic functions
        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="manager">The game state manager</param>
        public Level02(Manager manager)
            : base(manager)
        {

        }


        public override void Initialize()
        {
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
            //handls input from the player, and collision between player and trash, trash and trashcan, checks of the time has run out and if the player won by cleaning the entire level from trash
            this.gameTime = gameTime;
            player.HandleInput(true);
            CheckCollisionPlayerWithTrash();
            CheckCollisionTrashToTrashcan();
            CheckIfTimePassed();
            CheckWinning();
            Utillities.UpdateAllObjects(objects, gameTime, viewport);

            //if the timer is not working, a new timer is created, otherwise it is updated based on the elapsed game time
            if (!timer.isWorking)
                timer = new Timer(manager.score.font, gameTime, timeLimit);
            else
                timer.Update(gameTime);
        }

        /// <summary>
        /// Load content
        /// loads all the content
        /// </summary>
        public override void LoadContent()
        {

           
            device = graphics.GraphicsDevice;
            //loads all the textures for the level
            backgroundTexture = Content.Load<Texture2D>("backgrounds/officebackground");
            Texture2D TrashTexture = Content.Load<Texture2D>("Sprites/trash");
            Texture2D MonkeyTexture = Content.Load<Texture2D>("Sprites/monkey");
            Texture2D CanTexture = Content.Load<Texture2D>("Sprites/TrashCan");

            //loads the sound effects for collisions
            trashInCan = Content.Load<SoundEffect>("SoundFX/TrashInCan");
            trashKick = Content.Load<SoundEffect>("SoundFX/TrashKick");
            //creates the spawn point for the player and the trash can
            Vector2 monkeyPos = Utillities.RandomPosition(viewport, MonkeyTexture.Bounds);
            Vector2 canPos = new Vector2(viewport.Bounds.Center.X, viewport.Bounds.Center.Y);
            //creats the player and trash can in the previously defined spawn points
            player = new Player(MonkeyTexture, monkeyPos);
            player.speed = 0.13f; //the player's movement speed
            trashcan = new InteractiveObject(CanTexture, canPos, "trashCan");
            objects.AddRange(Utillities.CreateListOfInteractiveObjectsInRandomPositions(numberOfTrashes, TrashTexture, viewport, "trash", true)); 

            //TODO: Load to objects' list
            objects.Add(trashcan);
            objects.Add(player);

            initialScores = manager.score.scores;//loads the starting score, depending on the score in the previous level
            //loads the background music and plays it
            backgroundMusic = Content.Load<Song>("BGM/Level2Music");
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

        //creats the background for the level
        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, viewport.Width, viewport.Height);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);

        }
        #endregion
    }
}

