using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameObjects;

namespace Assets.Scripts.Models.Stats
{
  public struct UnitStats : IStats
  {
    public GameObjectType Type { set; get; }
    public int Health { set; get; }
    public int Damage { set; get; }
    public int Speed { set; get; } // ticks per cell
    public MovementPriorityType MovementPriority { set; get; }
    public bool IsAir { set; get; }
    public SpecialEffect[] SpecialEffects { set; get; }
    public int Cost { set; get; }
    public DefenceType Defence { set; get; }

    public enum MovementPriorityType
    {
      Fastest,
      Optimal,
      Random,
    }

    public enum DefenceType
    {
      Undefended,
      LightArmor,
      HeavyArmor
    }
  }
}
