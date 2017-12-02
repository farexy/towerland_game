﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.GameObjects;

namespace Assets.Scripts.Models.GameField
{
    public class FieldState
    {
        public FieldState()
        {
            Towers = new List<Tower>();
            Units = new List<Unit>();
        }

        public FieldState(IEnumerable<Tower> towers, IEnumerable<Unit> units)
        {
            Towers = towers.ToList();
            Units = units.ToList();
        }
    
        public Dictionary<int, GameObjectLogical> Objects { set; get; }
        public List<Tower> Towers { private set; get; }
        public List<Unit> Units { private set; get; }
        public Castle Castle { set; get; }
    
        public int MonsterMoney { set; get; }
        public int TowerMoney { set; get; }
    }
}