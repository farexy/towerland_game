using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.GameObjects;

namespace Assets.Scripts.Models.Interfaces
{
  public interface IPathOptimiser
  {
    int GetFastestPath(Path[] paths, Unit unit);
    int GetOptimalPath(Path[] paths, Unit unit);
  }
}
