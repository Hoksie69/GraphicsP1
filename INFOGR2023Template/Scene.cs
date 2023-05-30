using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace INFOGR2023Template
{
    internal class Scene
    {
        public List<Primitive> primitivesList = new List<Primitive>();
        public List<Light> lightsList = new List<Light>();

        public Scene()
        {
            primitivesList.Add(new Sphere(new Vector3(1, 0, 0), 1));
            //lightsList.Add(new Light(new Vector3(1, 1, 0), new Vector3(256, 256, 256)));
        }

        public bool Intersection(Vector3 ray)
        {
            foreach(Primitive primitive in primitivesList)
            {
                if(primitive is Sphere)
                {
                    return true;
                }

                if(primitive is Plane)
                {
                    return true;
                }
                
            }
            return false;
        }
    }
}
