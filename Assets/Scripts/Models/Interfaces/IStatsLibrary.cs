using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Models.Stats;

namespace Assets.Scripts.Models.Interfaces
{
  public interface IStatsLibrary
  {
    IStats GetStats(GameObjectType type);
    UnitStats GetUnitStats(GameObjectType type);
    TowerStats GetTowerStats(GameObjectType type);
  }
}
