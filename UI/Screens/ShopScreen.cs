using ditto;
using ditto.mono;
using GameForAndroid.Elements.Gameplay;
using GameForAndroid.Elements.Ships;
using GameForAndroid.UI.Shop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameForAndroid.UI.Screens
{
    public class ShopScreen : CCRectangle
    {
        public Button CloseShop;
        public Button BuyButton;
        private Texture2D ShipTexture;
        private ShipCharacteristics ShipCharacteristics;
        public ShipType CurrentShip;
        private Ship PrevieShip;
        private Showcase Showcase;
        private CCLabel _showcaseLabel;
        private CCLabel _currentShipLabel;
        private ShipType _playerShip;
        private NotBuying NotBuying;
        private CCLabel _haveCoins;

        public ShopScreen(double width, double height, CCColor background, ShipType currentShip)
            : base(width, height, background)
        {
            ShipCharacteristics = new ShipCharacteristics(200, 100, CCColors.Black, CCColors.White, 1, 10);
            _playerShip = currentShip;

            CurrentShip = currentShip;
            PrevieShip = Ship.GetShip(CurrentShip);
            ShipTexture = PrevieShip.ShipTexture;

            NotBuying = new NotBuying
            {
                Visible = false
            };

            CloseShop = new Button("Close Shop", "RobotoBlack", 30, CCTextAlignment.Center, new CCSize(150, 50), CCColors.DarkRed, 10)
            {
                Foreground = CCColors.White
            };
            BuyButton = new Button("Buy", "RobotoBlack", 30, CCTextAlignment.Center, new CCSize(150, 50), CCColors.Green, 10)
            {
                Foreground = CCColors.Black,
                Visible = false
            };
            _haveCoins = new CCLabel("Coints: ", "RobotoBlack", 40, CCTextAlignment.Center) { Color = CCColors.White };

            _currentShipLabel = new CCLabel("Current Ship", "RobotoBlack", 60, CCTextAlignment.Center) { Color = CCColors.White };

            AddChild(ShipCharacteristics);
            AddChild(_currentShipLabel);
            AddChild(_haveCoins);
        }

        public void ReInitialize(bool isShop)
        {
            if(Showcase != null)
                Showcase.ReInitialize(isShop);
        }
        public override void Draw(GraphicsDevice device)
        {
            base.Draw(device);

            SpriteBatch.Begin(SpriteSortMode.Immediate);

            SpriteBatch.Draw(ShipTexture, new Vector2((RenderRectangle.Center.X / 2) -ShipTexture.Width, RenderRectangle.Center.Y - (ShipTexture.Height)), null, Color.White, 0, Vector2.Zero, new Vector2(2, 2), SpriteEffects.None, 1);

            SpriteBatch.End();

            CloseShop.Draw(device);
            BuyButton.Draw(device);
            _currentShipLabel.Draw(device);

            if (Showcase != null)
                Showcase.Draw(device);

            if (_showcaseLabel != null)
                _showcaseLabel.Draw(device);

            NotBuying.Draw(device);
        }

        public void Update(GameTime gameTime, TouchCollection touches, Player player)
        {
            if (Showcase != null)
            {
                Showcase.Position = new CCPoint(RenderRectangle.Right - Showcase.Width - 10, RenderRectangle.Y + 200);
                _showcaseLabel.Position = new CCPoint(Showcase.RenderRectangle.Center.X - (_showcaseLabel.Width / 2), Showcase.RenderRectangle.Y - 20 - _showcaseLabel.Height);
                _currentShipLabel.Position = new CCPoint((RenderRectangle.Center.X / 2) - (_currentShipLabel.Width / 2), _showcaseLabel.Position.Y);
                _haveCoins.Position = new CCPoint(_currentShipLabel.Position.X, _currentShipLabel.RenderRectangle.Bottom + 50);

                Showcase.Update(gameTime, touches);
                _showcaseLabel.Update(gameTime);

                if (Showcase.SelectedCommodity != null)
                {
                    CurrentShip = Showcase.SelectedCommodity.ShipType;

                    foreach (var i in Inventory.PurchasedShips)
                    {
                        if (i == Showcase.SelectedCommodity.ShipType)
                        {
                            BuyButton.Visible = false;
                            break;
                        }
                        BuyButton.Visible = true;
                    }
                }
                else
                {
                    CurrentShip = _playerShip;
                    BuyButton.Visible = false;
                }

                PrevieShip = Ship.GetShip(CurrentShip);
                ShipTexture = PrevieShip.ShipTexture;

                if (BuyButton.IsPressed && Showcase.SelectedCommodity != null && !Showcase.SelectedCommodity.IsPurchased)
                {
                    if (Inventory.Coins >= Showcase.SelectedCommodity.Price)
                    {
                        Showcase.SelectedCommodity.IsPurchased = true;
                        Inventory.Coins -= Showcase.SelectedCommodity.Price;
                        Inventory.PurchasedShips.Add(Showcase.SelectedCommodity.ShipType);
                    }
                    else
                    {
                        NotBuying.Visible = true;
                    }
                }

            }

            ShipCharacteristics.Position = new CCPoint(RenderRectangle.X + 50, RenderRectangle.Bottom - 100 - ShipCharacteristics.Height);

            _haveCoins.Text = $"You have: {Inventory.Coins} coins";

            ShipCharacteristics.Speed.Text = $"Speed: {PrevieShip.ShipSpeed}";
            ShipCharacteristics.Health.Text = $"Health: {PrevieShip.Health}";
            ShipCharacteristics.ShotSpeed.Text = $"ShotSpeed: {PrevieShip.FireSpeed}";
            ShipCharacteristics.Gun.Text = $"Gun: {PrevieShip.FireType}";
            ShipCharacteristics.Damage.Text = $"Damage: {PrevieShip.Damage}";

            CloseShop.Position = new CCPoint(RenderRectangle.Center.X - (CloseShop.RenderRectangle.Width / 2), RenderRectangle.Bottom - 10 - CloseShop.RenderRectangle.Height);
            BuyButton.Position = new CCPoint(RenderRectangle.Right - (BuyButton.RenderRectangle.Width) - 10, RenderRectangle.Bottom - 10 - BuyButton.RenderRectangle.Height);
            NotBuying.Position = new CCPoint(RenderRectangle.Center.X - (NotBuying.RenderRectangle.Width / 2), RenderRectangle.Center.Y - (NotBuying.RenderRectangle.Height / 2));

            CloseShop.Update(gameTime, touches);
            BuyButton.Update(gameTime, touches);
            NotBuying.Update(gameTime, touches);
            _currentShipLabel.Update(gameTime);


            base.Update(gameTime);
        }
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if(propertyName == nameof(Position))
            {
                if (Showcase == null)
                {
                    Showcase = new Showcase(RenderRectangle.Center.X, RenderRectangle.Y + 200, RenderRectangle.Width / 1.7 - 10, RenderRectangle.Height - 300, 1, CCColors.White, CCColors.SteelBlue, 10);
                    _showcaseLabel = new CCLabel("Showcase", "RobotoBlack", 60, CCTextAlignment.Center) { Color = CCColors.White};

                    _showcaseLabel.Position = new CCPoint(Showcase.RenderRectangle.Center.X - (_showcaseLabel.Width / 2), Showcase.RenderRectangle.Y - 20 - _showcaseLabel.Height);
                }
            }
            base.OnPropertyChanged(propertyName);
        }
    }
}