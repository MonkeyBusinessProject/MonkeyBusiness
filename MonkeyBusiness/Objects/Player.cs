using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonkeyBusiness;

namespace MonkeyBusiness.Objects
{
    class Player : InteractiveObject
    {
        double speed;
        Vector2 target;
        bool isMovingToTarget = false;

        public Player(Texture2D texture, Vector2 position)
            : base(texture, position)
        {

        }

        public void GoToTarget(Vector2 target)
        {
            this.target = target;
            isMovingToTarget = true;
            this.SetVelocity((float)(speed) * Vector2.Normalize(this.target - Utillities.PointToVector2(this.center)));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.position == this.target)
            {
                isMovingToTarget = false;
                this.SetVelocity(Vector2.Zero);
            }
        }
    }
}
