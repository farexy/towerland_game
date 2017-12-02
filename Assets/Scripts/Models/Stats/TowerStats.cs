using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameObjects;

namespace Assets.Scripts.Models.Stats
{
  public struct TowerStats : IStats
  {
    public GameObjectType Type { set; get; }
    public int Damage { set; get; }
    public int Range { set; get; }
    public int AttackSpeed { set; get; }
    public AttackType Attack { set; get; }
    public SpecialEffect[] SpecialEffects { set; get; }
    public int Cost { set; get; }

    public enum AttackType
    {
      Usual,
      Magic,
      Burst
    }
  }
}
