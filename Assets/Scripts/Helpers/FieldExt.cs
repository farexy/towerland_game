using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.GameObjects;

namespace Helpers
{
    public static class FieldExt
    {
        public static void MoveUnit(this Field field, int gameId, Point position, int wait)
        {
            var unit = field[gameId];
            unit.Position = position;
            unit.WaitTicks = wait;
        }
    }
}