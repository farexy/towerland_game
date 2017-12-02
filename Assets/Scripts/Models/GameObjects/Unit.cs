
namespace Assets.Scripts.Models.GameObjects
{
  public class Unit : GameObjectLogical
  {
    public Unit()
    {
      Type = GameObjectType.Unit;
    }

    public int Health { set; get; }
    public int? PathId { set; get; }
  }
}
