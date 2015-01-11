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
    class Level01 : MiniGame
    {
        Player player;
        private Texture2D SpriteTexture;
        List<DrawableObject> objects = new List<DrawableObject>();

        public Level01(Manager manager) : base(manager)
        {

        }

        public override void Initialize() {
            manager.IsMouseVisible = true;
        }

        public override void Draw()
        {
            GetGraphicDevice();
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            foreach(DrawableObject drawableObject in objects)
                drawableObject.Draw(spriteBatch);
            manager.score.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            player.HandleInput();
            foreach(InteractiveObject interactiveObject in objects)
                interactiveObject.Update(gameTime);
        }

        public override void LoadContent()
        {
            SpriteTexture = Content.Load<Texture2D>("monkey");
            Vector2 pos = new Vector2(100, 100);


            player = new Player(SpriteTexture, pos);

            //Load to objects' list
            objects.Add(player);
        }

        public override void UnloadContent() { }
    }
}
