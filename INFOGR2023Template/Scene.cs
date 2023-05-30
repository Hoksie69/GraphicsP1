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
            primitivesList.Add(new Sphere(new Vector3(-3, 0, 0), new Vector3(0,0,255), 1));
            //lightsList.Add(new Light(new Vector3(1, 1, 0), new Vector3(256, 256, 256)));
        }

        public Intersection SceneIntersection(Vector3 origin, Vector3 direction)
        {
            foreach(Primitive primitive in primitivesList)
            {
                if(primitive is Sphere)
                {
                    float abcA = (direction.X * direction.X + direction.Y * direction.Y + direction.Z * direction.Z) * (16/10);
                    float abcB = (2 * origin.X * direction.X + 2 * origin.Y * direction.Y + 2 * origin.Z * direction.Z - 2 * direction.X * primitive.position.X - 2 * direction.Y * primitive.position.Y - 2 * direction.Z * primitive.position.Z) * (16/10);
                    float abcC = (origin.X * origin.X + origin.Y * origin.Y + origin.Z * origin.Z + primitive.position.X * primitive.position.X + primitive.position.Y * primitive.position.Y + primitive.position.Z * primitive.position.Z - 2 * origin.X * primitive.position.X - 2 * origin.Y * primitive.position.Y - 2 * origin.Z * primitive.position.Z - primitive.radius * primitive.radius) * (16/10); 
                    float abcD = (float)(abcB * abcB - 4 * abcA * abcC);
                    if (abcA == 0)
                        return null;
                    if (abcD > 0)
                    {
                        float t1 = (float)(-abcB + Math.Sqrt(abcD)) / 2 * abcA;
                        float t2 = (float)(-abcB + Math.Sqrt(abcD)) / 2 * abcA;

                        if(t1 != t2)
                        {
                            if((origin + t1 * direction).Length < (origin + t2 * direction).Length)
                            {
                                Vector3 normal = (origin + t1 * direction) - primitive.position;
                                return new Intersection(((origin + t1 * direction).Length), primitive, normal);
                            }
                            else
                            {
                                Vector3 normal = (origin + t2 * direction) - primitive.position;
                                return new Intersection(((origin + t2 * direction).Length), primitive, normal);
                            }                            
                        }
                        else
                        {
                            Vector3 normal = (origin + t1 * direction) - primitive.position;
                            return new Intersection(((origin + t1 * direction).Length), primitive, normal);
                        }
                        
                    }
                }
                //if(primitive is Plane)                   
            }

            return null;
        }
    }
}
