using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;


namespace GameForAndroid.Elements.Shots
{
    public class DupletShot : Shot
    {
        private OneShot _firstShot;
        private OneShot _secondShot;

        public DupletShot(Texture2D shotSprite, Rectangle playerRect, int damage)
        {
            Texture = shotSprite;
            Damage = damage;

            Position = new Vector2(playerRect.Right, playerRect.Center.Y - Texture.Height / 2);
            PlayerRect = playerRect;

            _firstShot = new OneShot(shotSprite, playerRect, Damage)
            {
                Position = new Vector2(playerRect.Right, playerRect.Center.Y - shotSprite.Height - 5)
            };

            _secondShot = new OneShot(shotSprite, playerRect, Damage)
            {
                Position = new Vector2(playerRect.Right, playerRect.Center.Y + shotSprite.Height + 5)
            };

            Direction = new Vector2(XSpeed, 0);
            Color = Color.White;

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!_firstShot.Hidden)
                spriteBatch.Draw(Texture, _firstShot.Position, Color);
            if (!_secondShot.Hidden)
                spriteBatch.Draw(Texture, _secondShot.Position, Color);
        }

        public override void Update(TouchCollection touches, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            Direction = new Vector2(XSpeed, YSpeed);

            if (!Hidden)
                Position += Direction;

            if (!_firstShot.Hidden)
                _firstShot.Update();
            if (!_secondShot.Hidden)
                _secondShot.Update();

            Hidden = _firstShot.Hidden && _secondShot.Hidden;
        }
    }
}