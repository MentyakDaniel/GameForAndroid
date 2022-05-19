using ditto;
using ditto.mono;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameForAndroid.UI.Screens
{
    public class PauseScreen : CCRectangle
    {
        private Texture2D Title;

        public Button ToMenu;
        public Button Continue;

        private int timeCounter;
        private bool counterIncrease;
        private bool counterDecrease;
        private Vector2 TitlePos;

        public bool ContinuePressed;
        public bool MenuPressed;

        public PauseScreen(double width, double height, CCColor background, Texture2D title)
            : base(width, height, background)
        {
            Title = title;

            ToMenu = new Button("Back to Menu", "RobotoBlack", 60, CCTextAlignment.Center, new CCSize(350, 100), CCColors.CornflowerBlue, 20);
            Continue = new Button("Continue", "RobotoBlack", 60, CCTextAlignment.Center, new CCSize(350, 100), CCColors.CornflowerBlue, 20);

            Height = 10 + (Title.Height * 2) + 50 + ToMenu.Height + 50 + Continue.Height + 50;

            Width = Title.Width * 2 + 20;
        }
        public override void Draw(GraphicsDevice device)
        {
            base.Draw(device);

            SpriteBatch.Begin(SpriteSortMode.Immediate);

            if(Title != null)
                SpriteBatch.Draw(Title, TitlePos, null, Color.White, 0, Vector2.Zero, new Vector2(2, 2), SpriteEffects.None, 1);

            SpriteBatch.End();

            ToMenu.Draw(device);
            Continue.Draw(device);
        }

        public void Update(GameTime gameTime, TouchCollection touches)
        {
            Height = 10 + (Title.Height * 2) + ToMenu.Height + Continue.Height + 50;
            Width = Title.Width * 2 + 20;

            TitlePos = new Vector2(RenderRectangle.Center.X - (Title.Width), RenderRectangle.Y + 10);

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

            ToMenu.Position = new CCPoint(RenderRectangle.Center.X - (ToMenu.Width / 2), TitlePos.Y + (Title.Height*2) + 50);
            Continue.Position = new CCPoint(RenderRectangle.Center.X - (Continue.Width / 2), ToMenu.RenderRectangle.Bottom + 50);

            ToMenu.Update(gameTime, touches);
            Continue.Update(gameTime, touches);

            if (ToMenu.IsPressed)
                MenuPressed = true;

            if (Continue.IsPressed)
                ContinuePressed = true;

            base.Update(gameTime);
        }
    }
}