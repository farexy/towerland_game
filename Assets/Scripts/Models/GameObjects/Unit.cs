
using Assets.Scripts.Models.Effects;
using Newtonsoft.Json;

namespace Assets.Scripts.Models.GameObjects
{
  public class Unit : GameObjectLogical
  {
    public Unit()
    {
      Type = GameObjectType.Unit;
    }

    [JsonProperty("h")] public int Health { set; get; }
    [JsonProperty("z")] public int? PathId { set; get; }

    public override object Clone()
    {
      return new Unit
      {
        GameId = GameId,
        Position = Position,
        Type = Type,
        WaitTicks = WaitTicks,
        Effect = new SpecialEffect{Id = Effect.Id, Duration = Effect.Duration},
        Health = Health,
        PathId = PathId
      };
    }
  }
}
