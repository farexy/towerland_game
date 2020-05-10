using Assets.Scripts.Models.GameObjects;

namespace Assets.Scripts.Models.Stats
{
    public interface IStats
    {
        GameObjectType Type { get; set; }
        int Cost { get; set; }
        bool Hidden { get; set; }
    }
}