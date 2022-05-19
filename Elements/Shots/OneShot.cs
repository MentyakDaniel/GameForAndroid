using ditto;
using ditto.mono;
using GameForAndroid.Elements.Gameplay;
using GameForAndroid.Elements.Gameplay.Bosses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;


namespace GameForAndroid.Elements.Shots
{
    public class OneShot : Shot
    {

        public OneShot(Texture2D shootSprite, Rectangle playerRect, int damage, bool enemy = false)
        {
            Texture = shootSprite;
            Damage = damage;

            if(!enemy)
                Position = new Vector2(playerRect.Right, playerRect.Center.Y - Texture.Height / 2);
            else
                Position = new Vector2(playerRect.X, playerRect.Center.Y - Texture.Height / 2);

            IsEnemy = enemy;

            Color = Color.White;
            PlayerRect = playerRect;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color);

#if DEBUG
            CCRectangle rect = new CCRectangle(Rectangle.Width, Rectangle.Height, CCColors.Transparent, 0)
            {
                Position = new CCPoint(Rectangle.X, Rectangle.Y),
                BorderWidth = 1,
                BorderColor = CCColors.Red
            };

            rect.Draw(spriteBatch.GraphicsDevice);
#endif
        }

        public override void Update(TouchCollection touches, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            if (!IsEnemy)
            {
                foreach (var i in Asteroids.AsteroidsList)
                {
                    if (Rectangle.Intersects(i.Rectangle))
                    {
                        if (Collision.PerPixelCollision(Rectangle, Texture, i.Rectangle, i.Texture))
                        {
                            i.Health -= Damage;
                            Hidden = true;

                            if (i.Health <= 0)
                                i.IsShooted = true;

                            Sounds.FireHitSound.Play(1, 0, 0);
                        }
                    }
                }

                foreach (var i in EnemyShips.ShipsList)
                {
                    if (Rectangle.Intersects(i.Rectangle))
                    {
                        if (Collision.PerPixelCollision(Rectangle, Texture, i.Rectangle, i.Texture))
                        {
                            i.Health -= Damage;
                            Hidden = true;

                            if (i.Health <= 0)
                                i.IsShooted = true;

                            Sounds.FireHitSound.Play(1, 0, 0);
                        }
                        if(i.IsShooted)
                        {
                            if (i is Boss)
                                Inventory.Coins += i.FullHP / 3;
                            else
                                Inventory.Coins++;
                        }
                    }
                }
            }

            Direction = new Vector2(XSpeed, YSpeed);

            if (!Hidden)
                Position += Direction;
        }
    }
}