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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Stack<MiniGame> miniGames = new Stack<MiniGame>();
        Stack<MiniGame> miniGamesDone = new Stack<MiniGame>();
        MiniGame miniGame;
        public int count = 0;// TODO: remove, just for debugging
        bool isRunning = false;
        public Score score;
        public Timer timer;

        #region get and set
        public GraphicsDeviceManager GetGraphicDevice()
        {
            return graphics;
        }

        public SpriteBatch GetSpriteBatch()
        {
            return spriteBatch;
        }
        #endregion

        #region Constractor and Initialize
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
            timer = new Timer(Content.Load<SpriteFont>("Timer"));

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //System.Diagnostics.Debug.WriteLine("stuff");
            if (isRunning)
                miniGame.LoadContent();

            // TODO: use this.Content to load your game content here
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

        private void AddAllMiniGames()
        {
            // TODO: Add all mini-games
            //Add in reversed order
            miniGames.Push(new LevelLast(this));
            miniGames.Push(new PreUsingKeys(this));
            //miniGames.Push(new Level03(this));
            miniGames.Push(new Level02(this));
            //miniGames.Push(new PreUsingMouse(this));
            //miniGames.Push(new Level01(this));
            //miniGames.Push(new PreUsingMouse(this));
            //miniGames.Push(new StarWars(this));
        }

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

        public void RestartMiniGame()
        {
            miniGame.UnloadContent();
            miniGame.LoadContent();
        }

        public void SetNextMiniGameAsCurrent()
        {
            if (miniGames.Count != 0)
            {
                miniGame.UnloadContent();
                miniGamesDone.Push(miniGames.Pop());
                SetFirstMiniGameAsCurrent();
                miniGame.LoadContent();
            }
            else
            {
                isRunning = false;
            }
        }

        public void SetPreviousMiniGameAsCurrent()
        {
            if (miniGamesDone.Count != 0)
            {
                miniGame.UnloadContent();
                miniGames.Push(miniGamesDone.Pop());
                SetFirstMiniGameAsCurrent();
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
