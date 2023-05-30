﻿using System;
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
            primitivesList.Add(new Sphere(new Vector3(5, 0, 2), new Vector3(0,0,255), 1));
            primitivesList.Add(new Sphere(new Vector3(10, 0, 1), new Vector3(0, 255, 255), 1));
            primitivesList.Add(new Sphere(new Vector3(5, 0, -2), new Vector3(255, 0, 255), 1));
            primitivesList.Add(new Sphere(new Vector3(10, 0, -1), new Vector3(0, 255, 0), 1));
            primitivesList.Add(new Plane(new Vector3(0, 0, 0), new Vector3(100, 100, 100), 10, new Vector3(0, 1, 0)));
            //lightsList.Add(new Light(new Vector3(1, 1, 0), new Vector3(256, 256, 256)));
        }

        public Intersection SceneIntersection(Vector3 origin, Vector3 direction)
        {
            foreach(Primitive primitive in primitivesList)
            {
                if(primitive is Sphere)
                {
                    float abcA = (direction.X * direction.X + direction.Y * direction.Y + direction.Z * direction.Z);
                    float abcB = (2 * origin.X * direction.X + 2 * origin.Y * direction.Y + 2 * origin.Z * direction.Z - 2 * direction.X * primitive.position.X - 2 * direction.Y * primitive.position.Y - 2 * direction.Z * primitive.position.Z);
                    float abcC = (origin.X * origin.X + origin.Y * origin.Y + origin.Z * origin.Z + primitive.position.X * primitive.position.X + primitive.position.Y * primitive.position.Y + primitive.position.Z * primitive.position.Z - 2 * origin.X * primitive.position.X - 2 * origin.Y * primitive.position.Y - 2 * origin.Z * primitive.position.Z - primitive.radius * primitive.radius); 
                    float abcD = (float)(abcB * abcB - 4 * abcA * abcC);
                    if (abcA == 0)
                        return null;
                    if (abcD > 0)
                    {
                        float t1 = (float)(-abcB + Math.Sqrt(abcD)) / (2 * abcA);
                        float t2 = (float)(-abcB - Math.Sqrt(abcD)) / (2 * abcA);

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
                if(primitive is Plane)
                {
                    float A = primitive.normal.X * origin.X + primitive.normal.Y * origin.Y + primitive.normal.Z * origin.Z;
                    float B = primitive.normal.X * direction.X + primitive.normal.Y * direction.Y + primitive.normal.Z * direction.Z;
                    if(B != 0 && B < 0)
                    {
                        float t = (-A) / B;
                        Vector3 normal = (origin + t * direction) - primitive.position;
                        return new Intersection(((origin + t * direction).Length), primitive, normal);
                    }
                }   
            }

            return null;
        }
    }
}
