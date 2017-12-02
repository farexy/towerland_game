using Assets.Scripts.Models.GameObjects;

namespace Assets.Scripts.Models.Interfaces
{
  public interface IGameObjectFactory<out T> where T : GameObjectLogical
  {
    T Create(GameObjectType type, CreationOptions? options);
  }
}
