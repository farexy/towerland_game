using Assets.Scripts.Models.Effects;
using Newtonsoft.Json;

namespace Assets.Scripts.Models.GameObjects
{
  public class Castle : GameObjectLogical
  {
    public Castle()
    {
      Type = GameObjectType.Castle;
    }

    [JsonProperty("h")] public int Health { set; get; }

    public override object Clone()
    {
      return new Castle
      {
        GameId = GameId,
        Position = Position,
        Type = Type,
        WaitTicks = WaitTicks,
        Effect = new SpecialEffect{Effect = Effect.Effect, Duration = Effect.Duration},
        Health = Health,
      };
    }
  }
}
