using ditto;
using ditto.mono;
using GameForAndroid.Elements.Gameplay.Bosses;
using GameForAndroid.Elements.Ships;
using GameForAndroid.Elements.Shots;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace GameForAndroid.Elements.Gameplay
{
    public class Player : Element
    {
        public bool IsShoot;
        public int Speed;
        readonly List<Shot> shots;
        public Texture2D ShootTexture;
        public int Damage;
        public int Health;
        public int FullHealth;
        public Ship PlayerShip;
        
        public Player(ShipType ship, Vector2 startPos)
        {
            ChangePlayerShip(ship);

            Position = startPos;
            shots = new List<Shot>();
        }
        public void ReloadShip()
        {
            Texture = PlayerShip.ShipTexture;
            Health = PlayerShip.Health;
            Damage = PlayerShip.Damage;
            Speed = PlayerShip.ShipSpeed;
            ShootTexture = PlayerShip.ShotTexture;
        }
        public void ChangePlayerShip(ShipType shipType)
        {
            PlayerShip = Ship.GetShip(shipType);

            ReloadShip();

            FullHealth = Health;
        }

        public void Shoot()
        {
            switch (PlayerShip.FireType)
            {
                case FireType.OneShot:
                    shots.Add(new OneShot(PlayerShip.ShotTexture, Rectangle, PlayerShip.Damage) { XSpeed = PlayerShip.FireSpeed });
                    break;
                case FireType.Duplet:
                    shots.Add(new DupletShot(PlayerShip.ShotTexture, Rectangle, PlayerShip.Damage) { XSpeed = PlayerShip.FireSpeed });
                    break;
                case FireType.Triple:
                    shots.Add(new TripleShoot(PlayerShip.ShotTexture, Rectangle, PlayerShip.Damage) { XSpeed = PlayerShip.FireSpeed });
                    break;
                case FireType.BigShot:
                    shots.Add(new OneShot(PlayerShip.ShotTexture, Rectangle, PlayerShip.Damage) { XSpeed = PlayerShip.FireSpeed });
                    break;
            }

            Sounds.FireSound.Play(.7f, 0, 0);
        }
        public void Up()
        {
            if (Position.Y > 0)
                Position = new Vector2(Position.X, Position.Y - Speed);
        }
        public void Down()
        {
            if(Position.Y < Stars.Height - Texture.Height)
                Position = new Vector2(Position.X, Position.Y + Speed);
        }
        public void Left()
        {
            if (Position.X > 0)
                Position = new Vector2(Position.X - Speed, Position.Y);
        }
        public void Right()
        {
            if (Position.X < Stars.Width - Texture.Width)
                Position = new Vector2(Position.X + Speed, Position.Y);
        }
        public void ClearShoots()
        {
            shots.Clear();
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
            foreach (var i in shots)
                i.Draw(spriteBatch);
        }
        public override void Update()
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            Color = Color.FromNonPremultiplied(255, (255 / FullHealth) * Health, (255 / FullHealth) * Health, 255);

            foreach (var i in Asteroids.AsteroidsList)
            {
                if (Rectangle.Intersects(i.Rectangle))
                {
                    if (Collision.PerPixelCollision(Rectangle, Texture, i.Rectangle, i.Texture))
                    {
                        Health--;
                        i.Destroy = true;

                        if (Health > 0)
                            Sounds.ShipHitSound.Play(.8f, 0, 0);
                        else if (Health <= 0)
                            Sounds.ShipDestroySound.Play(.8f, 0, 0);
                    }
                }
            }

            foreach (var i in EnemyShips.ShipsList)
            {
                if (Rectangle.Intersects(i.Rectangle))
                {
                    if (Collision.PerPixelCollision(Rectangle, Texture, i.Rectangle, i.Texture))
                    {
                        Health -= 2;
                        i.Destroy = true;

                        Inventory.Coins++;

                        if (Health > 0)
                            Sounds.ShipHitSound.Play(.8f, 0, 0);
                        else if (Health <= 0)
                            Sounds.ShipDestroySound.Play(.8f, 0, 0);
                    }
                }

                for (int counter = 0; counter < i.shoots.Count; counter++)
                {
                    if(i.shoots[counter].Rectangle.Intersects(Rectangle))
                    {
                        if (Collision.PerPixelCollision(Rectangle, Texture, i.shoots[counter].Rectangle, i.shoots[counter].Texture))
                        {
                            Health--;
                            i.shoots[counter].Hidden = true;

                            if (Health > 0)
                                Sounds.ShipHitSound.Play(.8f, 0, 0);
                            else if (Health <= 0)
                                Sounds.ShipDestroySound.Play(.8f, 0, 0);
                        }
                    }
                    if (i.shoots[counter].Hidden)
                    {
                        i.shoots.RemoveAt(counter);
                        counter--;
                    }
                    else
                        i.shoots[counter].Update();
                }
            }


            for (int i = 0; i < shots.Count; i++)
            {
                if (shots[i].Hidden)
                {
                    shots.RemoveAt(i);
                    i--;
                }
                else
                    shots[i].Update();
            }
        }

        public override void Update(TouchCollection touches, GameTime gameTime)
        {
        }
    }
}