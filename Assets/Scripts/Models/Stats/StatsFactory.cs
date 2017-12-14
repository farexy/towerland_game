﻿using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameObjects;

namespace Assets.Scripts.Models.Stats
{
    public class StatsFactory
  {
    public UnitStats[] Units =
    {
      new UnitStats
      {
        Type = GameObjectType.Unit_Skeleton,
        Damage = 20,
        Health = 200,
        IsAir = false,
        MovementPriority = UnitStats.MovementPriorityType.Fastest,
        Speed = 2,
        Cost = 50,
        Defence = UnitStats.DefenceType.LightArmor
      },
      new UnitStats
      {
        Type = GameObjectType.Unit_Orc,
        Damage = 30,
        Health = 500,
        IsAir = false,
        MovementPriority = UnitStats.MovementPriorityType.Random,
        Speed = 6,
        Cost = 120,
        Defence = UnitStats.DefenceType.HeavyArmor
      },
      new UnitStats
      {
        Type = GameObjectType.Unit_Impling,
        Damage = 15,
        Health = 250,
        IsAir = false,
        MovementPriority = UnitStats.MovementPriorityType.Random,
        Speed = 4,
        Cost = 200,
        Defence = UnitStats.DefenceType.LightArmor
      },  
      new UnitStats
      {
        Type = GameObjectType.Unit_Goblin,
        Damage = 15,
        Health = 200,
        IsAir = false,
        MovementPriority = UnitStats.MovementPriorityType.Optimal,
        Speed = 3,
        Cost = 250,
        Defence = UnitStats.DefenceType.LightArmor
      }, 
      new UnitStats
      {
        Type = GameObjectType.Unit_Dragon,
        Damage = 25,
        Health = 400,
        IsAir = true,
        MovementPriority = UnitStats.MovementPriorityType.Optimal,
        Speed = 3,
        Cost = 500,
        Defence = UnitStats.DefenceType.Undefended
      },
      new UnitStats
      {
        Type = GameObjectType.Unit_Golem,
        Damage = 30,
        Health = 700,
        IsAir = false,
        MovementPriority = UnitStats.MovementPriorityType.Random,
        Speed = 5,
        Cost = 750,
        Defence = UnitStats.DefenceType.HeavyArmor
      },
    };

    public TowerStats[] Towers =
    {
      new TowerStats
      {
        Type = GameObjectType.Tower_Usual,
        TargetPriority = TowerStats.AttackPriority.Random,
        Attack = TowerStats.AttackType.Usual,
        AttackSpeed = 8,
        Damage = 10,
        Range = 2,
        Cost = 50
      },
      new TowerStats
      {
        Type = GameObjectType.Tower_Frost,
        TargetPriority = TowerStats.AttackPriority.Random,
        Attack = TowerStats.AttackType.Magic,
        AttackSpeed = 3,
        Damage = 4,
        Range = 3,
        Cost = 120,
        SpecialEffects = new []{new SpecialEffect{Effect = EffectId.UnitFreezed, Duration = 16}}
      },
      new TowerStats
      {
        Type = GameObjectType.Tower_Cannon,
        TargetPriority = TowerStats.AttackPriority.Random,
        Attack = TowerStats.AttackType.Burst,
        AttackSpeed = 15,
        Damage = 20,
        Range = 4,
        Cost = 200
      },
      new TowerStats
      {
        Type = GameObjectType.Tower_FortressWatchtower,
        TargetPriority = TowerStats.AttackPriority.Optimal,
        Attack = TowerStats.AttackType.Usual,
        AttackSpeed = 12,
        Damage = 30,
        Range = 5,
        Cost = 350,
      },
      new TowerStats
      {
        Type = GameObjectType.Tower_Magic,
        TargetPriority = TowerStats.AttackPriority.Optimal,
        Attack = TowerStats.AttackType.Magic,
        AttackSpeed = 6,
        Damage = 20,
        Range = 6,
        Cost = 400,
        SpecialEffects = new []{new SpecialEffect{Effect = EffectId.Unit10xDamage_10PercentProbability}}
      }
    };

    public DefenceCoeff[] DefenceCoeffs =
    {
      new DefenceCoeff
      {
        Defence = UnitStats.DefenceType.Undefended,
        Attack = TowerStats.AttackType.Usual,
        Coeff = 0.9
      },
      new DefenceCoeff
      {
        Defence = UnitStats.DefenceType.Undefended,
        Attack = TowerStats.AttackType.Burst,
        Coeff = 1
      },
      new DefenceCoeff
      {
        Defence = UnitStats.DefenceType.Undefended,
        Attack = TowerStats.AttackType.Magic,
        Coeff = 0.8
      },
      new DefenceCoeff
      {
        Defence = UnitStats.DefenceType.LightArmor,
        Attack = TowerStats.AttackType.Usual,
        Coeff = 0.7
      },
      new DefenceCoeff
      {
        Defence = UnitStats.DefenceType.LightArmor,
        Attack = TowerStats.AttackType.Burst,
        Coeff = 0.9
      },
      new DefenceCoeff
      {
        Defence = UnitStats.DefenceType.LightArmor,
        Attack = TowerStats.AttackType.Magic,
        Coeff = 0.5
      },
      new DefenceCoeff
      {
        Defence = UnitStats.DefenceType.HeavyArmor,
        Attack = TowerStats.AttackType.Usual,
        Coeff = 0.7
      },
      new DefenceCoeff
      {
        Defence = UnitStats.DefenceType.HeavyArmor,
        Attack = TowerStats.AttackType.Burst,
        Coeff = 0.5
      },
      new DefenceCoeff
      {
        Defence = UnitStats.DefenceType.HeavyArmor,
        Attack = TowerStats.AttackType.Magic,
        Coeff = 0.2
      },
    };
  }
}