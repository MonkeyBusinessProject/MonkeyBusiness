using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MonkeyBusiness.Objects;
using Microsoft.Xna.Framework.Graphics;
using GameStateManager;

namespace MonkeyBusiness
{
    class Utillities
    {
        /// <summary>
        /// Convert a Point to a Vector2
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>The point as Vector2</returns>
        public static Vector2 PointToVector2(Point point)
        {
            float x = point.X, y = point.Y;
            Vector2 vector = new Vector2(x, y);
            return vector;
        }

        /// <summary>
        /// Convert a Vector2 to a Point
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <returns>The Vector as Point</returns>
        public static Point Vector2ToPoint(Vector2 vector)
        {
            int x = (int)(vector.X), y = (int)(vector.Y);
            Point point = new Point(x, y);
            return point;
        }

        /// <summary>
        /// Normalize Vector2. If the vector is (0, 0), return (0, 0)
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <returns>Normalized Vector</returns>
        public static Vector2 Normalize(Vector2 vector)
        {
            if (vector.Length() < 1)
                return vector;
            else
                return Vector2.Normalize(vector);
        }

        /// <summary>
        /// Draw all objects in an objects' list and the scores
        /// </summary>
        /// <param name="objects">List of objects</param>
        /// <param name="score">The scores</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        public static void DrawAllObjects(List<DrawableObject> objects, Score score, SpriteBatch spriteBatch)
        {
            foreach (DrawableObject drawableObject in objects)
                drawableObject.Draw(spriteBatch);
            score.Draw(spriteBatch);
        }

        /// <summary>
        /// Draw all objects in an objects' list
        /// </summary>
        /// <param name="objects">List of objects</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        public static void DrawAllObjects(List<DrawableObject> objects, SpriteBatch spriteBatch, Manager manager)
        {
            foreach (DrawableObject drawableObject in objects)
                drawableObject.Draw(spriteBatch);
        }

        /// <summary>
        /// Update all objects in an objects' list
        /// </summary>
        /// <param name="objects">List of objects</param>
        /// <param name="gameTime">GameTime</param>
        /// <param name="viewport">ViewPort</param>
        public static void UpdateAllObjects(List<DrawableObject> objects, GameTime gameTime, Viewport viewport)
        {
            foreach (InteractiveObject interactiveObject in objects)
                interactiveObject.Update(gameTime, viewport);
        }
    }
}
