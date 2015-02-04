﻿using System;
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
        public static Random rnd = new Random(DateTime.Now.Millisecond);
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
            foreach (DrawableObject interactiveObject in objects)
                if(interactiveObject is InteractiveObject)
                    (interactiveObject as InteractiveObject).Update(gameTime, viewport);
        }

        /// <summary>
        /// Returns random position inside the screen
        /// </summary>
        /// <param name="viewport"></param>
        /// <param name="textureDimentions"></param>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static Vector2 RandomPosition(Viewport viewport, Rectangle textureDimentions)
        {
            return new Vector2(rnd.Next(0, (int)(viewport.Width - textureDimentions.Width)), rnd.Next(0, (int)(viewport.Height - textureDimentions.Height)));
        }

        /// <summary>
        /// Creates a list of a specific number of interactive objects with certian texture
        /// </summary>
        /// <param name="numberOfObjects"></param>
        /// <param name="texture"></param>
        /// <param name="viewport"></param>
        /// <returns></returns>
        public static List<InteractiveObject> CreateListOfInteractiveObjectsInRandomPositions(int numberOfObjects, Texture2D texture, Viewport viewport)
        {
            List<InteractiveObject> list = new List<InteractiveObject>();
            Random rnd = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < numberOfObjects; i++)
            {
                list.Add(new InteractiveObject(texture, RandomPosition(viewport, texture.Bounds)));
            }
            return list;
        }
        
        /// <summary>
        /// Creates a list of a specific number of interactive objects with certian texture
        /// </summary>
        /// <param name="numberOfObjects"></param>
        /// <param name="texture"></param>
        /// <param name="viewport"></param>
        /// <returns></returns>
        public static List<InteractiveObject> CreateListOfInteractiveObjectsInRandomPositions(int numberOfObjects, Texture2D texture, Viewport viewport, string type)
        {
            List<InteractiveObject> list = new List<InteractiveObject>();
            Random rnd = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < numberOfObjects; i++)
            {
                list.Add(new InteractiveObject(texture, RandomPosition(viewport, texture.Bounds), type));
            }
            return list;
        }

        /// <summary>
        /// This function removes a list of nodes from the source list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="toRemove"></param>
        public static void RemoveNodesFromList<T>(List<T> source, List<T> toRemove)
        {
            foreach (T item in toRemove)
            {
                if (source.Contains(item))
                    source.Remove(item);
            }
        }


        public static List<DrawableObject> GetColliadedObjects(InteractiveObject mainObject, List<DrawableObject> objects, string type)
        {
            List<DrawableObject> collidadObjects = new List<DrawableObject>();
            foreach (DrawableObject interactiveObject in objects)
            {
                if (interactiveObject is InteractiveObject)
                {
                    if (mainObject.BoundingBox.Intersects((interactiveObject as InteractiveObject).BoundingBox) && (interactiveObject as InteractiveObject).type == type)
                    {
                        collidadObjects.Add(interactiveObject);
                    }
                }
            }
            return collidadObjects;
        }
    }
}
