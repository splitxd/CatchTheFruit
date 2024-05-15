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
        static SoundEffect crunchSound_1;
        static SoundEffect crunchSound_2;
        static SoundEffect crunchSound_3;
        static List<SoundEffect> crunchSounds = new();
        static SoundEffect tenPoints;
        static SoundEffect hundretPoints;
        static SoundEffect thousenPoints;
        static SoundEffect bombSound;

        //Текстуры
        Texture2D plateTexture;
        Texture2D melonTexture;
        Texture2D appleTexture;
        Texture2D peachTexture;
        Texture2D orangeTexture;
        static Texture2D bombTexture;
        Texture2D background;
        Texture2D whiteTexture;

        //Подключение классов
        private static Plate _plate;
        private static InteractionObjects _interactionObjects;
        private static ScorePoints _scorePoints;
        private static Difficulty _difficulty;

        //Фрукты
        static List<Texture2D> fruitTextures = new();

        //Очки
        public int score;


        //Отображение и логические переменные
        
        private SpriteFont font;
        private static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //для звуков 

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

            _interactionObjects = new InteractionObjects();
            _plate = new Plate(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _difficulty = new Difficulty();

            _interactionObjects.Initialize(_plate, _graphics);
            _scorePoints = new ScorePoints();

            crunchSounds = new List<SoundEffect>();
            whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Color.White });
            score = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("Img/background");
            melonTexture = Content.Load<Texture2D>("Img/melon");
            peachTexture = Content.Load<Texture2D>("Img/peach");
            appleTexture = Content.Load<Texture2D>("Img/apple");
            orangeTexture = Content.Load<Texture2D>("Img/orange");
            fruitTextures.Add(melonTexture);
            fruitTextures.Add(peachTexture);
            fruitTextures.Add(appleTexture);
            fruitTextures.Add(orangeTexture);
            bombTexture = Content.Load<Texture2D>("Img/bomb");
            plateTexture = Content.Load<Texture2D>("Img/plate");
            font = Content.Load<SpriteFont>("Font/File");
            bombSound = Content.Load<SoundEffect>("Sounds/bombSound");
            crunchSound_1 = Content.Load<SoundEffect>("Sounds/crunchSound_1");
            crunchSound_2 = Content.Load<SoundEffect>("Sounds/crunchSound_2");
            crunchSound_3 = Content.Load<SoundEffect>("Sounds/crunchSound_3");
            crunchSounds.Add(crunchSound_1);
            crunchSounds.Add(crunchSound_2);
            crunchSounds.Add(crunchSound_3);
            tenPoints = Content.Load<SoundEffect>("Sounds/10points");
            hundretPoints = Content.Load<SoundEffect>("Sounds/100points");
            thousenPoints = Content.Load<SoundEffect>("Sounds/1000points");

            _interactionObjects.SoundsLoad(bombSound, crunchSounds);
            _interactionObjects.TexturesLoad(bombTexture, fruitTextures);
            _scorePoints.Initialize(thousenPoints, hundretPoints,tenPoints);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _plate.Update(gameTime,this);
            _interactionObjects.Update(gameTime,this);
            _difficulty.Update(gameTime);

            ScorePoints.ScoreSounds(score);

            base.Update(gameTime);
        }

        public static Rectangle ChangeSizeRect(Vector2 position, int width, int height)
        {
            return new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, ChangeSizeRect(Vector2.Zero, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            _spriteBatch.Draw(plateTexture, ChangeSizeRect(_plate.Position, _plate.Size, _plate.Size), Color.LightGray);

            foreach (var fruitPosition in InteractionObjects.objPositions)
            {
                _spriteBatch.Draw(fruitPosition.Texture, ChangeSizeRect(fruitPosition.Position, _interactionObjects.FruitSize,_interactionObjects.FruitSize), Color.White);
            }
            _spriteBatch.Draw(whiteTexture, ChangeSizeRect(new Vector2(8, 18), ScorePoints.backRectWidth, ScorePoints.backRectHeight), new Color(34, 139, 34));
            _spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(10, 20), new Color(173, 255, 47));

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
