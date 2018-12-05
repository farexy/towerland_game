
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
