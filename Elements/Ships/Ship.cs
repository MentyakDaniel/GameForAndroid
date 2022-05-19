using ditto.mono;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameForAndroid.Elements.Ships
{
    public enum ShipType
    {
        WhiteWarrior,
        SpiderWarrior,
        Tank,
        DamageDiller
    }
    public enum FireType
    {
        OneShot,
        Duplet,
        Triple,
        BigShot
    }

    public abstract class Ship
    {
        public abstract int Price { get; }
        public abstract ShipType ShipType { get; }
        public abstract FireType FireType { get; }
        public abstract int FireSpeed { get; }
        public abstract int Damage { get; }
        public abstract int Health { get; }
        public abstract int ShipSpeed { get; }
        public static int CountShipTypes => Enum.GetNames(typeof(ShipType)).Length;
        public Texture2D ShotTexture => CCMonoHelper.ContentManager.Load<Texture2D>($"{ShipType}Laser");
        public Texture2D ShipTexture => CCMonoHelper.ContentManager.Load<Texture2D>(ShipType.ToString());

        public static Ship GetShip(ShipType shipType) => shipType switch
        {
            ShipType.WhiteWarrior => new WhiteWarrior(),
            ShipType.SpiderWarrior => new SpiderWarrior(),
            ShipType.Tank => new Tank(),
            ShipType.DamageDiller => new DamageDiller(),
            _ => new WhiteWarrior(),
        };
    }
}