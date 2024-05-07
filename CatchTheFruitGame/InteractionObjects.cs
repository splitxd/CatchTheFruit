using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CatchTheFruitGame
{
    internal class InteractionObjects
    {
        private TimeSpan elapsedTimeSinceLastFruit;
        public int fruitSpawnTime = 1;
        public static GraphicsDeviceManager _graphics;

        static List<Texture2D> fruitTextures = new List<Texture2D>();
        const float fruitSpeed = 200f;
        public static int fruitSize = 64;
        public static List<Object> objPositions = new List<Object>();
        private static Plate _plate;

        static Random rnd = new Random();
        private static Texture2D bombTexture;

        private static Song bombSound;
        private static List<Song> crunchSounds;

        public void SoundsLoad(Song BombSound, List<Song> CrunchSounds)
        {
            bombSound = BombSound;
            crunchSounds = CrunchSounds;
        }

        public void TexturesLoad(Texture2D BombTexture, List<Texture2D> FruitTextures) 
        {
            bombTexture = BombTexture;
            fruitTextures = FruitTextures;
        }

        public void Initialize(Plate plate, GraphicsDeviceManager graphics)
        {
            _plate = plate;
            _graphics = graphics;
        }

        public int FruitSize { get { return fruitSize; } }

        public void Update(GameTime gameTime,Game1 game)
        {
            elapsedTimeSinceLastFruit += gameTime.ElapsedGameTime;

            if (elapsedTimeSinceLastFruit >= TimeSpan.FromSeconds(fruitSpawnTime))
            {
                if (rnd.Next(0, 10) < 2)
                    objectsAdd(objPositions, bombTexture);
                else
                    objectsAdd(objPositions, randomFruitTexture());
                elapsedTimeSinceLastFruit = TimeSpan.Zero;
            }

            for (int i = objPositions.Count - 1; i >= 0; i--)
            {
                objPositions[i] = new(objPositions[i].Texture, new Vector2(objPositions[i].Position.X, objPositions[i].Position.Y + fruitSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds));

                Colluzion(_plate.Position, objPositions[i], i,game);
            }
        }

        private static void Colluzion(Vector2 plate, Object obj, int i,Game1 game)
        {
            Rectangle plateRect = new Rectangle((int)plate.X, (int)plate.Y, _plate.Size, _plate.Size);
            Rectangle fruitRect = new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)(fruitSize * 0.7), (int)(fruitSize * 0.7));
            if (obj.Position.Y >= _graphics.PreferredBackBufferHeight + fruitSize)
            {

                if (obj.Texture != bombTexture)
                {
                    game.score--;
                }
                objPositions.RemoveAt(i);
            }
            else if (fruitRect.Intersects(plateRect) && (plate.Y + (_plate.Size / 2) > obj.Position.Y))
            {
                if (obj.Texture == bombTexture)
                {
                    MediaPlayer.Play(bombSound);
                    game.score--;
                }
                else
                {
                    MediaPlayer.Play(crunchSounds[rnd.Next(0, crunchSounds.Count)]);
                    game.score++;
                }
                objPositions.RemoveAt(i);
            }
        }
        public static List<Object> objectsAdd(List<Object> objects, Texture2D texture)
        {
            Object obj = new Object(texture, RandomObjSpawn());
            objects.Add(obj);
            return objects;
        }

        public static Vector2 RandomObjSpawn()
        {
            return new Vector2(_graphics.PreferredBackBufferWidth / 2 - fruitSize / 2 + rnd.Next(-300, 300), -(int)fruitSize);
        }

        public static Texture2D randomFruitTexture()
        {
            int randomIndex = rnd.Next(fruitTextures.Count);
            return fruitTextures[randomIndex];
        }
    }
}
