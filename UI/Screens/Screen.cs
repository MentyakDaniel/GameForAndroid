using GameForAndroid.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;


namespace GameForAndroid.UI.Screens
{
    public class Screen : Element
    {
        int timeCounter;
        float scaleX;
        float scaleY;
        public bool StartNew = true;
        private bool counterIncrease;
        private bool counterDecrease;

        public Screen(Texture2D texture)
        {
            Texture = texture;
            Color = Color.White;
            Color.A = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color, 0, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 1);
        }
        public override void Update(TouchCollection touches, GameTime gameTime)
        {

        }

        public override void Update()
        {
            scaleX = Width / Texture.Width;
            scaleY = Height / Texture.Height;

            if (scaleX == 0)
                scaleX = 1;

            if (scaleY == 0)
                scaleY = 1;

            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(Texture.Width * scaleX), (int)(Texture.Height * scaleY));

            Color = Color.FromNonPremultiplied(255, 255, 255, timeCounter);

            if (timeCounter == 255)
            {
                counterIncrease = false;
                counterDecrease = true;
            }
            if (timeCounter == 0)
            {
                counterIncrease = true;
                counterDecrease = false;
            }

            if (counterIncrease)
                timeCounter++;
            else if (counterDecrease)
                timeCounter--;
        }
    }
}