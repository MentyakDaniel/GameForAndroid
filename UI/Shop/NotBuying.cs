using ditto;
using ditto.mono;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameForAndroid.UI.Shop
{
    public class NotBuying : CCRectangle
    {
        public const int WIDTH = 100;
        public const int HEIGHT = 100;

        public CCLabel Title;

        public TextButton Exit;
        public NotBuying() 
            : base(WIDTH, HEIGHT, CCColors.CadetBlue, 10)
        {
            Title = new CCLabel("You do not have enough coins", "RobotoBlack", 80, CCTextAlignment.Center) { Color = CCColors.White };
            Exit = new TextButton("Ok", "RobotoBlack", 60) { Color = CCColors.Green };

            Width = Title.Width + 20;
            Height = Title.Height + Exit.Height + 20;

            AddChild(Title);
            AddChild(Exit);
        }

        public void Update(GameTime gameTime, TouchCollection touches)
        {
            Title.Position = new CCPoint(RenderRectangle.Center.X - (Title.RenderRectangle.Width / 2), RenderRectangle.Y + 10);
            Exit.Position = new CCPoint(RenderRectangle.Center.X - (Exit.RenderRectangle.Width / 2), RenderRectangle.Bottom - Exit.Height);

            if (Exit.IsPressed)
                Visible = false;

            Exit.Update(touches);

            base.Update(gameTime);
        }
    }
}