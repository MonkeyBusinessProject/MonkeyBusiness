﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManager;
using Microsoft.Xna.Framework;
using MonkeyBusiness.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Media;

namespace MonkeyBusiness.MiniGames
{
    // TODO: add your mini game to the manager
    class Level03 : MiniGame
    {
        #region Fields
        List<DrawableObject> objects = new List<DrawableObject>();
        const int numberOfCollectors = 5, collectorsHeight = 50;// TODO: change
        int initialScores, widthOfAColumn = 100;
        KeyboardState lastKeyboardState = Keyboard.GetState();
        private SpriteFont font;

        private Song backingTrack;
        private SoundEffect[] notesSounds;
        #endregion

        #region gameplay fields
        string xmlDirectory;
        float gravity = 0.15f;
        int distanceBetweenNotes;
        private int initialHeight = 10;
        int currentHeight;
        InteractiveObject[] collectors = new InteractiveObject[numberOfCollectors];
        const int scoresForNote = 100, scoreForMissedNote = -100;
        int numberOfNotes = 10, totalScores;
        List<int> notes = new List<int>();
        int length = 100, freqOfPauses = 10, restLength = 4;
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
            if (Utillities.GetObjectsFromType(objects, "note").Count == 0)
                if (manager.score.scores > 0)
                    manager.SetNextMiniGameAsCurrent();
                else
                    manager.RestartMiniGame();
        }

        private void CheckOutsideScreenNotes()
        {
            List<DrawableObject> notes = Utillities.GetObjectsFromType(objects, "note");
            if (notes.Count != 0)
                if ((notes.First<DrawableObject>() as InteractiveObject).center.Y >= viewport.Height)
                {
                    objects.Remove(notes.First<DrawableObject>());
                    manager.score.addScores(scoreForMissedNote);
                }
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

        private void CreateNote(Texture2D texture, int column, int height)
        {
            if (column != 0)
            {
                Vector2 position = new Vector2(widthOfAColumn * column, height);
                InteractiveObject note = new InteractiveObject(texture, position, "note");
                note.SetOutsideScreen(true);
                note.SetVelocity(0, gravity);
                objects.Add(note);
            }
        }

        private void CreateAllNotes(Texture2D texture)
        {
            distanceBetweenNotes = 2 * texture.Bounds.Height;
            foreach (int column in notes)
            {
                CreateNote(texture, column, currentHeight);
                if (column != 0)
                    currentHeight -= distanceBetweenNotes;
                else
                    currentHeight -= distanceBetweenNotes / 2;
            }
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
                    manager.score.addScores(scoreForMissedNote);
                    playNote(0);
                }
                else
                {
                    foreach (DrawableObject note in collidadNotes)
                    {
                        playNote(numberPressed + 1);
                        objects.Remove(note);
                        manager.score.addScores(scoresForNote);
                    }
                }
            }
            lastKeyboardState = keyboardState;
        }

        private void playNote(int note)
        {
            notesSounds[note].Play();
        }

        private List<int> RandomNotesList(int length, int freqOfPauses)
        {
            Random rnd = Utillities.rnd;
            List<int> notesTemp = new List<int>();
            for (int i = 0; i < length; i++)
            {
                if (rnd.Next(0, freqOfPauses) != 0)
                    notesTemp.Add(rnd.Next(1, numberOfCollectors + 1));
                else if (rnd.Next(0, (int)(freqOfPauses / 2)) != 0)
                    notesTemp.Add(0);
                else
                    for (int j = 0; j < restLength; j++)
                    {
                        notesTemp.Add(0);
                    }
            }
            return notesTemp;
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
            currentHeight = initialHeight;

            ////////

            //notes.Add(1);
            //notes.Add(2);
            /*notes.Add(1);
            notes.Add(4);
            notes.Add(0);
            notes.Add(3);
            notes.Add(2);
            notes.Add(1);*/
            try
            {
                xmlDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Guitar.xml";
                notes = Utillities.XMLFileToIntList(xmlDirectory);
            }
            catch (Exception)
            {
                notes = RandomNotesList(length, freqOfPauses);
            }
            numberOfNotes = notes.Count;
            totalScores = scoresForNote * numberOfNotes;

            ///////
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
            CheckOutsideScreenNotes();
            CheckWinning();
        }

        /// <summary>
        /// Load content
        /// Here you should:
        ///     1. Load objects' textures
        ///     2. Add all objects to the object' list
        /// </summary>
        public override void LoadContent()
        {
            Texture2D NoteTexture = Content.Load<Texture2D>("Sprites/note");
            Texture2D NoteCollectorTexture = Content.Load<Texture2D>("Sprites/notesCollector");
            backgroundTexture = Content.Load<Texture2D>("backgrounds/stage");
            font = Content.Load<SpriteFont>("GameFont");

            CreateNoteCollectors(NoteCollectorTexture);
            CreateAllNotes(NoteTexture);

            initialScores = manager.score.scores;

            //Load Music
            backingTrack = Content.Load<Song>("BGM/backingTrack");
            MediaPlayer.Play(backingTrack);
            MediaPlayer.Volume = 0.5f;

            //TODO
            notesSounds = new SoundEffect[numberOfCollectors + 1];
            for (int i = 0; i < numberOfCollectors + 1; i++)
            {
                try
                {
                    notesSounds[i] = Content.Load<SoundEffect>("BGM/notes/" + i);
                }
                catch (Exception)
                {
                    notesSounds[i] = Content.Load<SoundEffect>("BGM/notes/0");
                }
            }
            //TODO
        }

        /// <summary>
        /// Unload content (if needed)
        /// </summary>
        public override void UnloadContent()
        {
            objects.Clear();
            notes.Clear();
            manager.score.scores = initialScores;
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
