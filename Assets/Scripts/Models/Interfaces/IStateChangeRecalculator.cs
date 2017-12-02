using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.GameObjects;

namespace Assets.Scripts.Models.Interfaces
{
  public interface IStateChangeRecalculator
  {
    void AddNewUnit(Field field, GameObjectType type, CreationOptions? opt);
    void AddNewTower(Field field, GameObjectType type, CreationOptions? opt);
  }
}
