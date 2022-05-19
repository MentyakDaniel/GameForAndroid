using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;


namespace GameForAndroid.Elements.Shots
{
    public class TripleShoot : Shot
    {
        private OneShot _top;
        private OneShot _center;
        private OneShot _bottom;

        public TripleShoot(Texture2D shootSprite, Rectangle playerRect, int damage)
        {
            Texture = shootSprite;
            Damage = damage;

            Position = new Vector2(playerRect.Right, playerRect.Center.Y - Texture.Height / 2);
            PlayerRect = playerRect;

            _top = new OneShot(Texture, playerRect, Damage)
            {
                Position = new Vector2(Position.X - PlayerRect.Width / 1.5f, Position.Y - PlayerRect.Height / 2 + Texture.Height / 2)
            };

            _center = new OneShot(Texture, playerRect, Damage)
            {
                Position = new Vector2(playerRect.Right, playerRect.Center.Y - Texture.Height / 2)
            };

            _bottom = new OneShot(Texture, playerRect, Damage)
            {
                Position = new Vector2(Position.X - PlayerRect.Width / 1.5f, Position.Y + PlayerRect.Height / 2 - Texture.Height / 2)
            };

            Direction = new Vector2(XSpeed, 0);
            Color = Color.White;

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!_top.Hidden)
                spriteBatch.Draw(Texture, _top.Position, Color);
            if (!_center.Hidden)
                spriteBatch.Draw(Texture, _center.Position, Color);
            if (!_bottom.Hidden)
                spriteBatch.Draw(Texture, _bottom.Position, Color);
        }

        public override void Update(TouchCollection touches, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            Direction = new Vector2(XSpeed, 0);

            if (!Hidden)
                Position += Direction;

            if (!_top.Hidden)
                _top.Update();
            if (!_center.Hidden)
                _center.Update();
            if (!_bottom.Hidden)
                _bottom.Update();

            Hidden = _top.Hidden && _center.Hidden && _bottom.Hidden;
        }
    }
}