using ditto;
using ditto.mono;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;

namespace GameForAndroid.Elements.Gameplay
{
    public static class Asteroids
    {
        public static int Width, Height;
        public static Random rand = new Random();
        public static int CountAsteroids = 10;
        public static bool isInit;
        public static List<Asteroid> AsteroidsList = new List<Asteroid>();
        private static Texture2D _asteroidTexture;

        public static int GetRand(int min, int max) => rand.Next(min, max);
        public static void Init(Texture2D asteroidTexture)
        {
            AsteroidsList = new List<Asteroid>();
            _asteroidTexture = asteroidTexture;

            for (int i = 0; i < GetRand(1, CountAsteroids); i++)
                AsteroidsList.Add(new Asteroid(asteroidTexture));

            isInit = true;
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            if (!isInit) return;

            foreach (Asteroid i in AsteroidsList)
                i.Draw(spriteBatch);
        }
        public static void Update()
        {
            if (!isInit) return;

            for (int i = 0; i < AsteroidsList.Count; i++)
            {
                AsteroidsList[i].Update();

                if (AsteroidsList[i].IsShooted)
                {
                    AsteroidsList.RemoveAt(i);

                    Game1.Score++;

                    i--;
                }
                else if (AsteroidsList[i].Destroy)
                {
                    AsteroidsList.RemoveAt(i);
                    i--;
                }

            }

            if(AsteroidsList.Count == 0)
            {
                if (GetRand(0, 1 + 1) == 1)
                    Init(_asteroidTexture);
            }
        }
    }
    public class Asteroid : Element
    {
        public Vector2 Direction;
        public float Scale;
        public float Rotation = 0;
        public float RotationSpeed = 0;
        public int Health = 2;
        public bool Destroy;
        public bool IsShooted = false;

        private Vector2 TextureCenter => new Vector2(Texture.Width / 2, Texture.Height / 2);
        public Asteroid(Texture2D texture,Vector2 position, Vector2 direction, float scale, float rotation, float rotationSpeed)
        {
            Position = position;
            Direction = direction;
            Scale = scale;
            Rotation = rotation;
            RotationSpeed = rotationSpeed;
            Texture = texture;
        }
        public Asteroid(Texture2D asteroidTexture)
        {
            Texture = asteroidTexture;
            RandomSet();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color, Rotation, TextureCenter, Scale, SpriteEffects.None, 1);

#if DEBUG
            CCRectangle rect = new CCRectangle(Rectangle.Width, Rectangle.Height, CCColors.Transparent, 0)
            {
                Position = new CCPoint(Rectangle.X, Rectangle.Y),
                BorderWidth = 1,
                BorderColor = CCColors.Red
            };
            rect.Draw(spriteBatch.GraphicsDevice);
#endif
        }
        private void RandomSet()
        {
            Position = new Vector2(Asteroids.GetRand(Asteroids.Width + (int)(Texture.Width * Scale), Asteroids.Width + (int)(Texture.Width * Scale) + 300), Asteroids.GetRand(Texture.Height, Asteroids.Height - (int)((Texture.Height * Scale) * 2)));

            Direction = new Vector2(-Asteroids.GetRand(3, 6), 0);
            Scale = (float)Asteroids.GetRand(40, 101) / 100;
            RotationSpeed = (float)(Asteroids.rand.NextDouble() - .5) / 2;

            Color = Color.White;
        }
        public override void Update(TouchCollection touches, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            Position += Direction;
            Rotation += RotationSpeed;

            Rectangle = new Rectangle((int)Position.X - (int)(Texture.Width * Scale / 2), (int)Position.Y - (int)(Texture.Width * Scale / 2), (int)(Texture.Width * Scale), (int)(Texture.Height * Scale));

            if (Rectangle.Right <= 0 || Health <= 0)
            {
                Destroy = true;
                return;
            }

            Color = Color.FromNonPremultiplied(255, (int)(Health * 127.5), (int)(Health * 127.5), 255);
        }
    }
}