﻿using ditto;
using ditto.mono;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameForAndroid.UI.Screens
{
    public class VictoryScreen : CCRectangle
    {
        public Texture2D Texture;
        public TextButton RestartButton;
        public TextButton NextButton;
        public TextButton ToMenu;

        public CCLabel Coints;
        private int timeCounter;
        private bool counterIncrease;
        private bool counterDecrease;
        private Color Color;

        public VictoryScreen(double width, double height, Texture2D texture)
            : base(width, height, CCColors.Transparent)
        {
            Texture = texture;

            RestartButton = new TextButton("Restart Level", "RobotoBlack", 100)
            {
                Color = CCColors.White
            };
            NextButton = new TextButton("Next Level", "RobotoBlack", 100)
            {
                Color = CCColors.White
            };
            ToMenu = new TextButton("Back to Menu", "RobotoBlack", 100)
            {
                Color = CCColors.White
            };

            Coints = new CCLabel("text", "RobotoBlack", 60, CCTextAlignment.Center) { Color = CCColors.White };

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

            SpriteBatch.End();

            RestartButton.Draw(device);
            NextButton.Draw(device);
            ToMenu.Draw(device);
            Coints.Draw(device);
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


            RestartButton.Position = new CCPoint(RenderRectangle.Center.X - (RestartButton.RenderRectangle.Width / 2), RenderRectangle.Bottom - (RestartButton.RenderRectangle.Height / 2) + 100);
            RestartButton.Update(touches);

            NextButton.Position = new CCPoint(RestartButton.RenderRectangle.Right + 100, RestartButton.RenderRectangle.Y);
            NextButton.Update(touches);

            ToMenu.Position = new CCPoint(RestartButton.RenderRectangle.X - 100 - ToMenu.Width, RestartButton.RenderRectangle.Y);
            ToMenu.Update(touches);

            Coints.Position = new CCPoint(RenderRectangle.Center.X - (Coints.RenderRectangle.Width / 2), RenderRectangle.Y - (Coints.RenderRectangle.Height / 2) - 100);
            Coints.Update(gameTime);

            base.Update(gameTime);
        }
    }
}