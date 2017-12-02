using Assets.Scripts.Models.GameField;

namespace Assets.Scripts.Models.Interfaces
{
  public interface IFieldFactory
  {
    Field ClassicField { get; }
    Field GenerateNewField(int width, int height, Point startPoint, Point endPoint);
  }
}
