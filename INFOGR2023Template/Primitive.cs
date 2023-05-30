using OpenTK.Mathematics;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
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

        public Primitive(Vector3 _position, Vector3 _color) 
        {
            position = _position;
            color = _color;
        }
    }

    internal class Sphere : Primitive
    {
        public Sphere(Vector3 _position, Vector3 _color, float _radius) : base(_position, _color)
        {
            radius = _radius;
        }
    }

    internal class Plane : Primitive
    {
        float direction;
        public Plane(Vector3 _position, Vector3 _color, float _direction, Vector3 _normal) : base(_position, _color)
        {
            normal = _normal;
            direction = _direction; 
        }
    }
}
