using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace GameForAndroid.Elements.Boosts
{
    public enum BoostType
    {
        DamageIncrease,
        DamageDecrease,
        DefaultFire,
        DoubleFire,
        FireSpeedIncrease,
        FireSpeedDecrease,
        PlayerSpeedIncrease,
        PlayerSpeedDecrease,
        PlayerHeal
    }
    public class Boosts : Element
    {
        public BoostType BoostType;
        public Vector2 Direction;
        public bool Destroy;
        public bool PlayerTakeIt;
        public Boosts(Texture2D boostTexture, BoostType boostType)
        {
            BoostType = boostType;
            Texture = boostTexture;
            Direction = new Vector2(-2, 0);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color);
        }

        public override void Update(TouchCollection touches, GameTime gameTime)
        {

        }

        public override void Update()
        {
            Position += Direction;

            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            if (Rectangle.Right <= 0 || PlayerTakeIt)
            {
                Destroy = true;
                return;
            }
        }
    }
}