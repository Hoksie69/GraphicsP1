using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

//Goeie gedachte (volgens 

namespace INFOGR2023Template
{
    internal class Scene
    {
        public List<Primitive> primitivesList = new List<Primitive>();
        public List<Light> lightsList = new List<Light>();
        List<Intersection> intersections;
        Vector3 testVector;


        public Scene()
        {
            primitivesList.Add(new Sphere(new Vector3(10, 3, 1), new Vector3(0, 0.5f, 1), new Vector3(1, 1, 1), 1));
            primitivesList.Add(new Sphere(new Vector3(5, 2, 2), new Vector3(0, 0, 0.5f), new Vector3(1, 1, 1), 1));
            primitivesList.Add(new Sphere(new Vector3(7, 2, 1), new Vector3(0.5f, 1, 0.5f), new Vector3(1, 1, 1), 1));
            primitivesList.Add(new Sphere(new Vector3(5, 0, -2), new Vector3(1, 0, 1), new Vector3(1, 1, 1), 1));
            primitivesList.Add(new Sphere(new Vector3(10, 0, -1), new Vector3(0, 1, 0), new Vector3(1, 1, 1), 1));
            testVector = new Vector3(0, 1, 0);
            testVector.Normalize();
            primitivesList.Add(new Plane(new Vector3(3, -1, 0), new Vector3(0.3f, 0.3f, 0.3f), new Vector3(5, 5, 0), testVector));
            lightsList.Add(new Light(new Vector3(5, 4, 0), new Vector3(5, 5, 5)));
        }

        public Intersection SceneIntersection(Vector3 origin, Vector3 direction)
        {
            intersections = new List<Intersection>();
            intersections.Clear();

            foreach (Primitive primitive in primitivesList)
            {
                if (primitive is Sphere)
                {
                    float abcA = (direction.X * direction.X + direction.Y * direction.Y + direction.Z * direction.Z);
                    float abcB = (2 * origin.X * direction.X + 2 * origin.Y * direction.Y + 2 * origin.Z * direction.Z - 2 * direction.X * primitive.position.X - 2 * direction.Y * primitive.position.Y - 2 * direction.Z * primitive.position.Z);
                    float abcC = (origin.X * origin.X + origin.Y * origin.Y + origin.Z * origin.Z + primitive.position.X * primitive.position.X + primitive.position.Y * primitive.position.Y + primitive.position.Z * primitive.position.Z - 2 * origin.X * primitive.position.X - 2 * origin.Y * primitive.position.Y - 2 * origin.Z * primitive.position.Z - primitive.radius * primitive.radius);
                    float abcD = (float)(abcB * abcB - 4 * abcA * abcC);
                    if (abcA == 0)
                        return null;
                    if (abcD >= 0)
                    {
                        float t1 = (float)(-abcB + Math.Sqrt(abcD)) / (2 * abcA);
                        float t2 = (float)(-abcB - Math.Sqrt(abcD)) / (2 * abcA);

                        if(t1 > 0 || t2 > 0)
                        {
                            if (t1 != t2)
                            {
                               if ((origin + t1 * direction).Length < (origin + t2 * direction).Length)
                               {
                                    Vector3 normal = (t1 * direction) - primitive.position;
                                    intersections.Add(new Intersection(((t1 * direction).Length), primitive, normal, origin + t1 * direction));
                               }
                               else
                               {
                                    Vector3 normal = (t2 * direction) - primitive.position;
                                    intersections.Add(new Intersection(((t2 * direction).Length), primitive, normal, origin + t2 * direction));
                               } 
                            }
                            else
                            {
                                Vector3 normal = (t1 * direction) - primitive.position;
                                intersections.Add(new Intersection(((t1 * direction).Length), primitive, normal, origin + t1 * direction));
                            }
                        }

                    }
                }
                if (primitive is Plane)
                {
                    float B = primitive.normal.X * origin.X + primitive.normal.Y * origin.Y + primitive.normal.Z * origin.Z - primitive.position.X * primitive.normal.X - primitive.position.Y * primitive.normal.Y - primitive.position.Z * primitive.normal.Z;
                    float A = primitive.normal.X * direction.X + primitive.normal.Y * direction.Y + primitive.normal.Z * direction.Z;
                    if (A != 0 && A < 0)
                    {
                        float t = (-B) / A;

                        Vector3 normal = ((origin + t * direction) - primitive.position);
                        intersections.Add(new Intersection(((t * direction).Length), primitive, normal, origin + t * direction));
                    }
                }
            }
            return intersections.MinBy(test => test.distance);
        }

        public bool ShadowIntersection(Vector3 origin, Vector3 direction)
        {
            foreach (Primitive primitive in primitivesList)
            {
                if (primitive is Sphere)
                {
                    float abcA = (direction.X * direction.X + direction.Y * direction.Y + direction.Z * direction.Z);
                    float abcB = (2 * origin.X * direction.X + 2 * origin.Y * direction.Y + 2 * origin.Z * direction.Z - 2 * direction.X * primitive.position.X - 2 * direction.Y * primitive.position.Y - 2 * direction.Z * primitive.position.Z);
                    float abcC = (origin.X * origin.X + origin.Y * origin.Y + origin.Z * origin.Z + primitive.position.X * primitive.position.X + primitive.position.Y * primitive.position.Y + primitive.position.Z * primitive.position.Z - 2 * origin.X * primitive.position.X - 2 * origin.Y * primitive.position.Y - 2 * origin.Z * primitive.position.Z - primitive.radius * primitive.radius);
                    float abcD = (float)(abcB * abcB - 4 * abcA * abcC);
                    //Console.WriteLine(abcD); 
                    if (abcD >= 0)
                    {
                        float t1 = (float)(-abcB + Math.Sqrt(abcD)) / (2 * abcA);
                        float t2 = (float)(-abcB - Math.Sqrt(abcD)) / (2 * abcA);

                        if ((t1 > 0.0001f && t1 < (direction * t1 - origin).Length - 0.0001f)|| (t2 > 0.0001f && t2 < (direction * t2 - origin).Length))
                            return true;
                    }
                }

                if (primitive is Plane)
                {   
                    float B = primitive.normal.X * origin.X + primitive.normal.Y * origin.Y + primitive.normal.Z * origin.Z - primitive.position.X * primitive.normal.X - primitive.position.Y * primitive.normal.Y - primitive.position.Z * primitive.normal.Z;
                    float A = primitive.normal.X * direction.X + primitive.normal.Y * direction.Y + primitive.normal.Z * direction.Z;
                    if (A != 0 && A < 0)
                    {
                        float t = (-B) / A;
                        if (t > 0.001f && t < (direction * t - origin).Length - 0.001f)
                            return true;
                    }
                }
            }
            return false;
        }
    }
}

