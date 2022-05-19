using GameForAndroid.Elements.Ships;
using GameForAndroid.Levels;
using System;
using System.Collections.Generic;

namespace GameForAndroid.Save
{
    [Serializable]
    public class SaveObject
    {
        public int Highscore { get; set; }
        public int Coints { get; set; }
        public ShipType PlayerShip { get; set; }
        public LevelsName CurrentLevel { get; set; }
        public List<ShipType> PurchaseShips { get; set; }
    }
}