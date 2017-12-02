using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.GameObjects;

namespace Helpers
{
    public static class FieldExt
    {
        public static void MoveUnit(this Field field, int gameId, Point position, int wait)
        {
            var unit = (Unit) field[gameId];
            var path = field.StaticData.Path[unit.PathId.Value];
            unit.Position = position;
            unit.WaitTicks = wait;
        }
    }
}