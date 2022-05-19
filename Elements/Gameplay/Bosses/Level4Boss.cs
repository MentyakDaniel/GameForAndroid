using ditto.mono;
using GameForAndroid.Elements.Shots;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace GameForAndroid.Elements.Gameplay.Bosses
{
    public class Level4Boss : Boss
    {
        public Level4Boss(Texture2D bossTexture)
            : base(bossTexture, CCMonoHelper.ContentManager.Load<Texture2D>("EnemyBlast"))
        { }

        public override void Shoot()
        {
            /*Center*/
            shoots.Add(new OneShot(ShootTexture, Rectangle, 3, true) { XSpeed = -7, Position = new Vector2(Rectangle.X, Rectangle.Center.Y - (ShootTexture.Height / 2)) });
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