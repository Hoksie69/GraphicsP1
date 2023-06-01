using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace INFOGR2023Template
{
    internal class Intersection
    {
        public float distance;
        public Primitive victim;
        public Vector3 normal;
        public Vector3 intersectionPoint;

        public Intersection(float _distance, Primitive _victim, Vector3 _normal, Vector3 _intersectionPoint)
        {
            distance = _distance;
            victim = _victim;
            normal = _normal.Normalized();
            intersectionPoint = _intersectionPoint;
        }
    }
}
