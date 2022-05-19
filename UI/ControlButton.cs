using GameForAndroid.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameForAndroid.UI
{
    public class ControlButton : Element
    {
        public bool IsPressed;
        private int Alpha = 127;
        private TouchCollection _currentTouch;

        public ControlButton(Texture2D texture, Vector2 position, float width, float height)
        {
            Texture = texture;
            Position = position;
            Width = width;
            Height = height;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float scaleX = Width / Texture.Width;
            float scaleY = Height / Texture.Height;

            if (scaleX == 0)
                scaleX = 1;
            if (scaleY == 0)
                scaleY = 1;

            spriteBatch.Draw(Texture, Position, null, Color.FromNonPremultiplied(255, 255, 255, Alpha), 0, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 1);
        }
        public override void Update(TouchCollection touches, GameTime gameTime)
        {
            _currentTouch = touches;

            Rectangle = new Rectangle(Position.ToPoint(), new Point((int)Width, (int)Height));

            foreach (var i in _currentTouch)
            {
                if ((i.State == TouchLocationState.Pressed || i.State == TouchLocationState.Moved) && Rectangle.Contains(i.Position))
                {
                    IsPressed = true;
                    Alpha = 255;
                }
                else if (i.State == TouchLocationState.Released)
                {
                    IsPressed = false;
                    Alpha = 127;
                }
                else
                {
                    IsPressed = false;
                    Alpha = 127;
                }
            }
        }
        public override void Update()
        {
        }
    }
}