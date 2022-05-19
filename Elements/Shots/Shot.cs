using Microsoft.Xna.Framework;


namespace GameForAndroid.Elements.Shots
{
    public abstract class Shot : Element
    {
        public Vector2 Direction;
        public Rectangle PlayerRect;

        public int Damage = 1;

        public int XSpeed = 5;
        public int YSpeed = 0;

        public bool IsEnemy;

        private bool hidden;
        public bool Hidden
        {
            get => Position.X > Stars.Width || hidden;
            set => hidden = value;
        }
    }
}