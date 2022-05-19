using ditto.mono;
using GameForAndroid.Elements.Shots;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace GameForAndroid.Elements.Gameplay.Bosses
{
    public class Level1Boss : Boss
    {
        public Level1Boss(Texture2D bossTexture)
            : base(bossTexture, CCMonoHelper.ContentManager.Load<Texture2D>("EnemyLaser"))
        {
        }

        public override void Shoot()
        {
            shoots.Add(new OneShot(ShootTexture, Rectangle, 1, true) { XSpeed = -5, Position = new Vector2(Rectangle.X, Rectangle.Center.Y - 120 - (ShootTexture.Height / 2)) });
            shoots.Add(new OneShot(ShootTexture, Rectangle, 1, true) { XSpeed = -5, Position = new Vector2(Rectangle.X, Rectangle.Center.Y - (ShootTexture.Height / 2))       });
            shoots.Add(new OneShot(ShootTexture, Rectangle, 1, true) { XSpeed = -5, Position = new Vector2(Rectangle.X, Rectangle.Center.Y + 120 - (ShootTexture.Height / 2)) });
        }

        public override void Update(TouchCollection touches, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            base.Update();

            if (Rectangle.Right < Stars.Width)
                Direction = Vector2.Zero;

            HpBar.Width = Health * (startWidth / FullHP);
        }
    }
}