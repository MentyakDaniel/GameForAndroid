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
    public class InventoryScreen : CCRectangle
    {
        public Button CloseInventory;
        public Button EquipButton;
        private Texture2D ShipTexture;
        private ShipCharacteristics ShipCharacteristics;
        public ShipType CurrentShip;
        private Ship PrevieShip;
        private Showcase Showcase;
        private CCLabel _showcaseLabel;
        private CCLabel _currentShipLabel;
        private ShipType _playerShip;
        private CCLabel _haveCoins;

        public InventoryScreen(double width, double height, CCColor background, ShipType currentShip)
            : base(width, height, background)
        {
            ShipCharacteristics = new ShipCharacteristics(200, 100, CCColors.Black, CCColors.White, 1, 10);
            _playerShip = currentShip;

            CurrentShip = currentShip;
            PrevieShip = Ship.GetShip(CurrentShip);
            ShipTexture = PrevieShip.ShipTexture;

            CloseInventory = new Button("Close Inventory", "RobotoBlack", 30, CCTextAlignment.Center, new CCSize(200, 50), CCColors.DarkRed, 10)
            {
                Foreground = CCColors.White
            };
            EquipButton = new Button("Equip", "RobotoBlack", 30, CCTextAlignment.Center, new CCSize(150, 50), CCColors.Green, 10)
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

        public override void Draw(GraphicsDevice device)
        {
            base.Draw(device);

            SpriteBatch.Begin(SpriteSortMode.Immediate);

            SpriteBatch.Draw(ShipTexture, new Vector2((RenderRectangle.Center.X / 2) - ShipTexture.Width, RenderRectangle.Center.Y - (ShipTexture.Height)), null, Color.White, 0, Vector2.Zero, new Vector2(2, 2), SpriteEffects.None, 1);

            SpriteBatch.End();

            CloseInventory.Draw(device);
            EquipButton.Draw(device);
            _currentShipLabel.Draw(device);

            if (Showcase != null)
                Showcase.Draw(device);

            if (_showcaseLabel != null)
                _showcaseLabel.Draw(device);
        }

        public void Update(GameTime gameTime, TouchCollection touches, Player player)
        {
            if (Showcase != null)
            {
                Showcase.Position = new CCPoint(RenderRectangle.Center.X, RenderRectangle.Y + 200);
                _showcaseLabel.Position = new CCPoint(Showcase.RenderRectangle.Center.X - (_showcaseLabel.Width / 2), Showcase.RenderRectangle.Y - 20 - _showcaseLabel.Height);
                _currentShipLabel.Position = new CCPoint((RenderRectangle.Center.X / 2) - (_currentShipLabel.Width / 2), _showcaseLabel.Position.Y);
                _haveCoins.Position = new CCPoint(_currentShipLabel.Position.X, _currentShipLabel.RenderRectangle.Bottom + 50);

                Showcase.Update(gameTime, touches);
                _showcaseLabel.Update(gameTime);

                if (Showcase.SelectedCommodity != null)
                {
                    CurrentShip = Showcase.SelectedCommodity.ShipType;
                    EquipButton.Visible = player.PlayerShip.ShipType != Showcase.SelectedCommodity.ShipType;
                }
                else
                {
                    CurrentShip = player.PlayerShip.ShipType;
                    EquipButton.Visible = false;
                }

                PrevieShip = Ship.GetShip(CurrentShip);
                ShipTexture = PrevieShip.ShipTexture;

                if (EquipButton.IsPressed && Showcase.SelectedCommodity != null)
                {
                    player.ChangePlayerShip(Showcase.SelectedCommodity.ShipType);

                    foreach (var i in Showcase.Commodities)
                    {
                        if (i.ShipType == player.PlayerShip.ShipType)
                            i.IsPurchased = true;
                        else
                            i.IsPurchased = false;
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

            CloseInventory.Position = new CCPoint(RenderRectangle.Center.X - (CloseInventory.RenderRectangle.Width / 2), RenderRectangle.Bottom - 10 - CloseInventory.RenderRectangle.Height);
            EquipButton.Position = new CCPoint(RenderRectangle.Right - (EquipButton.RenderRectangle.Width) - 10, RenderRectangle.Bottom - 10 - EquipButton.RenderRectangle.Height);

            CloseInventory.Update(gameTime, touches);
            EquipButton.Update(gameTime, touches);
            _currentShipLabel.Update(gameTime);


            base.Update(gameTime);
        }
        public void ReInitialize(bool isShop)
        {
            if (Showcase != null)
                Showcase.ReInitialize(isShop);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == nameof(Position))
            {
                if (Showcase == null)
                {
                    Showcase = new Showcase(RenderRectangle.Center.X, RenderRectangle.Y + 200, RenderRectangle.Width / 2 - 10, RenderRectangle.Height - 400, 1, CCColors.White, CCColors.SteelBlue, 10, false);
                    foreach(var i in Showcase.Commodities)
                    {
                        if (i.ShipType == CurrentShip)
                            i.IsPurchased = true;
                        else
                            i.IsPurchased = false;
                    }
                    _showcaseLabel = new CCLabel("Showcase", "RobotoBlack", 60, CCTextAlignment.Center) { Color = CCColors.White };

                    _showcaseLabel.Position = new CCPoint(Showcase.RenderRectangle.Center.X - (_showcaseLabel.Width / 2), Showcase.RenderRectangle.Y - 20 - _showcaseLabel.Height);
                }
            }
            base.OnPropertyChanged(propertyName);
        }
    }
}