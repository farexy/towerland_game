using Assets.Scripts.Models.GameActions;

namespace Assets.Scripts.Models.Resolvers
{
    public interface IActionResolver
    {
        void Resolve(GameAction action);
    }
}