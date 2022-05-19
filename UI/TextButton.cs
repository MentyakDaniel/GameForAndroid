using ditto;
using ditto.mono;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace GameForAndroid.UI
{
    public class TextButton : CCLabel
    {
        public bool IsPressed;
        int timeCounter;
        bool counterDecrease;
        bool counterIncrease = true;
        float Scale;
        public static Random rand = new Random();

        public TextButton(string text, string fontFamily, int fontSize) 
            : base(text, fontFamily, fontSize, CCTextAlignment.Center)
        {
            Text = text;
            FontSize = fontSize;
        }

        public void Update(TouchCollection touches)
        {
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

            if (Font != null)
            {
                double width = 0;
                double height = 0;

                foreach (char letter in Text)
                {
                    foreach (var glyphs in Font.Glyphs)
                    {
                        if (letter == glyphs.Character)
                        {
                            width += glyphs.Cropping.Width;

                            if (height < glyphs.Cropping.Height)
                                height = glyphs.Cropping.Height;
                        }
                    }
                }

                float ScaleX = FontSize / (float)height;
                float ScaleY = ScaleX;
                Scale = ScaleX;

                RenderRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(width * ScaleX), (int)(height * ScaleY));
            }

            Color = new CCColor(timeCounter, rand.Next(0,255+1), rand.Next(0, 255 + 1), rand.Next(0, 255 + 1));

            if (counterIncrease)
                timeCounter++;
            else if (counterDecrease)
                timeCounter--;

            foreach (var i in touches)
            {
                if (RenderRectangle.Contains(i.Position))
                    IsPressed = true;
                else
                    IsPressed = false;
            }
            if (touches.Count == 0)
                IsPressed = false;
        }
    }
}