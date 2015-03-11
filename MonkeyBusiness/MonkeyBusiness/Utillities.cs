using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MonkeyBusiness.Objects;
using Microsoft.Xna.Framework.Graphics;
using GameStateManager;
using Microsoft.Xna.Framework.Input;
using System.Xml;

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
                if (interactiveObject is InteractiveObject)
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
        public static List<InteractiveObject> CreateListOfInteractiveObjectsInRandomPositionsWithVelocity(int numberOfObjects, Texture2D texture, Viewport viewport, string type, float minimumVelocity, float maximumVelocity)
        {
            List<InteractiveObject> list = new List<InteractiveObject>();
            Random rnd = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < numberOfObjects; i++)
            {
                InteractiveObject obj = new InteractiveObject(texture, RandomPosition(viewport, texture.Bounds), type);
                int intMinVelocity = (int)(minimumVelocity * 1000);
                int intMaxVelocity = (int)(maximumVelocity * 1000);
                obj.SetVelocity(new Vector2(rnd.Next(intMinVelocity, intMaxVelocity) / 1000f, rnd.Next(intMinVelocity, intMaxVelocity) / 1000f));
                list.Add(obj);
            }
            return list;
        }

        public static List<InteractiveObject> CreateListOfInteractiveObjectsInRandomPositionsWithVelocityOutsideSafeZone(int numberOfObjects, Texture2D texture, Viewport viewport, string type, float minimumVelocity, float maximumVelocity, Rectangle safeZone)
        {
            List<InteractiveObject> list = new List<InteractiveObject>();
            Random rnd = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < numberOfObjects; i++)
            {
                Vector2 position = RandomPosition(viewport, texture.Bounds);
                while(safeZone.Contains(Utillities.Vector2ToPoint(position)))
                    position = RandomPosition(viewport, texture.Bounds);
                InteractiveObject obj = new InteractiveObject(texture,position , type);
                int intMinVelocity = (int)(minimumVelocity * 1000);
                int intMaxVelocity = (int)(maximumVelocity * 1000);
                obj.SetVelocity(new Vector2(rnd.Next(intMinVelocity, intMaxVelocity) / 1000f, rnd.Next(intMinVelocity, intMaxVelocity) / 1000f));
                list.Add(obj);
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
        /// Creates a list of a specific number of interactive objects with certian texture and set them as elastic if passed "true"
        /// </summary>
        /// <param name="numberOfObjects"></param>
        /// <param name="texture"></param>
        /// <param name="viewport"></param>
        /// <returns></returns>
        public static List<InteractiveObject> CreateListOfInteractiveObjectsInRandomPositions(int numberOfObjects, Texture2D texture, Viewport viewport, string type, bool isElastic)
        {
            List<InteractiveObject> list = new List<InteractiveObject>();
            Random rnd = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < numberOfObjects; i++)
            {
                InteractiveObject obj = new InteractiveObject(texture, RandomPosition(viewport, texture.Bounds), type);
                obj.SetElastic(isElastic);
                list.Add(obj);
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

        public static List<DrawableObject> GetObjectsFromType(List<DrawableObject> objects, string type)
        {
            List<DrawableObject> objectFromType = new List<DrawableObject>();
            foreach (DrawableObject interactiveObject in objects)
            {
                if (interactiveObject is InteractiveObject)
                {
                    if ((interactiveObject as InteractiveObject).type == type)
                    {
                        objectFromType.Add(interactiveObject);
                    }
                }
            }
            return objectFromType;
        }

        public static int KeyboardNumberPressed(KeyboardState keyboardState, KeyboardState lastKeyboardState)
        {
            if ((keyboardState.IsKeyDown(Keys.D0) || keyboardState.IsKeyDown(Keys.NumPad0)) && (!lastKeyboardState.IsKeyDown(Keys.D0) && !lastKeyboardState.IsKeyDown(Keys.NumPad0)))
            {
                return 0;
            }
            if ((keyboardState.IsKeyDown(Keys.D1) || keyboardState.IsKeyDown(Keys.NumPad1)) && (!lastKeyboardState.IsKeyDown(Keys.D1) && !lastKeyboardState.IsKeyDown(Keys.NumPad1)))
            {
                return 1;
            }
            if ((keyboardState.IsKeyDown(Keys.D2) || keyboardState.IsKeyDown(Keys.NumPad2)) && (!lastKeyboardState.IsKeyDown(Keys.D2) && !lastKeyboardState.IsKeyDown(Keys.NumPad2)))
            {
                return 2;
            }
            if ((keyboardState.IsKeyDown(Keys.D3) || keyboardState.IsKeyDown(Keys.NumPad3)) && (!lastKeyboardState.IsKeyDown(Keys.D3) && !lastKeyboardState.IsKeyDown(Keys.NumPad3)))
            {
                return 3;
            }
            if ((keyboardState.IsKeyDown(Keys.D4) || keyboardState.IsKeyDown(Keys.NumPad4)) && (!lastKeyboardState.IsKeyDown(Keys.D4) && !lastKeyboardState.IsKeyDown(Keys.NumPad4)))
            {
                return 4;
            }
            if ((keyboardState.IsKeyDown(Keys.D5) || keyboardState.IsKeyDown(Keys.NumPad5)) && (!lastKeyboardState.IsKeyDown(Keys.D5) && !lastKeyboardState.IsKeyDown(Keys.NumPad5)))
            {
                return 5;
            }
            if ((keyboardState.IsKeyDown(Keys.D6) || keyboardState.IsKeyDown(Keys.NumPad6)) && (!lastKeyboardState.IsKeyDown(Keys.D6) && !lastKeyboardState.IsKeyDown(Keys.NumPad6)))
            {
                return 6;
            }
            if ((keyboardState.IsKeyDown(Keys.D7) || keyboardState.IsKeyDown(Keys.NumPad7)) && (!lastKeyboardState.IsKeyDown(Keys.D7) && !lastKeyboardState.IsKeyDown(Keys.NumPad7)))
            {
                return 7;
            }
            if ((keyboardState.IsKeyDown(Keys.D8) || keyboardState.IsKeyDown(Keys.NumPad8)) && (!lastKeyboardState.IsKeyDown(Keys.D8) && !lastKeyboardState.IsKeyDown(Keys.NumPad8)))
            {
                return 8;
            }
            if ((keyboardState.IsKeyDown(Keys.D9) || keyboardState.IsKeyDown(Keys.NumPad9)) && (!lastKeyboardState.IsKeyDown(Keys.D9) && !lastKeyboardState.IsKeyDown(Keys.NumPad9)))
            {
                return 9;
            }
            return -1;
        }
        public static string KeyboardArrowPressed(KeyboardState keyboardstate, KeyboardState lastkeyboardstate)
        {
            if (keyboardstate.IsKeyDown(Keys.Right) && !lastkeyboardstate.IsKeyDown(Keys.Right) && !keyboardstate.IsKeyDown(Keys.Left))
                return "Right";
            if (keyboardstate.IsKeyDown(Keys.Left) && !lastkeyboardstate.IsKeyDown(Keys.Left) && !keyboardstate.IsKeyDown(Keys.Right))
                return "Left";
            if (keyboardstate.IsKeyDown(Keys.Down) && !lastkeyboardstate.IsKeyDown(Keys.Down) && !keyboardstate.IsKeyDown(Keys.Up))
                return "Down";
            if (keyboardstate.IsKeyDown(Keys.Up) && !lastkeyboardstate.IsKeyDown(Keys.Up) && !keyboardstate.IsKeyDown(Keys.Down))
                return "Up";
            return null;
        }

        public static string KeyboardArrowReleased(KeyboardState keyboardstate, KeyboardState lastkeyboardstate)
        {
            if (!keyboardstate.IsKeyDown(Keys.Right) && lastkeyboardstate.IsKeyDown(Keys.Right))
                return "Right";
            if (!keyboardstate.IsKeyDown(Keys.Left) && lastkeyboardstate.IsKeyDown(Keys.Left))
                return "Left";
            if (!keyboardstate.IsKeyDown(Keys.Down) && lastkeyboardstate.IsKeyDown(Keys.Down))
                return "Down";
            if (!keyboardstate.IsKeyDown(Keys.Up) && lastkeyboardstate.IsKeyDown(Keys.Up))
                return "Up";
            return null;
        }

        public static string KeyboardArrowDown(KeyboardState keyboardstate)
        {
            if (keyboardstate.IsKeyDown(Keys.Right))
                return "Right";
            if (keyboardstate.IsKeyDown(Keys.Left))
                return "Left";
            if (keyboardstate.IsKeyDown(Keys.Down))
                return "Down";
            if (keyboardstate.IsKeyDown(Keys.Up))
                return "Up";
            return null;
        }

        public static List<int> XMLFileToIntList(string xmlPath)
        {
            List<int> list = new List<int>();
            using (XmlReader reader = XmlReader.Create(xmlPath))
            {
                while (reader.Read())
                {
                    // Only detect start elements.
                    if (reader.IsStartElement())
                    {
                        // Get element name and switch on it.
                        switch (reader.Name)
                        {
                            case "note":
                                // Detect this element.
                                reader.Read();
                                list.Add(int.Parse(reader.Value.Trim().ToString()));
                                break;
                            case "delay":
                                list.Add(0);
                                break;
                        }
                    }
                }
            }
            return list;
        }
    }
}
