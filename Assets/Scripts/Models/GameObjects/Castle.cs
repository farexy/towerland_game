namespace Assets.Scripts.Models.GameObjects
{
  public class Castle : GameObjectLogical
  {
    public Castle()
    {
      Type = GameObjectType.Castle;
    }

    public int Health { set; get; }
  }
}
