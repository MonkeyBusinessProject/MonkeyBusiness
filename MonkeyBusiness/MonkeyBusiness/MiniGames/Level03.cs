﻿using System;
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
    // TODO: add your mini game to the manager
    class Level03 : MiniGame
    {
        #region Fields
        List<DrawableObject> objects = new List<DrawableObject>();
        const int numberOfCollectors = 4, collectorsHeight = 50;// TODO: change
        int initialScores, widthOfAColumn = 100;
        KeyboardState lastKeyboardState = Keyboard.GetState();
        private SpriteFont font;
        #endregion

        #region gameplay fields
        float gravity = 0.1f;
        private int initialHeight = 10;
        InteractiveObject[] collectors = new InteractiveObject[numberOfCollectors];
        const int scoresForNote = 100, numberOfNotes = 10, totalScores = scoresForNote * numberOfNotes;
        #endregion

        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="manager">The game state manager</param>
        public Level03(Manager manager)
            : base(manager)
        {
            
        }

        #region gameplay


        private void CheckWinning()
        {
            if (manager.score.scores == totalScores + initialScores)
                manager.SetNextMiniGameAsCurrent();
        }

        private void CreateNoteCollectors(Texture2D texture)
        {
            widthOfAColumn = viewport.Width / (numberOfCollectors + 1);
            for (int i = 0; i < numberOfCollectors; i++)
            {
                Vector2 position = new Vector2(widthOfAColumn * (i + 1), viewport.Height - collectorsHeight);
                InteractiveObject collector = new InteractiveObject(texture, position, "collector" + i);
                objects.Add(collector);
                collectors[i] = collector;
            }
        }

        private void CreateNote(Texture2D texture, int column)
        {
            Vector2 position = new Vector2(widthOfAColumn * (column + 1), initialHeight);
            InteractiveObject note = new InteractiveObject(texture, position, "note");
            note.SetVelocity(0, gravity);
            objects.Add(note);
        }

        private void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            int numberPressed = Utillities.KeyboardNumberPressed(keyboardState, lastKeyboardState);
            numberPressed--;
            if (numberPressed >= 0 && numberPressed < collectors.Length)
            {
                List<DrawableObject> collidadNotes = Utillities.GetColliadedObjects(collectors[numberPressed], objects, "note");
                if (collidadNotes.Count == 0)
                {
                    manager.score.scores = initialScores;
                    manager.RestartMiniGame();
                    //TODO : RESTART
                }
                else
                {
                    foreach (DrawableObject note in collidadNotes)
                    {
                        objects.Remove(note);
                        manager.score.addScores(scoresForNote);
                    }
                }
            }
            lastKeyboardState = keyboardState;
        }

        #endregion

        #region basic functions
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

            Utillities.DrawAllObjects(objects, manager.score, spriteBatch);


            for (int i = 0; i < numberOfCollectors; i++)
            {
                Vector2 position = new Vector2(widthOfAColumn * (i + 1) + 10, viewport.Height - collectorsHeight);
                String name = (i + 1).ToString();
                spriteBatch.DrawString(font, name, position, Color.Black);
            }


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
            HandleInput();

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
            //TODO: Load Content
            /*Example:
            SpriteTexture = Content.Load<Texture2D>("monkey");
            Vector2 pos = new Vector2(100, 100);
            player = new Player(SpriteTexture, pos);
            */

            Texture2D NoteTexture = Content.Load<Texture2D>("note");
            Texture2D NoteCollectorTexture = Content.Load<Texture2D>("notesCollector");
            font = Content.Load<SpriteFont>("GameFont");

            CreateNoteCollectors(NoteCollectorTexture);
            CreateNote(NoteTexture, 0);

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
    }
}
