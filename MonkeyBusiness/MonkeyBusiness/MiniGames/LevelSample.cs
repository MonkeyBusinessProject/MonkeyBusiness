using System;
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
    //TODO : change class name
    //TODO: add your mini game to the manager
    class LevelSample : MiniGame
    {
        #region Fields
        List<DrawableObject> objects = new List<DrawableObject>();
        #endregion

        public LevelSample(Manager manager)
            : base(manager)
        {

        }

        public override void Initialize()
        {
            manager.IsMouseVisible = true;//Or not...
        }

        public override void Draw()
        {
            GetGraphicDevice();
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            foreach (DrawableObject drawableObject in objects)
                drawableObject.Draw(spriteBatch);
            manager.score.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            //TODO: Handle input
            //Example:          player.HandleInput();
            foreach (InteractiveObject thisObject in objects)
                thisObject.Update(gameTime);
        }

        public override void LoadContent()
        {
            //TODO: Load Content
            /*Example:
            SpriteTexture = Content.Load<Texture2D>("monkey");
            Vector2 pos = new Vector2(100, 100);
            player = new Player(SpriteTexture, pos);
            */

            //TODO: Load to objects' list
            //Example:            objects.Add(player);
        }

        public override void UnloadContent() { }
    }
}
