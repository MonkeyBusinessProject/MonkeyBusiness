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
        Queue<MiniGame> miniGames = new Queue<MiniGame>();
        Queue<MiniGame> miniGamesDone = new Queue<MiniGame>();
        MiniGame miniGame;
        public int count = 0;// TODO: remove, just for debugging
        bool isRunning = false;
        public Score score;

        #region get and set
        public GraphicsDeviceManager getGraphicDevice()
        {
            return graphics;
        }

        public SpriteBatch getSpriteBatch()
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

            score = new Score(Content.Load<SpriteFont>("Score"));

            AddAllMiniGames();

            FirstMiniGame();
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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //System.Diagnostics.Debug.WriteLine("stuff");
            if(isRunning)
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

            if(isRunning)
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

            if(isRunning)
                miniGame.Draw();

            base.Draw(gameTime);
        }

        #endregion

        #region Manager

        private void AddAllMiniGames()
        {
            // TODO: Add all mini-games
            miniGames.Enqueue(new Level01(this));
        }

        private void FirstMiniGame()
        {
            try
            {
                miniGame = miniGames.First<MiniGame>();
                miniGame.Initialize();
                isRunning = true;
            }
            catch (Exception ex)
            {
                isRunning = false;
                //TODO: stop running alert
            }
        }

        private void NextMiniGame()
        {
            try
            {
                miniGame.UnloadContent();
                miniGamesDone.Enqueue(miniGame);
                miniGames.Dequeue();
                FirstMiniGame();
                miniGame.LoadContent();
            }
            catch (Exception ex)
            {
                isRunning = false;
            }
        }

        #endregion

    }
}
