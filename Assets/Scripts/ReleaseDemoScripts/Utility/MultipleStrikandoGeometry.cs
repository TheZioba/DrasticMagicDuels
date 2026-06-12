using System;
using UnityEngine;

namespace MathUtils
{
    public class MultipleStrikandoGeometry
    {
        public const float PHI = Mathf.PI / 6f;

        public static float Theta(Vector2 viewfinder, Vector2 castpoint)
        {
            return Mathf.Atan2(viewfinder.y - castpoint.y, viewfinder.x - castpoint.x);
        }

        public static float CalculateRadius(Vector2 viewfinder, Vector2 castpoint)
        {
            return (viewfinder - castpoint).magnitude / (2 * Mathf.Sin(PHI));
        }
    }
}
