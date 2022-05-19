using ditto.mono;
using GameForAndroid.Elements.Shots;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace GameForAndroid.Elements.Gameplay.Bosses
{
    public class Level3Boss : Boss
    {
        public Level3Boss(Texture2D bossTexture)
            : base(bossTexture, CCMonoHelper.ContentManager.Load<Texture2D>("EnemyLaser"))
        { }

        public override void Shoot()
        {
            /*Top*/         shoots.Add(new OneShot(ShootTexture, Rectangle, 1, true) { XSpeed = -5, Position = new Vector2(Rectangle.X + 67, Rectangle.Center.Y - 24 - (ShootTexture.Height / 2)) });

            /*Diagonal*/ shoots.Add(new OneShot(ShootTexture, Rectangle, 1, true) { XSpeed = -4, YSpeed = -1, Position = new Vector2(Rectangle.X + 19, Rectangle.Center.Y - 26 - (ShootTexture.Height / 2)) });

            /*Center*/ shoots.Add(new OneShot(ShootTexture, Rectangle, 2, true) { XSpeed = -6, Position = new Vector2(Rectangle.X, Rectangle.Center.Y - (ShootTexture.Height / 2)) });

            /*Diagonal*/ shoots.Add(new OneShot(ShootTexture, Rectangle, 1, true) { XSpeed = -5, YSpeed = 1, Position = new Vector2(Rectangle.X + 19, Rectangle.Center.Y + 26 - (ShootTexture.Height / 2)) });

            /*Bottom*/      shoots.Add(new OneShot(ShootTexture, Rectangle, 1, true) { XSpeed = -4, Position = new Vector2(Rectangle.X + 67, Rectangle.Center.Y + 24 - (ShootTexture.Height / 2)) });
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