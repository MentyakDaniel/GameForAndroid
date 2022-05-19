namespace GameForAndroid.Elements.Ships
{
    public class DamageDiller : Ship
    {
        public override ShipType ShipType => ShipType.DamageDiller;
        public override FireType FireType => FireType.BigShot;
        public override int FireSpeed => 7;
        public override int Damage => 3;
        public override int Health => 5;
        public override int ShipSpeed => 10;
        public override int Price => 300;
    }
}