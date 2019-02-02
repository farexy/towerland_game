using System;
using System.Text;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameObjects;

namespace Helpers
{
  public class SkillTextBuilder
  {
    public string BuildSkillText(SkillId skillId, GameObjectType goType)
    {
      if (skillId == SkillId.None)
      {
        return string.Empty;
      }

      var skill = LocalStorage.StatsLibrary.GetSkill(skillId, goType);
      var sb = new StringBuilder();

      if (skill.ProbabilityPercent > 0)
      {
        sb.Append($"{skill.ProbabilityPercent}% chance to");
      }
      switch (skillId)
      {
        case SkillId.None:
          break;
        case SkillId.FreezesUnit:
          sb.Append("Freezes unit on attack.");
          sb.Append($" When freezed unit speed decreases by {skill.DebuffValue} times");
          break;
        case SkillId.PoisonsUnit:
          sb.Append("Poisons unit on attack.");
          sb.Append($" When poisoned unit loses {skill.DebuffValue * 100}% of health on each step");
          break;
        case SkillId.ExtraDamageUnit:
          sb.Append($" make {skill.BuffValue}x extra damage");
          break;
        case SkillId.ShurikenAttack:
          sb.Append("Attacks units at the neighbour positions if any");
          break;
        case SkillId.BlocksUnitSkills:
          sb.Append("Blocks unit skills on attack");
          break;
        case SkillId.BlocksUnitSkillsInRange:
          sb.Append("Blocks unit skills");
          break;
        case SkillId.RevivesDeadUnit:
          sb.Append("While on field revives each dead unit as a skeleton");
          break;
        case SkillId.StealsTowerMoney:
          sb.Append($" steal {skill.DebuffValue} money from Tower player and give it to Monsters player");
          break;
        case SkillId.DestroysTowerOnDeath:
          sb.Append("When unit dies destroys tower that killed him");
          break;
        case SkillId.BlocksTowerSkills:
        case SkillId.BlocksTowerSkillsInRange:
          sb.Append("Blocks tower skills");
          break;
        case SkillId.AirUnit:
          sb.Append("Air unit");
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      if (skill.Range > 0)
      {
        sb.Append($" in range {skill.Range}");
      }
      if (skill.Duration > 0)
      {
        sb.Append($" for {skill.Duration} seconds");
      }

      return sb.ToString();
    }
  }
}