﻿using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameObjects;
using Newtonsoft.Json;

namespace Assets.Scripts.Models.Stats
{
  public struct TowerStats : IStats
  {
    [JsonProperty("t")] public GameObjectType Type { set; get; }
    [JsonProperty("d")] public int Damage { set; get; }
    [JsonProperty("r")] public int Range { set; get; }
    [JsonProperty("s")] public int AttackSpeed { set; get; }
    [JsonProperty("a")] public AttackType Attack { set; get; }
    [JsonProperty("tp")] public AttackPriority TargetPriority { set; get; }
    [JsonProperty("e")] public SkillId Skill { set; get; }
    [JsonProperty("c")] public int Cost { set; get; }
    [JsonProperty("p")] public TowerSpawnType SpawnType { set; get; }
    [JsonProperty("i")] public bool Hidden { get; set; }

    public enum AttackType
    {
      Usual,
      Magic,
      Burst
    }

    public enum AttackPriority
    {
      Random,
      Optimal,
      UnitsAtPosition,
    }

    public enum TowerSpawnType
    {
      Ground = 1,
      Tree = 2
    }
  }
}
