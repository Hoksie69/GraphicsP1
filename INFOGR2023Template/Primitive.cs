using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2023Template
{
    internal class Primitive
    {
        public Vector3 position;
        public float radius;
        public Vector3 normal;
        public Vector3 color;

        public Primitive(Vector3 _position) 
        {
            position = _position;
        }


    }

    internal class Sphere : Primitive
    {
        public Sphere(Vector3 _position, float _radius) : base(_position)
        {
            radius = _radius;
        }
    }

    internal class Plane : Primitive
    {
        Vector3 direction;
        public Plane(Vector3 _position, Vector3 _direction, Vector3 _normal) : base(_position)
        {
            normal = _normal;
            direction = _direction; 
        }
    }
}
