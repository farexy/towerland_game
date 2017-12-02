using Assets.Scripts.Models.GameField;

namespace Assets.Scripts.Models.GameActions
{
  public struct GameAction
  {
    public ActionId ActionId;
    public int UnitId;
    public int TowerId;
    public Point Position;
    public int Damage;
    public int WaitTicks;
    public int Money;
  }
}
