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
        Vector3 position = new Vector3();
        Vector3 direction = new Vector3();

        public Primitive(Vector3 _position, Vector3 _direction) 
        {
            position = _position;
            direction = _direction;
        }

        public void Sphere()
        {

        }

        public void Plane()
        {

        }
    }
}
