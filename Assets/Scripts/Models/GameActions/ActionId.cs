﻿namespace Assets.Scripts.Models.GameActions
{
  public enum ActionId
  {
    Reserved = 0,

    
    
    Tower = 10,
    TowerAttacks = 11,
    TowerAttacksPosition = 12,
    TowerKills = 13,

    TowerRecharges = 21,
    TowerSearches = 22,

    
    
    Unit = 100,
    UnitMoves = 101,
    UnitMovesFreezed = 102,

    UnitDies = 201,
    UnitRecievesDamage = 202,
    UnitFreezes = 210,

    UnitAttacksCastle = 301,

    UnitEffectCanseled = 401,
    
    
    
    Other = 1000,
    MonsterPlayerRecievesMoney = 1001,
    TowerPlayerRecievesMoney = 1002
    
  }
}