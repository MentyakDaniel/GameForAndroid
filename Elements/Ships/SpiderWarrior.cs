namespace GameForAndroid.Elements.Ships
{
    public class SpiderWarrior : Ship
    {
        public override ShipType ShipType => ShipType.SpiderWarrior;

        public override FireType FireType => FireType.Duplet;

        public override int FireSpeed => 6;

        public override int Damage => 2;

        public override int Health => 7;
        public override int ShipSpeed => 6;

        public override int Price => 100;
    }
}