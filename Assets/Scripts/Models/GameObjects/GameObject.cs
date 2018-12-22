using System;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameField;
using Newtonsoft.Json;

namespace Assets.Scripts.Models.GameObjects
{
  public class GameObjectLogical : ICloneable, IEquatable<GameObjectLogical>
  {
    [JsonProperty("i")] public int GameId { set; get; }
    [JsonProperty("p")] public Point Position { get; set; }
    [JsonProperty("w")] public int WaitTicks { set; get; }
    [JsonProperty("e")] public SpecialEffect Effect { set; get; }
    [JsonProperty("t")] public GameObjectType Type { set; get; }
    [JsonProperty("h")] public int Health { set; get; }
    [JsonProperty("z")] public int? PathId { set; get; }

    protected GameObjectLogical()
    {
      Effect = SpecialEffect.Empty;
    }

    [JsonIgnore]
    public bool IsTower
    {
      get { return ResolveType() == GameObjectType.Tower; }
    }

    [JsonIgnore]
    public bool IsUnit
    {
      get { return ResolveType() == GameObjectType.Unit; }
    }
    
    public GameObjectType ResolveType()
    {
      return ResolveType(Type);
    }

    public static GameObjectType ResolveType(GameObjectType type)
    {
      if(type >= GameObjectType.Reserved && type < GameObjectType.Castle)
        return GameObjectType.Reserved;

      if (type >= GameObjectType.Castle && type < GameObjectType.Whizzbang)
        return GameObjectType.Castle;

      if (type >= GameObjectType.Whizzbang && type < GameObjectType.Explosion)
        return GameObjectType.Whizzbang;
      
      if (type >= GameObjectType.Explosion && type < GameObjectType.Tower)
        return GameObjectType.Explosion;

      if (type >= GameObjectType.Tower && type < GameObjectType.Unit)
        return GameObjectType.Tower;

      if (type >= GameObjectType.Unit)
        return GameObjectType.Unit;

      return GameObjectType.Undefined;
    }

    public virtual object Clone()
    {
      return new Tower
      {
        GameId = GameId,
        Position = Position,
        Type = Type,
        WaitTicks = WaitTicks,
        Effect = new SpecialEffect{Id = Effect.Id, Duration = Effect.Duration}
      };
    }
    
    #region Equals

    public bool Equals(GameObjectLogical other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return GameId == other.GameId;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((GameObjectLogical) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return base.GetHashCode();
      }
    }

    #endregion
  }
}
