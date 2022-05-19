namespace GameForAndroid.Elements.Ships
{
    public class Tank : Ship
    {
        public override ShipType ShipType => ShipType.Tank;
        public override FireType FireType => FireType.Triple;
        public override int FireSpeed => 4;
        public override int Damage => 1;
        public override int Health => 15;
        public override int ShipSpeed => 5;

        public override int Price => 150;
    }
}