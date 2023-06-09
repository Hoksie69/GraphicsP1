﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
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
        public static float epsilon;


        public Scene()
        {
            epsilon = .001f;

            //Plane
            testVector = new Vector3(0, 1, 0);
            testVector.Normalize();
            primitivesList.Add(new Plane(new Vector3(3, -1, 0), new Vector3(0.7f, 0.7f, 0.7f), new Vector3(0, 0, 0), testVector, true));

            //Spheres
            primitivesList.Add(new Sphere(new Vector3(5, 1, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0), 1, 100));
            primitivesList.Add(new Sphere(new Vector3(5, 1, 2), new Vector3(0, 1, 0), new Vector3(1, 1, 1), 1));
            primitivesList.Add(new Sphere(new Vector3(5, 1, -2), new Vector3(1, 0, 1), new Vector3(0, 0, 0), 1));
            primitivesList.Add(new Sphere(new Vector3(7, 3, -1), new Vector3(0, 0.5f, 1), new Vector3(0, 0, 0), 1, 0, true));

            //Lights
            lightsList.Add(new Light(new Vector3(1, 3, 1), new Vector3(10, 10, 10)));
            lightsList.Add(new Light(new Vector3(2, 1, -1), new Vector3(5, 5, 5)));


            //primitivesList.Add(new Sphere(new Vector3(10, 3, 1), new Vector3(0, 0.5f, 1), new Vector3(1, 1, 1), 1));
            //primitivesList.Add(new Sphere(new Vector3(5, 3, 2), new Vector3(0, 0, 0.5f), new Vector3(1, 1, 1), 1));
            //primitivesList.Add(new Sphere(new Vector3(7, 4, 1), new Vector3(0.5f, 1, 0.5f), new Vector3(1, 1, 1), 1));
            //primitivesList.Add(new Sphere(new Vector3(5, 4, -2), new Vector3(1, 0, 1), new Vector3(0, 0, 0), 1, 0, true));
            //primitivesList.Add(new Sphere(new Vector3(10, 0, -1), new Vector3(0, 1, 0), new Vector3(0, 0, 0), 1));
            //lightsList.Add(new Light(new Vector3(6, 2, 0), new Vector3(5, 5, 5)));
            //lightsList.Add(new Light(new Vector3(0, 6, 0), new Vector3(20, 20, 20)));
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

                        if(t1 > epsilon || t2 > epsilon)
                        {
                            if (t1 != t2)
                            {
                                if ((t1 * direction).Length < (t2 * direction).Length)
                                {
                                     Vector3 normal = (origin + t1 * direction) - primitive.position;
                                     intersections.Add(new Intersection(((t1 * direction).Length), primitive, normal, origin + t1 * direction));
                                }
                                else
                                {
                                     Vector3 normal = (origin + t2 * direction) - primitive.position;
                                     intersections.Add(new Intersection(((t2 * direction).Length), primitive, normal, origin + t2 * direction));
                                } 
                            }
                            else
                            {
                                Vector3 normal = (origin + t1 * direction) - primitive.position;
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

                        Vector3 normal = ((t * direction) - primitive.position);
                        intersections.Add(new Intersection(((t * direction).Length), primitive, normal, origin + t * direction));
                    }
                }
            }
            return intersections.MinBy(Intersection => Intersection.distance);
        }

        public bool ShadowIntersection(Vector3 origin, Vector3 direction, Light light)
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

                        if ((t1 > epsilon && t1 < (light.location - (origin + direction)).Length) || (t2 > epsilon && t2 < (light.location - (origin + direction)).Length))
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
                        if (t > epsilon && t < (light.location - (origin + direction)).Length - epsilon)
                            return true;
                    }
                }
            }
            return false;
        }
    }
}

