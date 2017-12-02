using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.GameObjects;

namespace Assets.Scripts.Models.State
{
  public class TowerCreationOption
  {
    public GameObjectType Type { set; get; }
    public Point Position { set; get; }
  }
}
