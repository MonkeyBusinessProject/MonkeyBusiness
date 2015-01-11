using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonkeyBusiness
{
    class Utillities
    {
        public static Vector2 PointToVector2(Point point)
        {
            float x = point.X, y = point.Y;
            Vector2 vector = new Vector2(x, y);
            return vector;
        }

        public static Point Vector2ToPoint(Vector2 vector)
        {
            int x = (int)(vector.X), y = (int)(vector.Y);
            Point point = new Point(x, y);
            return point;
        }
    }
}
