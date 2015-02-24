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
        // TODO: add all objects
        Player player;
        InteractiveObject trashcan;

        const int numberOfTrashes = 10, scoreForTrash = 100, totalScores = scoreForTrash * numberOfTrashes, trashMovementTime = 1;
        int initialScores;
        List<DrawableObject> objects = new List<DrawableObject>();
        private GameTime gameTime;
        private SoundEffect trashInCan;
        private SoundEffect trashKick;

        private Song backgroundMusic;
        #endregion

        #region gameplay

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
        private void CheckWinning()
        {
            if (manager.score.scores == totalScores + initialScores)
                manager.SetNextMiniGameAsCurrent();
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
            this.gameTime = gameTime;
            player.HandleInput(true);
            CheckCollisionPlayerWithTrash();
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

            Texture2D TrashTexture = Content.Load<Texture2D>("Sprites/trash");
            Texture2D MonkeyTexture = Content.Load<Texture2D>("Sprites/monkey");
            Texture2D CanTexture = Content.Load<Texture2D>("Sprites/TrashCan");
            trashInCan = Content.Load<SoundEffect>("SoundFX/TrashInCan");
            trashKick = Content.Load<SoundEffect>("SoundFX/TrashKick");

            Vector2 monkeyPos = Utillities.RandomPosition(viewport, MonkeyTexture.Bounds);
            Vector2 canPos = new Vector2(viewport.Bounds.Center.X, viewport.Bounds.Center.Y);

            player = new Player(MonkeyTexture, monkeyPos);
            player.speed = 0.25f;
            trashcan = new InteractiveObject(CanTexture, canPos, "trashCan");
            objects.AddRange(Utillities.CreateListOfInteractiveObjectsInRandomPositions(numberOfTrashes, TrashTexture, viewport, "trash"));

            //TODO: Load to objects' list
            objects.Add(trashcan);
            objects.Add(player);

            initialScores = manager.score.scores;

            backgroundMusic = Content.Load<Song>("BGM/Level2Music");
            MediaPlayer.Play(backgroundMusic);
        }



        /// <summary>
        /// Unload content (if needed)
        /// </summary>
        public override void UnloadContent()
        {
        


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

