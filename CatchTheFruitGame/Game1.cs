using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CatchTheFruitGame
{
    public class Object
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }

        public Object(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }
    }
    public class Game1 : Game
    {
        //Тарелка
        Texture2D plateTexture;
        Vector2 platePosition;
        float defaultSpeed = 300f;
        float plateSpeed;
        const int plateSize = 100;


        //Фрукты
        Texture2D melonTexture;
        Texture2D appleTexture;
        Texture2D peachTexture;
        Texture2D orangeTexture;
        static List<Texture2D> fruitTextures = new List<Texture2D>();
        const float fruitSpeed = 200f;
        const int fruitSize = 64;
        private static List<Object> objPositions = new List<Object>();

        //Бомба
        static Texture2D bombTexture;

        //Очки
        static int score;

        //Переменные
        static Random rnd = new Random();
        const int minDistance = -150;
        const int maxDistance = -100;


        //Отображение и логические переменные
        Texture2D background;
        private SpriteFont font;
        private static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Таймер
        private TimeSpan elapsedTimeSinceLastFruit;
        int fruitSpawnTime = 1;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.ApplyChanges();
            score = 0;
            plateSpeed = defaultSpeed;

            platePosition = new Vector2((_graphics.PreferredBackBufferWidth - plateSize) / 2, _graphics.PreferredBackBufferHeight - plateSize);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("background");
            melonTexture = Content.Load<Texture2D>("melon");
            peachTexture = Content.Load<Texture2D>("peach");
            appleTexture = Content.Load<Texture2D>("apple");
            orangeTexture = Content.Load<Texture2D>("orange");
            fruitTextures.Add(melonTexture);
            fruitTextures.Add(peachTexture);
            fruitTextures.Add(appleTexture);
            fruitTextures.Add(orangeTexture);
            bombTexture = Content.Load<Texture2D>("bomb");
            plateTexture = Content.Load<Texture2D>("plate");
            font = Content.Load<SpriteFont>("File");

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var kstate = Keyboard.GetState();

            //Передвижение тарелки
            if (kstate.IsKeyDown(Keys.A) || kstate.IsKeyDown(Keys.Left))
            {
                if (kstate.IsKeyDown(Keys.LeftShift))
                    plateSpeed = defaultSpeed * 1.5f;
                else
                    plateSpeed = defaultSpeed;
                platePosition.X -= plateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kstate.IsKeyDown(Keys.D) || kstate.IsKeyDown(Keys.Right))
            {
                if (kstate.IsKeyDown(Keys.LeftShift))
                    plateSpeed = defaultSpeed * 1.5f;
                else
                    plateSpeed = defaultSpeed;
                platePosition.X += plateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (platePosition.X <= 0)
                platePosition.X = 0;
            else if (platePosition.X >= _graphics.PreferredBackBufferWidth - plateSize)
                platePosition.X = _graphics.PreferredBackBufferWidth - plateSize;

            //Спавн объектов по таймеру
            elapsedTimeSinceLastFruit += gameTime.ElapsedGameTime;

            if (elapsedTimeSinceLastFruit >= TimeSpan.FromSeconds(fruitSpawnTime))
            {
                //Рандомный спавн бомб
                if (rnd.Next(0, 10) < 2)
                    objectsAdd(objPositions, bombTexture);
                else
                    objectsAdd(objPositions, randomFruitTexture());
                elapsedTimeSinceLastFruit = TimeSpan.Zero;
            }

            // Перемещение и удаление объектов
            for (int i = objPositions.Count - 1; i >= 0; i--)
            {
                objPositions[i] = new(objPositions[i].Texture, new Vector2(objPositions[i].Position.X, objPositions[i].Position.Y + fruitSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds));

                Colluzion(platePosition, objPositions[i], i);
            }

            base.Update(gameTime);
        }

        //Столкновение с объектами
        public static void Colluzion(Vector2 plate, Object obj, int i)
        {
            Rectangle plateRect = new Rectangle((int)plate.X, (int)plate.Y, plateSize, plateSize);
            Rectangle fruitRect = new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)(fruitSize * 0.7), (int)(fruitSize * 0.7));
            if (obj.Position.Y >= _graphics.PreferredBackBufferHeight + fruitSize)
            {

                if (obj.Texture != bombTexture)
                    score--;
                objPositions.RemoveAt(i);
            }
            else if (fruitRect.Intersects(plateRect))
            {
                if (obj.Texture == bombTexture)
                    score--;
                else
                    score++;
                objPositions.RemoveAt(i);
            }
        }

        //Добавление объектов в список
        public static List<Object> objectsAdd(List<Object> objects, Texture2D texture)
        {
            Object obj = new Object(texture, RandomObjSpawn());
            objects.Add(obj);
            return objects;
        }

        //Случайная текстурка фрукта
        public static Texture2D randomFruitTexture()
        {
            int randomIndex = rnd.Next(fruitTextures.Count);
            return fruitTextures[randomIndex];
        }

        //Спавн объектов
        public static Vector2 RandomObjSpawn()
        {

            return new Vector2(_graphics.PreferredBackBufferWidth / 2 - fruitSize / 2 + rnd.Next(-300, 300), -(int)fruitSize);
        }
        //Изменение размеров объектов
        public static Rectangle ChangeSizeRect(Vector2 position, int size)
        {
            return new Rectangle((int)position.X, (int)position.Y, size, size);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            _spriteBatch.Draw(plateTexture, ChangeSizeRect(platePosition, plateSize), Color.LightGray);

            // Отрисовка объектов
            foreach (var fruitPosition in objPositions)
            {
                _spriteBatch.Draw(fruitPosition.Texture, ChangeSizeRect(fruitPosition.Position, fruitSize), Color.White);
            }

            _spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(10, 20), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
