using ditto;
using ditto.mono;
using GameForAndroid.Elements.Gameplay;
using GameForAndroid.Elements.Ships;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace GameForAndroid.UI.Shop
{
    public class Showcase : CCRectangle
    {
        public List<Commodity> Commodities;
        public Commodity SelectedCommodity;
        public Showcase(int x, int y, double width, double height, double borderWidth, CCColor borderColor, CCColor background, int radius, bool isShop = true)
            : base(width, height, borderWidth, borderColor, background, radius)
        {
            Position = new CCPoint(x, y);

            Commodities = new List<Commodity>();

            int xPos = x + 20, yPos = 0 + 20;

            if (isShop)
            {
                for (int i = 0; i < Ship.CountShipTypes; i++)
                {
                    Ship ship = Ship.GetShip((ShipType)i);

                    Commodity commodity = new Commodity(ship.ShipType, ship.ShipTexture) { Position = new CCPoint(xPos, yPos) };
                    commodity.PropertyChanged += Commodity_PropertyChanged;

                    Commodities.Add(commodity);

                    if (xPos + Commodity.WIDTH + 20 >= RenderRectangle.Right)
                    {
                        xPos = x + 20;
                        yPos += Commodity.HEIGHT + 20;
                    }
                    else
                        xPos += Commodity.WIDTH + 20;
                }
            }
            else
            {
                foreach(var i in Inventory.PurchasedShips)
                {
                    Ship ship = Ship.GetShip(i);

                    Commodity commodity = new Commodity(ship.ShipType, ship.ShipTexture, false) { Position = new CCPoint(xPos, yPos) };
                    commodity.PropertyChanged += Commodity_PropertyChanged;

                    Commodities.Add(commodity);

                    if (xPos + Commodity.WIDTH + 20 >= RenderRectangle.Right)
                    {
                        xPos = x + 20;
                        yPos += Commodity.HEIGHT + 20;
                    }
                    else
                        xPos += Commodity.WIDTH + 20;
                }
            }
        }

        public void ReInitialize(bool isShop)
        {
            Commodities.Clear();
            SelectedCommodity = null;

            int xPos = (int)Position.X + 20, yPos = (int)Position.Y + 20;
            if (isShop)
            {
                for (int i = 0; i < Ship.CountShipTypes; i++)
                {
                    Ship ship = Ship.GetShip((ShipType)i);

                    Commodity commodity = new Commodity(ship.ShipType, ship.ShipTexture) { Position = new CCPoint(xPos, yPos) };
                    commodity.PropertyChanged += Commodity_PropertyChanged;

                    Commodities.Add(commodity);

                    if (xPos + Commodity.WIDTH + 20 >= RenderRectangle.Right)
                    {
                        xPos = (int)Position.X + 20;
                        yPos += Commodity.HEIGHT + 20;
                    }
                    else
                        xPos += Commodity.WIDTH + 20;
                }
            }
            else
            {
                foreach (var i in Inventory.PurchasedShips)
                {
                    Ship ship = Ship.GetShip(i);

                    Commodity commodity = new Commodity(ship.ShipType, ship.ShipTexture, false) { Position = new CCPoint(xPos, yPos) };
                    commodity.PropertyChanged += Commodity_PropertyChanged;

                    Commodities.Add(commodity);

                    if (xPos + Commodity.WIDTH + 20 >= RenderRectangle.Right)
                    {
                        xPos = (int)Position.X + 20;
                        yPos += Commodity.HEIGHT + 20;
                    }
                    else
                        xPos += Commodity.WIDTH + 20;
                }
            }
        }

        private void Commodity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Commodity commodity = sender as Commodity;

            if (e.PropertyName == nameof(commodity.IsPressed))
            {
                if (commodity.IsPressed == true)
                {
                    foreach (var i in Commodities)
                    {
                        if (commodity == i) { SelectedCommodity = i; continue; }

                        i.IsPressed = false;
                    }
                }
                else
                {
                    SelectedCommodity = null;
                    commodity.IsPressed = false;
                }
            }
        }

        public void Update(GameTime gameTime, TouchCollection touches)
        {
            int xPos = (int)(Position.X + 20), yPos = (int)(Position.Y + 20);

            for (int i = 0; i < Commodities.Count; i++)
            {
                Commodities[i].Position = new CCPoint(xPos, yPos);

                Commodities[i].Update(gameTime, touches);

                if ((xPos + Commodity.WIDTH + 20 + Commodity.WIDTH) >= RenderRectangle.Right)
                {
                    xPos = (int)(Position.X + 20);
                    yPos += Commodity.HEIGHT + 20;
                }
                else
                    xPos += Commodity.WIDTH + 20;
            }

            base.Update(gameTime);
        }

        public override void Draw(GraphicsDevice device)
        {
            base.Draw(device);

            foreach (var i in Commodities)
                i.Draw(device);
        }
    }
}