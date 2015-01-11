using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameStateManager
{
    public abstract class MiniGame
    {
        protected Manager manager;
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        protected ContentManager Content;

        public MiniGame(Manager manager)
        {
            this.manager = manager;
            GetGraphicDevice();
            this.Content = manager.Content;
        }
        public void GetGraphicDevice()
        {
            this.graphics = manager.getGraphicDevice();
            this.spriteBatch = manager.getSpriteBatch();
        }
        public abstract void Initialize();
        public abstract void Draw();
        public abstract void Update(GameTime gameTime);
        public abstract void LoadContent();
        public abstract void UnloadContent();
    }
}