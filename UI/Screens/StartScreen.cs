using ditto;
using ditto.mono;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameForAndroid.UI.Screens
{
    public class StartScreen : CCRectangle
    {
        public Texture2D Texture;
        public Texture2D Title;
        public TextButton StartButton;

        private int timeCounter;
        private bool counterIncrease;
        private bool counterDecrease;
        private Color Color;

        public StartScreen(double width, double height, Texture2D texture) 
            : base(width, height, CCColors.Transparent)
        {
            Texture = texture;

            StartButton = new TextButton("Press to start", "RobotoBlack", 100)
            {
                Color = CCColors.White
            };

            ScaleX = width / Texture.Width;
            ScaleY = height / Texture.Height;

            if (ScaleX == 0)
                ScaleX = 1;

            if (ScaleY == 0)
                ScaleY = 1;
        }

        public override void Draw(GraphicsDevice device)
        {
            base.Draw(device);

            SpriteBatch.Begin(SpriteSortMode.Immediate);

            SpriteBatch.Draw(Texture, CCMonoHelper.ConvertToVector2(Position), null, Color, 0, Vector2.Zero, new Vector2((float)ScaleX, (float)ScaleY), SpriteEffects.None, 1);

            if(Title != null)
                SpriteBatch.Draw(Title, new Vector2(RenderRectangle.Center.X - (Title.Width / 2), RenderRectangle.Center.Y + Title.Height + 100), Color.White);

            SpriteBatch.End();

            StartButton.Draw(device);
        }

        public void Update(GameTime gameTime, TouchCollection touches)
        {
            RenderRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(Texture.Width * ScaleX), (int)(Texture.Height * ScaleY));

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


            StartButton.Position = new CCPoint(RenderRectangle.Center.X - (StartButton.RenderRectangle.Width / 2), RenderRectangle.Center.Y - (StartButton.RenderRectangle.Height / 2));
            StartButton.Update(touches);

            base.Update(gameTime);
        }
    }
}