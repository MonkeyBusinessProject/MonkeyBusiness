using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManager;
using Microsoft.Xna.Framework;
using MonkeyBusiness.Objects;
using Microsoft.Xna.Framework.Graphics;

namespace MonkeyBusiness.MiniGames
{
    class Level01 : MiniGame
    {
        Player player;
        private Texture2D SpriteTexture;

        public Level01(Manager manager) : base(manager)
        {

        }
        public override void Initialize() {
            Vector2 pos = new Vector2(100, 100);
            player = new Player(SpriteTexture, pos);
            player.GoToTarget(new Vector2(1000, 1000));
        }
        public override void Draw(GameTime gameTime)
        {
            GetGraphicDevice();
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            spriteBatch.End();
        }
        public override void Update(GameTime gameTime) {
            player.Update(gameTime);
        }
        public override void LoadContent()
        {
            SpriteTexture = Content.Load<Texture2D>("monkey");
        }
        public override void UnloadContent() { }
    }
}
