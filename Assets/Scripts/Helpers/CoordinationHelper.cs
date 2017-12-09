using Assets.Scripts.Models.GameField;
using UnityEngine;

namespace Helpers
{
    public class CoordinationHelper
    {
        private static int _width;
        private static int _height;
        
        public static void Init(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public static Point GetByViewCoords(int x, int y)
        {
            return new Point(x, y);
        }    

        public static Vector2 GetViewPoint(Point point)
        {
            return new Vector2(point.Y - _height / 2, point.X - _width / 2);
        }
        
        public static bool DifferentFloats(float f1, float f2)
        {
            return DifferentFloats(f1, f2, 0.3f);
        }

        public static bool EqualFloats(float f1, float f2, float comparator)
        {
            return !DifferentFloats(f1, f2, comparator);
        }
        
        public static bool DifferentFloats(float f1, float f2, float comparator)
        {
            return Mathf.Abs(f1 - f2) > comparator;
        }
    }
}