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
        Vector3 distance;
        Primitive victim;
        Vector3 normal;

        public Intersection(Vector3 _distance, Primitive _victim, Vector3 _normal)
        {
            distance = _distance;
            victim = _victim;
            normal = _normal;
        }
    }
}
