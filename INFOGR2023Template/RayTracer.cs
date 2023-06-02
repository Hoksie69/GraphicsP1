using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using OpenTK.Platform.Windows;
using System.Diagnostics.CodeAnalysis;

namespace INFOGR2023Template
{
    internal class RayTracer
    {        
        public Camera camera = new Camera(new Vector3(0,0,0), new Vector3(1,0,0), new Vector3(0,1,0));
        public Scene scene = new Scene();
        Surface screen;
        public float ambientLight = 0.1f;
        public Vector3 backgroundColor = new Vector3(0.8f, 0.8f, 0.8f);

        public Vector3[] CamPlane { get { return camera.screenPlane; } }
        
        public RayTracer(Surface _screen)
        {
            screen = _screen;
        } 

        public void Render()
        {
            for(float x = 0; x < screen.width; x++)
            {
                for (float y = 0; y < screen.height; y++)
                {
                    Vector3 pointOnPlane = camera.screenPlane[0] + (x / screen.width) * camera.cameraPlaneBasisU + (y / screen.height) * camera.cameraPlaneBasisV;
                    Vector3 rayDirection = pointOnPlane - camera.position;
                    rayDirection.Normalize();

                    Intersection tempIntersection = scene.SceneIntersection(camera.position, rayDirection);
                    if(tempIntersection != null) 
                    {
                        Vector3 tempColor;
                        if(tempIntersection.victim.specularity == 100)
                        {
                            tempIntersection.normal.Normalize();
                            Vector3 reflectedRay = rayDirection - 2 * (Vector3.Dot(rayDirection, tempIntersection.normal) * tempIntersection.normal);
                            reflectedRay.Normalize();
                            
                            Intersection reflectedIntersection = scene.SceneIntersection(tempIntersection.intersectionPoint, reflectedRay);

                            if (reflectedIntersection != null && tempIntersection.victim != reflectedIntersection.victim)
                            {
                                tempColor = tempIntersection.victim.color * GetShadow(reflectedIntersection.victim, reflectedIntersection);

                                if (x % 10 == 0 && tempIntersection.intersectionPoint.Y == camera.position.Y)
                                {
                                    Debug.secondaryRay.Add((new Vector2(tempIntersection.intersectionPoint.X, tempIntersection.intersectionPoint.Z), new Vector2(reflectedIntersection.victim.position.X, reflectedIntersection.victim.position.Z)));
                                }
                            }
                            else
                                tempColor = backgroundColor;
                        }
                        else
                            tempColor = GetShadow(tempIntersection.victim, tempIntersection);
                        screen.Plot((int)x, (int)y, GetColor(tempColor.X, tempColor.Y, tempColor.Z));
                        if (y == 200 && x % 10 == 0)
                        {
                            Debug.rayList.Add((new Vector2(camera.position.X, camera.position.Z), new Vector2(tempIntersection.intersectionPoint.X, tempIntersection.intersectionPoint.Z)));
                        }
                        foreach (Light l in scene.lightsList)
                            if(x % 10 == 0 && tempIntersection.victim is Sphere && y == 200 && tempIntersection.intersectionPoint.Y == camera.position.Y)
                                Debug.shadowRay.Add((new Vector2(tempIntersection.intersectionPoint.X, tempIntersection.intersectionPoint.Z), new Vector2(l.location.X, l.location.Z)));
                    }
                    else
                    {
                        screen.Plot((int)x, (int)y, GetColor(backgroundColor.X, backgroundColor.Y, backgroundColor.Z));
                        if(y == 200 && x % 10 == 0)
                            Debug.rayList.Add((new Vector2(camera.position.X, camera.position.Z), new Vector2(camera.position.X + rayDirection.X * 20, camera.position.Z + rayDirection.Z * 20)));

                    }
                }
            }
        }
        
        public Vector3 GetShadow(Primitive victim, Intersection _intersection)
        {
            Vector3 pixelColor = new Vector3();
            foreach (Light l in scene.lightsList)
            {
                Vector3 shadowRayDirection = l.location - _intersection.intersectionPoint;
                shadowRayDirection.Normalize();
                Vector3 tempColor = new Vector3();

                if (!scene.ShadowIntersection(_intersection.intersectionPoint, shadowRayDirection,l))
                {
                    float dot = Math.Max(0, Vector3.Dot(_intersection.normal, shadowRayDirection));
                    Vector3 R = shadowRayDirection + 2 * Vector3.Dot(shadowRayDirection, _intersection.normal) * _intersection.normal;
                    R.Normalize();
                    Vector3 V = _intersection.intersectionPoint - camera.position;  
                    V.Normalize();
                    float dot2 = Math.Max(0, Vector3.Dot(V, R));

                    if(victim is Sphere)
                    {
                        tempColor.X = l.intensity.X * (float)(1 / Math.Pow((float)(l.location - _intersection.intersectionPoint).Length, 2) * (dot * victim.color.X) + victim.highlightColor.X * (float)Math.Pow(dot2, 2) * ambientLight);
                        tempColor.Y = l.intensity.Y * (float)(1 / Math.Pow((float)(l.location - _intersection.intersectionPoint).Length, 2) * (dot * victim.color.Y) + victim.highlightColor.Y * (float)Math.Pow(dot2, 2) * ambientLight);
                        tempColor.Z = l.intensity.Z * (float)(1 / Math.Pow((float)(l.location - _intersection.intersectionPoint).Length, 2) * (dot * victim.color.Z) + victim.highlightColor.Z * (float)Math.Pow(dot2, 2) * ambientLight);
                    }
                    if (victim is Plane)
                    {
                        Vector3 intersectionVector = _intersection.intersectionPoint;
                        float dotU = Vector3.Dot(intersectionVector, new Vector3(1, 0, 0));
                        float dotV = Vector3.Dot(intersectionVector, new Vector3(0, 0, 1));

                        float u = dotU;
                        float v = dotV;
                        Vector3 tempCheckerboard = Plane.CheckerboardPattern(u, v);
                        tempColor.X += tempCheckerboard.X * l.intensity.X * (float)(1 / Math.Pow((float)(l.location - _intersection.intersectionPoint).Length, 2) * victim.color.X + victim.highlightColor.X * (float)Math.Pow(dot2, 2));
                        tempColor.Y += tempCheckerboard.Y * l.intensity.Y * (float)(1 / Math.Pow((float)(l.location - _intersection.intersectionPoint).Length, 2) * victim.color.Y + victim.highlightColor.Y * (float)Math.Pow(dot2, 2));
                        tempColor.Z += tempCheckerboard.Z * l.intensity.Z * (float)(1 / Math.Pow((float)(l.location - _intersection.intersectionPoint).Length, 2) * victim.color.Z + victim.highlightColor.Z * (float)Math.Pow(dot2, 2));
                    }

                    if (tempColor.X > 1)
                        tempColor.X = 1;
                    if (tempColor.Y > 1)
                        tempColor.Y = 1;
                    if (tempColor.Z > 1)
                        tempColor.Z = 1;

                    pixelColor += tempColor;
                }
            }
            return pixelColor + new Vector3(Math.Clamp(pixelColor.X + victim.color.X * ambientLight, 0, 1), Math.Clamp(pixelColor.Y + victim.color.Y * ambientLight, 0, 1), Math.Clamp(pixelColor.Z + victim.color.Z * ambientLight, 0, 1));
        }

        public int GetColor(float R, float G, float B)
        {

            int R2 = (int)(R * 255);
            int G2 = (int)(G * 255);
            int B2 = (int)(B * 255);
            return (R2 << 16) + (G2 <<8) + B2;
        }
    }
}