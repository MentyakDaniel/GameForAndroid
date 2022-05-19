using ditto;
using ditto.mono;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameForAndroid.UI.Screens
{
    public class MenuScreen : CCRectangle
    {
        public Texture2D Texture;
        private Texture2D Title;
        public Button NewGame;
        public Button EndlessGame;
        public Button Continue;
        public Button Shop;
        public Button Inventory;
        public Button Exit;

        private int timeCounter;
        private bool counterIncrease;
        private bool counterDecrease;
        private Color Color;
        private Vector2 TitlePos;

        public MenuScreen(double width, double height, Texture2D texture)
            : base(width, height, CCColors.Transparent)
        {
            Texture = texture;

            NewGame = new Button("New Game", "RobotoBlack", 40, CCTextAlignment.Center, new CCSize(300, 80), CCColors.CornflowerBlue, 20) { Foreground = CCColors.White };
            Continue = new Button("Continue", "RobotoBlack", 40, CCTextAlignment.Center, new CCSize(300, 80), CCColors.CornflowerBlue, 20) { Foreground = CCColors.White, Visible = Levels.Level.CurrentLevel != Levels.LevelsName.Prologie };
            Shop = new Button("Shop", "RobotoBlack", 40, CCTextAlignment.Center, new CCSize(300, 80), CCColors.CornflowerBlue, 20) { Foreground = CCColors.White };
            Inventory = new Button("Inventory", "RobotoBlack", 40, CCTextAlignment.Center, new CCSize(300, 80), CCColors.CornflowerBlue, 20) { Foreground = CCColors.White };
            EndlessGame = new Button("Endless game", "RobotoBlack", 40, CCTextAlignment.Center, new CCSize(300, 80), CCColors.CornflowerBlue, 20) { Foreground = CCColors.White };
            Exit = new Button("Exit Game", "RobotoBlack", 40, CCTextAlignment.Center, new CCSize(300, 80), CCColors.CornflowerBlue, 20) { Foreground = CCColors.White };

            ScaleX = width / Texture.Width;
            ScaleY = height / Texture.Height;

            if (ScaleX == 0)
                ScaleX = 1;

            if (ScaleY == 0)
                ScaleY = 1;
        }
        public void SetTitle(Texture2D title)
        {
            Title = title;
            TitlePos = new Vector2(RenderRectangle.Center.X - (Title.Width / 2), RenderRectangle.Center.Y + Title.Height + 100);
        }
        public override void Draw(GraphicsDevice device)
        {
            base.Draw(device);

            SpriteBatch.Begin(SpriteSortMode.Immediate);

            SpriteBatch.Draw(Texture, CCMonoHelper.ConvertToVector2(Position), null, Color, 0, Vector2.Zero, new Vector2((float)ScaleX, (float)ScaleY), SpriteEffects.None, 1);

            if (Title != null)
            {
                if (TitlePos.Y > 100)
                    TitlePos.Y -= 10;
                else if (TitlePos.Y < 100)
                    TitlePos.Y = 100;

                SpriteBatch.Draw(Title, TitlePos, Color.White);
            }

            if(TitlePos.Y == 100)
            {
                NewGame.Draw(device);
                Continue.Draw(device);
                EndlessGame.Draw(device);
                Shop.Draw(device);
                Inventory.Draw(device);
                Exit.Draw(device);
            }

            SpriteBatch.End();
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

            Continue.Visible = (Levels.Level.CurrentLevel != Levels.LevelsName.Prologie && Levels.Level.CurrentLevel != Levels.LevelsName.EndlessLevel) || Levels.Level.StartPlay;

            if (Title != null)
            {
                Continue.Position = new CCPoint((TitlePos.X + (Title.Width / 2)) - (Continue.Width / 2), (TitlePos.Y + Title.Height) + 50);
                NewGame.Position = new CCPoint(Continue.RenderRectangle.X, Continue.RenderRectangle.Bottom + 20);
                EndlessGame.Position = new CCPoint(NewGame.RenderRectangle.X, NewGame.RenderRectangle.Bottom + 20);
                Shop.Position = new CCPoint(EndlessGame.RenderRectangle.X, EndlessGame.RenderRectangle.Bottom + 20);
                Inventory.Position = new CCPoint(Shop.RenderRectangle.X, Shop.RenderRectangle.Bottom + 20);
                Exit.Position = new CCPoint(Inventory.RenderRectangle.X, Inventory.RenderRectangle.Bottom + 20);
            }

            NewGame.Update(gameTime, touches);
            Continue.Update(gameTime, touches);
            EndlessGame.Update(gameTime, touches);
            Shop.Update(gameTime, touches);
            Inventory.Update(gameTime, touches);
            Exit.Update(gameTime, touches);

            base.Update(gameTime);
        }
    }
}