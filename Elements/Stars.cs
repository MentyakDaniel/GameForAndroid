using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;


namespace GameForAndroid.Elements
{
    public static class Stars
    {
        public static int Width, Height;
        public static Random rand = new Random();
        public static int CountStars = 50;
        public static bool isInit;
        static Star[] stars;

        public static int GetRand(int min, int max) => rand.Next(min, max);

        public static void Init(Texture2D starTexture)
        {
            stars = new Star[CountStars];

            for (int i = 0; i < stars.Length; i++)
                stars[i] = new Star(new Vector2(-GetRand(2, 15), 0), starTexture);

            isInit = true;
        }
        public static void Update()
        {
            if (!isInit)
                return;

            foreach (var i in stars)
                i.Update();
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            if (!isInit)
                return;

                foreach (var i in stars)
                i.Draw(spriteBatch);
        }

    }
    public class Star : Element
    {
        Vector2 Direction;

        public Star(Vector2 pos, Vector2 dir)
        {
            Position = pos;
            Direction = dir;
        }
        public Star(Vector2 dir, Texture2D star)
        {
            Direction = dir;
            Texture = star;

            RandomSet();
        }

        public override void Update(TouchCollection touches, GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color, 0, Vector2.Zero, new Vector2((float)Stars.rand.NextDouble()), SpriteEffects.None, 1);
        }
        private void RandomSet()
        {
            Position = new Vector2(Stars.GetRand(Stars.Width + 50, Stars.Width + 300), Stars.GetRand(0, Stars.Height));
            Color = Color.FromNonPremultiplied(Stars.GetRand(0, 256), Stars.GetRand(0, 256), Stars.GetRand(0, 256), Stars.GetRand(127, 256));
        }

        public override void Update()
        {
            Position += Direction;

            if (Position.X < 0)
                RandomSet();
        }
    }
}