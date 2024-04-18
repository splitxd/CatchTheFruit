using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CatchTheFruit
{
    public class Game1 : Game
    {
        //Тарелка
        Texture2D plateTexture;
        Vector2 platePosition;
        float defaultSpeed = 300f;
        float plateSpeed;
        const int plateSize = 100;
        

        //Фрукты
        Texture2D ballTexture;
        const float fruitSpeed = 200f;
        const int fruitSize = 64;
        private List<Tuple<Texture2D, Vector2>> objPositions = new List<Tuple<Texture2D, Vector2>> ();

        //Бомба
        Texture2D bombTexture;

        //Очки
        int score;

        //Переменные
        static Random rnd = new Random();
        const int minDistance = -150;
        const int maxDistance = -100;


        //Отображение и логические переменные
        private SpriteFont font;
        private GraphicsDeviceManager _graphics;
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
            ballTexture = Content.Load<Texture2D>("ball");
            plateTexture = Content.Load<Texture2D>("plate");
            bombTexture = Content.Load<Texture2D>("bomb");
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
                if (rnd.Next(0,10) < 2)
                    objPositions.Add(new (bombTexture,RandomObjSpawn(_graphics)));
                else
                    objPositions.Add(new(ballTexture, RandomObjSpawn(_graphics)));
                elapsedTimeSinceLastFruit = TimeSpan.Zero;
            }

            // Перемещение и удаление объектов
            for (int i = objPositions.Count - 1; i >= 0; i--)
            {
                objPositions[i] = new(objPositions[i].Item1,new Vector2(objPositions[i].Item2.X, objPositions[i].Item2.Y + fruitSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds));

                if (objPositions[i].Item2.Y >= _graphics.PreferredBackBufferHeight - fruitSize )
                {
                    
                    if (objPositions[i].Item1 != bombTexture)
                        score--;
                    objPositions.RemoveAt(i);
                }

                else if (Vector2.Distance(objPositions[i].Item2, platePosition) <= 40)
                {
                    if (objPositions[i].Item1 == bombTexture)
                        score--;
                    else
                        score++;
                    objPositions.RemoveAt(i);
                }
            }

            base.Update(gameTime);
        }


        //Спавн объектов
        public static Vector2 RandomObjSpawn(GraphicsDeviceManager _graphics)
        {
            
            return new Vector2(_graphics.PreferredBackBufferWidth / 2 - fruitSize/2 + rnd.Next(-300,300), - (int) fruitSize);
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

            _spriteBatch.Draw(plateTexture, ChangeSizeRect(platePosition,plateSize), Color.LightGray);

            // Отрисовка объектов
            foreach (var fruitPosition in objPositions)
            {
                _spriteBatch.Draw(fruitPosition.Item1,ChangeSizeRect(fruitPosition.Item2,fruitSize), Color.White);
            }

            _spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(10, 10), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
