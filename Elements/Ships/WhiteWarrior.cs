namespace GameForAndroid.Elements.Ships
{
    public class WhiteWarrior : Ship
    {
        public override ShipType ShipType => ShipType.WhiteWarrior;
        public override FireType FireType => FireType.OneShot;
        public override int FireSpeed => 5;
        public override int Damage => 1;
        public override int Health => 10;
        public override int ShipSpeed => 7;

        public override int Price => 0;
    }
}