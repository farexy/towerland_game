﻿using Newtonsoft.Json;

namespace Assets.Scripts.Models.Effects
{
  public class SpecialEffect
  {
    public const int FreezedSlowCoeff = 2;
    
    [JsonProperty("i")] public EffectId Effect { set; get; }
    [JsonProperty("d")] public int Duration { set; get; }

    public static SpecialEffect Empty
    {
      get { return new SpecialEffect {Effect = EffectId.None, Duration = 0}; }
    }
  }
}
