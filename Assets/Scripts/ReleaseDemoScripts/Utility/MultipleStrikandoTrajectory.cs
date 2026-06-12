using System;
using UnityEngine;
using static MathUtils.MultipleStrikandoGeometry;

namespace MathUtils
{
    public class MultipleStrikandoTrajectory
    {
        private static float L(Vector2 viewfinder, Vector2 castpoint)
        {
            return (viewfinder - castpoint).magnitude;
        }

        private static Vector2 ParallelVersor(Vector2 viewfinder, Vector2 castpoint)
        {
            return (viewfinder - castpoint).normalized;
        }

        private static Vector2 PerpendicularVersor(Vector2 viewfinder, Vector2 castpoint)
        {
            Vector2 tangentVersor = ParallelVersor(viewfinder, castpoint);
            return new Vector2(-tangentVersor.y, tangentVersor.x);
        }

        private static float PerpendicularComponent(float L, float R, float sign, float s)
        {
            float halfL = L / 2f;
            float d = (float)Math.Sqrt(R * R - halfL * halfL);

            return sign * (Mathf.Sqrt(R * R - (s - halfL) * (s - halfL)) - d);
        }

        private static float DerivativeOfPerpendicularComponent(float L, float R, float sign, float s)
        {
            return sign * ((s - L / 2) / Mathf.Sqrt(R * R - (s - L / 2) * (s - L / 2)));
        }

        public static Vector2 CurveParameterizationAlongCV(Vector2 viewfinder, Vector2 castpoint, float signOfPerpendicularComponent, float s)
        {
            float CVMagnitude = L(viewfinder, castpoint);
            float R = CalculateRadius(viewfinder, castpoint);
            Vector2 tangentVersor = ParallelVersor(viewfinder, castpoint);
            Vector2 perpendicularVersor = PerpendicularVersor(viewfinder, castpoint);

            return castpoint + s * tangentVersor + PerpendicularComponent(CVMagnitude, R, signOfPerpendicularComponent, s) * perpendicularVersor;
        }

        public static Vector2 TangentVersorAlongCurve(Vector2 viewfinder, Vector2 castpoint, float signOfDerivativeOfPerpendicularComponent, float v, float s)
        {
            float CVMagnitude = L(viewfinder, castpoint);
            float R = CalculateRadius(viewfinder, castpoint);
            Vector2 parallelVersor = ParallelVersor(viewfinder, castpoint);
            Vector2 perpendicularVersor = PerpendicularVersor(viewfinder, castpoint);
            return (v * (parallelVersor + DerivativeOfPerpendicularComponent(CVMagnitude, R, signOfDerivativeOfPerpendicularComponent, s) * perpendicularVersor)).normalized;
        }
    }
}
