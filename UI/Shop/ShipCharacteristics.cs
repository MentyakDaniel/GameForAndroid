using ditto;
using ditto.mono;
using Microsoft.Xna.Framework;

namespace GameForAndroid.UI.Shop
{
    public class ShipCharacteristics : CCRectangle
    {
        public CCLabel Speed;
        public CCLabel Health;
        public CCLabel Damage;
        public CCLabel ShotSpeed;
        public CCLabel Gun;

        public ShipCharacteristics(double width, double height, CCColor background, CCColor borderColor, int borderWidth, int radius)
            : base(width, height, borderWidth, borderColor, background, radius)
        {
            Speed = new CCLabel("Speed:", "RobotoBlack", 30, CCTextAlignment.Center) { Color = CCColors.White };
            Health = new CCLabel("Health:", "RobotoBlack", 30, CCTextAlignment.Center) { Color = CCColors.White };
            Damage = new CCLabel("Damage:", "RobotoBlack", 30, CCTextAlignment.Center) { Color = CCColors.White };
            ShotSpeed = new CCLabel("ShotSpeed:", "RobotoBlack", 30, CCTextAlignment.Center) { Color = CCColors.White };
            Gun = new CCLabel("Gun:", "RobotoBlack", 30, CCTextAlignment.Center) { Color = CCColors.White };

            AddChild(Speed);
            AddChild(Health);
            AddChild(Damage);
            AddChild(ShotSpeed);
            AddChild(Gun);
        }

        public override void Update(GameTime gameTime)
        {
            Speed.Position = new CCPoint(RenderRectangle.X + 10, RenderRectangle.Y + 10);
            Health.Position = new CCPoint(RenderRectangle.X + 10, Speed.RenderRectangle.Bottom + 10);
            Damage.Position = new CCPoint(RenderRectangle.X + 10, Health.RenderRectangle.Bottom + 10);
            ShotSpeed.Position = new CCPoint(RenderRectangle.X + 10, Damage.RenderRectangle.Bottom + 10);
            Gun.Position = new CCPoint(RenderRectangle.X + 10, ShotSpeed.RenderRectangle.Bottom + 10);

            Height = Gun.RenderRectangle.Bottom - Speed.Position.Y + 20;

            base.Update(gameTime);
        }
    }
}