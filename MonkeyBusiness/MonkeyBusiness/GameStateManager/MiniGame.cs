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
        protected GraphicsDevice device;
        protected SpriteBatch spriteBatch;
        protected ContentManager Content;
        protected Viewport viewport;
        protected Texture2D backgroundTexture;
        protected Texture2D foregroundTexture;
       
 

        public MiniGame(Manager manager)
        {
            this.manager = manager;
            UpdateGraphicDevices();
            this.Content = manager.Content;
        }
        public void UpdateGraphicDevices()
        {
            try
            {
                this.graphics = manager.GetGraphicDevice();
                this.spriteBatch = manager.GetSpriteBatch();
                this.viewport = manager.GraphicsDevice.Viewport;
            }catch(Exception ex){

            }
        }
        public abstract void Initialize();
        public abstract void Draw();
        public abstract void Update(GameTime gameTime);
        public abstract void LoadContent();
        public abstract void UnloadContent();
    }
}