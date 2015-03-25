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
    class MainMenu : MiniGame
    {
        #region fields
        private Song backgroundMusic;//Music
        private Texture2D txtrBackground;//Background
        private Texture2D txtrMonkey, txtrAlfredo, txtrLogo;//Sprites
        private SpriteFont fontItem;//Fonts

        //Positions
        Vector2 posMonkey = new Vector2(50, 20),
            posAlfredo = new Vector2(650, 20),
            posLogo = new Vector2(250, 20),
            posFirstItem = new Vector2(140, 20);
        int wdtMonkey = 100,
            wdtAlfredo = 100;
        Rectangle recMonkey, recAlfredo;
        float heightBetweenItems = 1.3f;

        //Menu Items
        List<string> items = new List<string>();
        int chosenItem = 0;
        
        //Keyboard
        KeyboardState lastState = Keyboard.GetState();

        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="manager">The game state manager</param>
        public MainMenu(Manager manager) : base(manager)
        {
        }
        #endregion

        private void LoadMenuItems()
        {
            items.Add("Play");
            items.Add("Exit");
        }

        private void HandleInput()
        {
            KeyboardState keyboard = Keyboard.GetState();

            //Move Item
            if (keyboard.IsKeyDown(Keys.Down) && lastState.IsKeyUp(Keys.Down))
                chosenItem++;
            else if(keyboard.IsKeyDown(Keys.Up) && lastState.IsKeyUp(Keys.Up))
                chosenItem--;
            if (chosenItem == -1)
                chosenItem = items.Count - 1;
            if (chosenItem == items.Count)
                chosenItem = 0;

            //Choose item
            if (keyboard.IsKeyDown(Keys.Enter) && lastState.IsKeyUp(Keys.Enter))
            {
                if (items[chosenItem] == "Play")
                {
                    manager.SetNextMiniGameAsCurrent();
                }
                else if (items[chosenItem] == "Exit")
                {
                    manager.Exit();
                }
            }

            lastState = keyboard;
        }

        private void StretchSprites()
        {
            //Monkey
            recMonkey = StretchASprite(txtrMonkey, posMonkey, wdtMonkey);

            //Alfredo
            recAlfredo = StretchASprite(txtrAlfredo, posAlfredo, wdtAlfredo);
        }

        private Rectangle StretchASprite(Texture2D sprite, Vector2 position, int width)
        {
            float ratio = width / sprite.Width;
            Rectangle rectangle = new Rectangle((int)(position.X), (int)(position.Y), (int)(width), (int)(sprite.Height * ratio));
            return rectangle;
        }
        #region basic functions

        
        public override void Initialize() {
            manager.IsMouseVisible = true; //makes the mouse visible
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

            spriteBatch.Draw(txtrMonkey, recMonkey, Color.White);
            spriteBatch.Draw(txtrAlfredo, recAlfredo, Color.White);
            spriteBatch.Draw(txtrLogo, posLogo, Color.White);
            posFirstItem.X = posLogo.X + 0.3f * txtrLogo.Width;
            posFirstItem.Y = posLogo.Y + txtrLogo.Height + heightBetweenItems * fontItem.MeasureString("a").Y;
            float heightDif = 0;
            foreach (string menuItem in items)
            {
                Color color;
                if (items[chosenItem] == menuItem)
                    color = Color.Brown;
                else
                    color = Color.Green;
                Vector2 position = new Vector2(posFirstItem.X, posFirstItem.Y + heightDif);
                spriteBatch.DrawString(fontItem, menuItem, position, color);
                heightDif += fontItem.MeasureString(menuItem).Y * heightBetweenItems;
            }


            spriteBatch.End();
        }

        /// <summary>
        /// Update all objects' state
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            HandleInput();
        }

        /// <summary>
        /// Load content
        /// loads all the content
        /// </summary>
        public override void LoadContent()
        {
            //loads all the textures for the level
            txtrMonkey = Content.Load<Texture2D>("Sprites/monkey");
            txtrAlfredo = Content.Load<Texture2D>("Sprites/alfredo");
            txtrBackground = Content.Load<Texture2D>("backgrounds/MenuBackground");
            txtrLogo = Content.Load<Texture2D>("Menu/logo");

            //load all fonts
            fontItem = Content.Load<SpriteFont>("MenuItem");

            //loads the background music and plays it
            backgroundMusic = Content.Load<Song>("BGM/MenuMusic");
            MediaPlayer.Play(backgroundMusic);

            LoadMenuItems();
            StretchSprites();
        }

        /// <summary>
        /// Unload content (if needed)
        /// </summary>
        public override void UnloadContent()
        {
            items.Clear();
        }

        #endregion

        #region useful functions

        //function that draws the background
        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, viewport.Width, viewport.Height);
            spriteBatch.Draw(txtrBackground, screenRectangle, Color.White);
        }
        #endregion
    }
}
