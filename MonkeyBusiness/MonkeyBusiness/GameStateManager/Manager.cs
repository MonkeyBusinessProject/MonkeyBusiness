using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonkeyBusiness.MiniGames;
using MonkeyBusiness.Objects;
using MonkeyBusiness;

namespace GameStateManager
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Manager : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Engine fields
        /// </summary>
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        /// <summary>
        /// Minigame choosing
        /// </summary>
        Stack<MiniGame> miniGames = new Stack<MiniGame>();
        Stack<MiniGame> miniGamesDone = new Stack<MiniGame>();
        MiniGame miniGame;
        /// <summary>
        /// Gameplay fields
        /// </summary>
        bool isRunning = false;
        public Score score;

        #region get and set
        /// <summary>
        /// Returns the graphic device of the manager.
        /// </summary>
        /// <returns></returns>
        public GraphicsDeviceManager GetGraphicDevice()
        {
            return graphics;
        }

        /// <summary>
        /// Returns the spritebatch of the manager.
        /// </summary>
        /// <returns></returns>
        public SpriteBatch GetSpriteBatch()
        {
            return spriteBatch;
        }
        #endregion

        #region Constractor and Initialize
        /// <summary>
        /// Creates the manager. set the graphic device and load the content folder.
        /// </summary>
        public Manager()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// 
        /// Load all minigames to the manager.
        /// </summary>
        protected override void Initialize()
        {

            AddAllMiniGames();

            SetFirstMiniGameAsCurrent();
            base.Initialize();
        }

        #endregion

        #region Content
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            score = new Score(Content.Load<SpriteFont>("Score"));

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            if (isRunning)
                miniGame.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            if (isRunning)
                miniGame.UnloadContent();
        }

        #endregion

        #region Draw and Update
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (isRunning)
                miniGame.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (isRunning)
                miniGame.Draw();

            base.Draw(gameTime);
        }

        #endregion

        #region Manager

        /// <summary>
        /// This function loads all minigames to a stack.
        /// </summary>
        private void AddAllMiniGames()
        {
            // TODO: Add all mini-games
            //Add in reversed order

            //miniGames.Push(new LevelLast(this));
            //miniGames.Push(new PreUsingKeys(this));
            //miniGames.Push(new StarWars4(this));
            //miniGames.Push(new Level03(this));
            //miniGames.Push(new PreGuitar(this));
            //miniGames.Push(new StarWars3(this));
            //miniGames.Push(new Level02(this));
            //miniGames.Push(new PreUsingMouse(this));
            //miniGames.Push(new StarWars2(this));
            miniGames.Push(new Level01(this));
            //miniGames.Push(new PreUsingMouse(this));
            //miniGames.Push(new StarWars1(this));
        }

        /// <summary>
        /// This function sets the first minigame as the current minigame.
        /// </summary>
        private void SetFirstMiniGameAsCurrent()
        {
            if (miniGames.Count != 0)
            {
                miniGame = miniGames.First<MiniGame>();
                miniGame.Initialize();
                isRunning = true;
            }
            else
            {
                isRunning = false;
                //TODO: stop running alert
            }
        }

        /// <summary>
        /// This function restarts the current minigame
        /// </summary>
        public void RestartMiniGame()
        {
            miniGame.UnloadContent();
            miniGame.Initialize();
            miniGame.LoadContent();
        }

        /// <summary>
        /// This function starts the next minigame.
        /// </summary>
        public void SetNextMiniGameAsCurrent()
        {
            if (miniGames.Count != 0)
            {
                miniGame.UnloadContent();
                miniGamesDone.Push(miniGames.Pop());
                SetFirstMiniGameAsCurrent();
                miniGame.Initialize();
                miniGame.LoadContent();
            }
            else
            {
                isRunning = false;
            }
        }

        /// <summary>
        /// This function goes one minigame backward.
        /// </summary>
        public void SetPreviousMiniGameAsCurrent()
        {
            if (miniGamesDone.Count != 0)
            {
                miniGame.UnloadContent();
                miniGames.Push(miniGamesDone.Pop());
                SetFirstMiniGameAsCurrent();
                miniGame.Initialize();
                miniGame.LoadContent();
            }
            else
            {
                isRunning = false;
            }
        }

        #endregion

    }
}
