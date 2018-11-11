using Newtonsoft.Json;

namespace Assets.Scripts.Models.Effects
{
  public class SpecialEffect
  {
    public const int FreezedSlowCoeff = 2;
    
    [JsonProperty("i")] public EffectId Id { set; get; }
    [JsonProperty("d")] public int Duration { set; get; }

    public static SpecialEffect Empty
    {
      get { return new SpecialEffect {Id = EffectId.None, Duration = 0}; }
    }
  }
}
