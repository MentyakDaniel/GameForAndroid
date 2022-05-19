using ditto.mono;
using GameForAndroid.Elements.Shots;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace GameForAndroid.Elements.Gameplay.Bosses
{
    public class Level7Boss : Boss
    {
        private Texture2D secondLaser;
        public Level7Boss(Texture2D bossTexture)
            : base(bossTexture, CCMonoHelper.ContentManager.Load<Texture2D>("EnemyBlast"))
        {
            secondLaser = CCMonoHelper.ContentManager.Load<Texture2D>("EnemyLaser");
        }

        public override void Shoot()
        {
            shoots.Add(new OneShot(secondLaser, Rectangle, 2, true) { XSpeed = -7, Position = new Vector2(Rectangle.X + 109, Rectangle.Center.Y - 85 - (secondLaser.Height / 2)) });
            shoots.Add(new OneShot(ShootTexture, Rectangle, 3, true) { XSpeed = -5, Position = new Vector2(Rectangle.X, Rectangle.Center.Y - (ShootTexture.Height / 2)) });
            shoots.Add(new OneShot(secondLaser, Rectangle, 2, true) { XSpeed = -7, Position = new Vector2(Rectangle.X + 109, Rectangle.Center.Y + 85 - (secondLaser.Height / 2)) });
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