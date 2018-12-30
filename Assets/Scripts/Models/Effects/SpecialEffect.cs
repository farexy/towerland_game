using System;
using Newtonsoft.Json;

namespace Assets.Scripts.Models.Effects
{
  public class SpecialEffect
  {
    public const int FreezedSlowCoeff = 2;
    
    [JsonProperty("i")] public EffectId Id { set; get; }
    [JsonProperty("d")] public int Duration { set; get; }
    [JsonProperty("e")] public double EffectValue { get; set; }


    public static SpecialEffect Empty
    {
      get { return new SpecialEffect {Id = EffectId.None, Duration = 0}; }
    }

    public BuffType GetBuffOrDebuffType()
    {
      return GetBuffOrDebuffType(Id);
    }

    public static BuffType GetBuffOrDebuffType(EffectId effectId)
    {
      switch (effectId)
      {
        case EffectId.None:
          return BuffType.None;
        case EffectId.SkillsDisabled:
          return BuffType.SkillDebuff;
        case EffectId.UnitFreezed:
          return BuffType.SpeedDebuff;
        case EffectId.UnitPoisoned:
          return BuffType.ConstantDamageDebuff;
        default:
          throw new ArgumentOutOfRangeException(nameof(effectId), effectId, null);
      }
    }
    
    public enum BuffType
    {
      None,
      SpeedBuff,
      SpeedDebuff,
      ConstantHealBuff,
      ConstantDamageDebuff,
      SkillDebuff
    }
    
    public object Clone()
    {
      return new SpecialEffect
      {
        Id = Id,
        Duration = Duration,
        EffectValue = EffectValue
      };
    }
  }
}
