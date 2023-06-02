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
        public Vector3 highlightColor;
        public int specularity;
        public bool checkerboardPattern;

        public Primitive(Vector3 _position, Vector3 _color) 
        {
            position = _position;
            color = _color;
        }
    }

    internal class Sphere : Primitive
    {
        public Sphere(Vector3 _position, Vector3 _color, Vector3 _highlightColor, float _radius, int _specularity = 0, bool _checkerboardPattern = false) : base(_position, _color)
        {
            radius = _radius;
            highlightColor = _highlightColor;
            specularity = _specularity;
            checkerboardPattern = _checkerboardPattern;
        }

        public static Vector3 CheckerboardPatternSphere(Vector3 _intersectionVector)
        {
            return new Vector3();
        }
    }

    internal class Plane : Primitive
    {
        public Plane(Vector3 _position, Vector3 _color, Vector3 _highlightColor, Vector3 _normal, bool _checkerboardPattern = false) : base(_position, _color)
        {
            normal = _normal;
            highlightColor = _highlightColor;
            checkerboardPattern = _checkerboardPattern;
        }

        public static Vector3 CheckerboardPatternPlane(Vector3 _intersectionVector)
        {
            float u = Vector3.Dot(_intersectionVector, new Vector3(1, 0, 0));
            float v = Vector3.Dot(_intersectionVector, new Vector3(0, 0, 1));
            int checkColor = ((int)u + (int)v) & 1;
            if (checkColor == 0)
                return new Vector3(0, 0, 0);
            else
                return new Vector3(1, 1, 1);
        }
    }
}
