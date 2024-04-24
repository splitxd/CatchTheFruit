using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;


namespace CatchTheFruitGame
{

    public class Game1 : Game
    {
        //Звуки
        static Song crunchSound_1;
        static Song crunchSound_2;
        static Song crunchSound_3;
        static List<Song> crunchSounds;
        static Song tenPoints;
        static Song hundretPoints;
        static Song thousenPoints;

        static Song bombSound;

        //Текстуры
        Texture2D plateTexture;
        Texture2D melonTexture;
        Texture2D appleTexture;
        Texture2D peachTexture;
        Texture2D orangeTexture;
        static Texture2D bombTexture;
        Texture2D background;
        Texture2D whiteTexture;

        //Тарелка
        static Vector2 platePosition;
        static float defaultSpeed = 300f;
        static float plateSpeed;
        const int plateSize = 100;


        //Фрукты
        static List<Texture2D> fruitTextures = new List<Texture2D>();
        const float fruitSpeed = 200f;
        const int fruitSize = 64;
        private static List<Object> objPositions = new List<Object>();

        //Очки
        static int score;
        static int backRectWidth = 152;
        static int backRectHeight = 46;

        //Переменные
        static Random rnd = new Random();
        const int minDistance = -150;
        const int maxDistance = -100;


        //Отображение и логические переменные
        
        private SpriteFont font;
        private static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Таймер
        private TimeSpan elapsedTimeSinceLastFruit;
        int fruitSpawnTime = 1;

        //для звуков 
        static int previous = 0;
        static bool isFirstTen = true;
        static bool isFirtstHundred = true;
        static bool isFirstTousand = true;
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
            crunchSounds = new List<Song>();
            whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Color.White });
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
            bombSound = Content.Load<Song>("bombSound");
            crunchSound_1 = Content.Load<Song>("crunchSound_1");
            crunchSound_2 = Content.Load<Song>("crunchSound_2");
            crunchSound_3 = Content.Load<Song>("crunchSound_3");
            crunchSounds.Add(crunchSound_1);
            crunchSounds.Add(crunchSound_2);
            crunchSounds.Add(crunchSound_3);
            tenPoints = Content.Load<Song>("10points");
            hundretPoints = Content.Load<Song>("100points");
            thousenPoints = Content.Load<Song>("1000points");
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Movement(gameTime);


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
            ScoreSounds();

            base.Update(gameTime);
        }

        public static void Movement(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
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
        }

        public static void ScoreSounds()
        {
            if (score % 1000 == 0 && score != 0)
            {
                if (previous < score)
                {
                    MediaPlayer.Play(thousenPoints);
                    previous = score;
                }
                if (isFirstTousand)
                {
                    var temp = backRectWidth + 20;
                    backRectWidth = temp;
                    isFirstTousand = false;
                }
            }
            else if (score % 100 == 0 && score != 0)
            {
                if (previous < score)
                {
                    MediaPlayer.Play(hundretPoints);
                    previous = score;
                }
                if (isFirtstHundred)
                {
                    var temp = backRectWidth + 20;
                    backRectWidth = temp;
                    isFirtstHundred = false;
                }
            }
            else if (score % 10 == 0 && score != 0)
            {
                if (previous < score)
                {
                    MediaPlayer.Play(tenPoints);
                    previous = score;
                }
                if (isFirstTen)
                {
                    var temp = backRectWidth + 20;
                    backRectWidth = temp;
                    isFirstTen = false;
                }
            }
        }

        //Столкновение с объектами
        public static void Colluzion(Vector2 plate, Object obj, int i)
        {
            Rectangle plateRect = new Rectangle((int)plate.X, (int)plate.Y, plateSize, plateSize);
            Rectangle fruitRect = new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)(fruitSize * 0.7), (int)(fruitSize * 0.7));
            if (obj.Position.Y >= _graphics.PreferredBackBufferHeight + fruitSize)
            {

                if (obj.Texture != bombTexture)
                {
                    score--;
                }
                objPositions.RemoveAt(i);
            }
            else if (fruitRect.Intersects(plateRect) && (plate.Y + (plateSize / 2) > obj.Position.Y ))
            {
                if (obj.Texture == bombTexture)
                {
                    MediaPlayer.Play(bombSound);
                    score--;
                }
                else
                {
                    MediaPlayer.Play(crunchSounds[rnd.Next(0, crunchSounds.Count)]);
                    score++;
                }
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
        public static Rectangle ChangeSizeRect(Vector2 position, int width, int height)
        {
            return new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, ChangeSizeRect(Vector2.Zero, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            _spriteBatch.Draw(plateTexture, ChangeSizeRect(platePosition, plateSize,plateSize), Color.LightGray);

            // Отрисовка объектов
            foreach (var fruitPosition in objPositions)
            {
                _spriteBatch.Draw(fruitPosition.Texture, ChangeSizeRect(fruitPosition.Position, fruitSize,fruitSize), Color.White);
            }

            _spriteBatch.Draw(whiteTexture, ChangeSizeRect(new Vector2(8, 18), backRectWidth, backRectHeight), new Color(34, 139, 34));
            _spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(10, 20), new Color(173, 255, 47));

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
