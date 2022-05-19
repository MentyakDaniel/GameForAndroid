using ditto;
using ditto.mono;
using Microsoft.Xna.Framework.Graphics;

namespace GameForAndroid.Elements.Gameplay.Bosses
{
    public abstract class Boss : EnemyShip
    {
        protected CCRectangle HpBarBackground;
        protected CCRectangle HpBar;
        protected int startWidth;

        public Boss(Texture2D bossTexture, Texture2D shootTexture)
            : base(bossTexture, shootTexture)
        {
            Health = 100;
            HpBarBackground = new CCRectangle(Stars.Width - 300, 50, CCColors.DarkGray, 50)
            {
                Position = new CCPoint(2, 5)
            };

            HpBar = new CCRectangle(Stars.Width - 300, 50, CCColors.Red, 50)
            {
                Position = new CCPoint(2, 5)
            };

            startWidth = HpBar.RenderRectangle.Width;
            FullHP = Health;
            _isBoss = true;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            HpBarBackground.Draw(spriteBatch.GraphicsDevice);

            HpBar.Draw(spriteBatch.GraphicsDevice);

            base.Draw(spriteBatch);
        }
    }
}