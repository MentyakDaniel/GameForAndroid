using ditto;
using ditto.mono;
using GameForAndroid.Elements.Gameplay;
using GameForAndroid.Elements.Ships;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameForAndroid.UI.Shop
{
    public class Commodity : CCRectangle
    {
        public static int WIDTH = 300;
        public static int HEIGHT = 250;
        private Texture2D productIcon;
        private CCLabel productText;
        private Texture2D _isPurchase;
        private TouchCollection _lastTouch;
        public int Price;
        public bool IsPressed { get; set; }
        public bool IsPurchased { get; set; }

        public bool IsShop;

        public ShipType ShipType;
        public Commodity(ShipType name, Texture2D product, bool isShop = true) 
            : base(WIDTH, HEIGHT, CCColors.Transparent,10)
        {
            productIcon = product;
            ShipType = name;
            IsShop = isShop;

            if (isShop)
            {
                foreach (var i in Inventory.PurchasedShips)
                {
                    if (i == ShipType)
                    {
                        IsPurchased = true;
                        break;
                    }
                }
            }

            Price = Ship.GetShip(name).Price;

            if(isShop)
                productText = new CCLabel($"{name}. Price: {Price}", "RobotoBlack", 30, CCTextAlignment.Center) { Color = CCColors.White };
            else
                productText = new CCLabel($"{name}", "RobotoBlack", 30, CCTextAlignment.Center) { Color = CCColors.White };

            _isPurchase = CCMonoHelper.ContentManager.Load<Texture2D>("Purchased");


            ScaleX = (WIDTH *.6) / productIcon.Width;
            ScaleY = (HEIGHT * .6) / productIcon.Height;
            
            AddChild(productText);
        }

        public override void Draw(GraphicsDevice device)
        {
            base.Draw(device);
            SpriteBatch.Begin(SpriteSortMode.Immediate);

            SpriteBatch.Draw(productIcon, 
                new Vector2((float)(RenderRectangle.Center.X - ((productIcon.Width * ScaleX) / 2)), (float)(RenderRectangle.Center.Y - ((productIcon.Height * ScaleY) / 2))), 
                null, Color.White, 0, Vector2.Zero, new Vector2((float)ScaleX, (float)ScaleY), SpriteEffects.None, 1);

            if(IsPurchased)
                SpriteBatch.Draw(_isPurchase, new Vector2(RenderRectangle.Right - (_isPurchase.Width / 2), RenderRectangle.Y - (_isPurchase.Height / 2)), Color.White);

            SpriteBatch.End();
        }

        public void Update(GameTime gameTime, TouchCollection touches)
        {
            foreach (var i in touches)
                if (RenderRectangle.Contains(i.Position))
                {
                    if (_lastTouch.Count > 0 && !RenderRectangle.Contains(_lastTouch[0].Position))
                    {
                        IsPressed = !IsPressed;
                        OnPropertyChanged(nameof(IsPressed));
                    }
                    else if (_lastTouch.Count == 0)
                    {
                        IsPressed = !IsPressed;
                        OnPropertyChanged(nameof(IsPressed));
                    }
                }

            if (IsPressed)
                Background = new CCColor(99, 99, 99);
            else
                Background = CCColors.Transparent;

            productText.Position = new CCPoint(RenderRectangle.Center.X - (productText.Width / 2), RenderRectangle.Bottom - 10 - productText.Height);

            _lastTouch = touches;
            base.Update(gameTime);
        }
    }
}