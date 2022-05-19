using ditto;
using ditto.mono;
using GameForAndroid.Elements.Shots;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;

namespace GameForAndroid.Elements.Gameplay
{

    public static class EnemyShips
    {
        public static int Width, Height;
        public static Random rand = new Random();
        public static int CountShips = 5;
        public static bool isInit;
        public static List<EnemyShip> ShipsList = new List<EnemyShip>();
        private static Texture2D _shipTexture;
        private static Texture2D _shootTexture;
        public static EnemyShip Boss;
        public static bool BossInitialized;

        public static int GetRand(int min, int max) => rand.Next(min, max);
        public static void Init(Texture2D shipTexture, Texture2D shootTexture)
        {
            ShipsList = new List<EnemyShip>();
            _shipTexture = shipTexture;
            _shootTexture = shootTexture;

            for (int i = 0; i < GetRand(1, CountShips); i++)
                ShipsList.Add(new EnemyShip(shipTexture, shootTexture));

            isInit = true;
        }
        public static void InitializeBoss(Texture2D bossTexture)
        {
            BossInitialized = true;

            Boss = Levels.Level.CurrentBoss(bossTexture);
            Boss.Position = new Vector2(Stars.Width + 10, (Stars.Height / 2) - (Boss.Texture.Height / 2));

            ShipsList.Add(Boss);
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            if (!isInit) return;

            foreach (EnemyShip i in ShipsList)
                i.Draw(spriteBatch);
        }
        public static void Update()
        {
            if (!isInit) return;

            for (int i = 0; i < ShipsList.Count; i++)
            {
                ShipsList[i].Update();

                if (ShipsList[i].IsShooted)
                {
                    ShipsList.RemoveAt(i);

                    Game1.Score += 2;

                    i--;
                }
                else if (ShipsList[i].Destroy)
                {
                    ShipsList.RemoveAt(i);
                    i--;
                }

            }

            if (ShipsList.Count == 0)
            {
                if (GetRand(0, 1 + 1) == 1)
                    Init(_shipTexture, _shootTexture);
            }
        }
    }

    public class EnemyShip : Element
    {
        public Vector2 Direction;
        public int Health = 5;
        public bool Destroy;
        public bool IsShooted = false;
        public Texture2D ShootTexture;
        public List<Shot> shoots;
        protected bool _isBoss;
        public int FullHP;

        public EnemyShip(Texture2D texture, Vector2 position, Vector2 direction)
        {
            Position = position;
            Direction = direction;
            Texture = texture;
            shoots = new List<Shot>();
        }
        public EnemyShip(Texture2D shipTexture, Texture2D shootTexture)
        {
            Texture = shipTexture;
            ShootTexture = shootTexture;
            shoots = new List<Shot>();
            FullHP = Health;

            RandomSet();
        }
        public void ChangeHP(int hp)
        {
            Health = hp;
            FullHP = hp;
        }

        public virtual void Shoot()
        {
            shoots.Add(new OneShot(ShootTexture, Rectangle, 1, true) { XSpeed = - 5 });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var i in shoots)
                i.Draw(spriteBatch);

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
        private void RandomSet()
        {
            Position = new Vector2(EnemyShips.GetRand(EnemyShips.Width + Texture.Width, EnemyShips.Width + Texture.Width + 300), EnemyShips.GetRand(Texture.Height, EnemyShips.Height - (Texture.Height * 2)));

            Direction = new Vector2(-EnemyShips.GetRand(2, 4), 0);

            Color = Color.White;
        }
        public override void Update(TouchCollection touches, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            if (_isBoss)
            {
                if (EnemyShips.GetRand(0, 150) == 0)
                    Shoot();
            }
            else
            {
                if (EnemyShips.GetRand(0, 250) == 0)
                    Shoot();
            }


            Position += Direction;

            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            if (Rectangle.Right <= 0 || Health <= 0)
            {
                Destroy = true;
                return;
            }

            for (int i = 0; i < shoots.Count; i++)
            {
                if (shoots[i].Hidden)
                {
                    shoots.RemoveAt(i);
                    i--;
                }
                else
                    shoots[i].Update();
            }

            Color = Color.FromNonPremultiplied(255, 255 / FullHP * Health, 255 / FullHP * Health, 255);
        }
    }
}