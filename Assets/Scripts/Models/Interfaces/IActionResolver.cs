using Assets.Scripts.Models.GameActions;

namespace Assets.Scripts.Models.Interfaces
{
  public interface IActionResolver
  {
    void Resolve(GameAction action);
  }
}
