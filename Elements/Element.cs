using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;


namespace GameForAndroid.Elements
{
    public abstract class Element
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Color Color;
        public Rectangle Rectangle;
        public float Width;
        public float Height;

        public abstract void Update(TouchCollection touches, GameTime gameTime);
        public abstract void Update();
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}