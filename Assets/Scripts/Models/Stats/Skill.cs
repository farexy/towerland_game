using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameObjects;

namespace Assets.Scripts.Models.Stats
{
  public struct Skill
  {
    public SkillId Id;
    public GameObjectType GameObjectType;
    public int Duration;
    public int ProbabilityPercent;
    public EffectId EffectId;
    public double BuffValue;
    public double DebuffValue;
    public int WaitTicks;
    public int Range;
  }
}