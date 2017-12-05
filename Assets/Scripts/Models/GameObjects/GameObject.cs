using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameField;

namespace Assets.Scripts.Models.GameObjects
{
  public abstract class GameObjectLogical
  {
    public int GameId { set; get; }
    public Point Position { get; set; }
    public int WaitTicks { set; get; }
    public SpecialEffect Effect { set; get; }
    public GameObjectType Type { set; get; }

    protected GameObjectLogical()
    {
      Effect = SpecialEffect.Empty;
    }
    
    public GameObjectType ResolveType()
    {
      return ResolveType(Type);
    }

    public static GameObjectType ResolveType(GameObjectType type)
    {
      if(type >= GameObjectType.Reserved && type < GameObjectType.Castle)
        return GameObjectType.Reserved;

      if (type >= GameObjectType.Castle && type < GameObjectType.Tower)
        return GameObjectType.Castle;

      if (type >= GameObjectType.Tower && type < GameObjectType.Unit)
        return GameObjectType.Tower;

      if (type >= GameObjectType.Unit)
        return GameObjectType.Unit;

      return GameObjectType.Undefined;
    }
  }
}
