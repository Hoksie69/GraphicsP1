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
                        Vector3 tempColor = GetShadow(tempIntersection.victim, tempIntersection);
                        screen.Plot((int)x, (int)y, GetColor(tempColor.X, tempColor.Y, tempColor.Z));
                        if (y == 200 && x % 10 == 0)
                        {
                            Debug.rayList.Add((new Vector2(camera.position.X, camera.position.Z), new Vector2(tempIntersection.intersectionPoint.X, tempIntersection.intersectionPoint.Z)));
                        }
                        foreach (Light l in scene.lightsList)
                            if(x % 10 == 0 && tempIntersection.victim is Sphere && y == 200 && tempIntersection.victim.position.Y == camera.position.Y)
                                Debug.shadowRay.Add((new Vector2(tempIntersection.intersectionPoint.X, tempIntersection.intersectionPoint.Z), new Vector2(l.location.X, l.location.Z)));
                    }
                    else
                    {
                        screen.Plot((int)x, (int)y, GetColor(0.8f,0.8f,0.8f));
                        if(y == 200 && x % 10 == 0)
                            Debug.rayList.Add((new Vector2(camera.position.X, camera.position.Z), new Vector2(camera.position.X + rayDirection.X * 20, camera.position.Z + rayDirection.Z * 20)));

                    }
                }
            }
        }

        public Vector3 GetShadow(Primitive victim, Intersection _intersection)
        {
            foreach (Light l in scene.lightsList)
            {
                Vector3 shadowRayDirection = l.location - _intersection.intersectionPoint;
                shadowRayDirection.Normalize();

                if (!scene.ShadowIntersection(_intersection.intersectionPoint, shadowRayDirection))
                {
                    Vector3 color = new Vector3();
                    float dot = Math.Max(0, Vector3.Dot(_intersection.normal, shadowRayDirection));
                    Vector3 R = -shadowRayDirection - 2 * Vector3.Dot(-shadowRayDirection, _intersection.normal) * _intersection.normal;
                    R.Normalize();
                    Vector3 V = _intersection.intersectionPoint - camera.position;  
                    V.Normalize();
                    float dot2 = Math.Max(0, Vector3.Dot(-V, R));
                    if(victim is Sphere)
                    {
                        color.X = l.intensity.X * (float)(1 / Math.Pow((float)(l.location - _intersection.intersectionPoint).Length, 2) * (dot * victim.color.X) + victim.highlightColor.X * (float)Math.Pow(dot2,250) ) + victim.color.X * 0.1f;
                        color.Y = l.intensity.Y * (float)(1 / Math.Pow((float)(l.location - _intersection.intersectionPoint).Length, 2) * (dot * victim.color.Y) + victim.highlightColor.Y * (float)Math.Pow(dot2, 250) ) + victim.color.Y * 0.1f;
                        color.Z = l.intensity.Z * (float)(1 / Math.Pow((float)(l.location - _intersection.intersectionPoint).Length, 2) * (dot * victim.color.Z) + victim.highlightColor.Z * (float)Math.Pow(dot2, 250) ) + victim.color.Z * 0.1f;
                    }
                   
                    if(victim is Plane)
                    {
                        color.X = l.intensity.X * (float)(1 / Math.Pow((float)(l.location - _intersection.intersectionPoint).Length, 2) * victim.color.X /*+ victim.highlightColor.X * (float)Math.Pow(dot2,250) */) + victim.color.X * 0.1f;
                        color.Y = l.intensity.Y * (float)(1 / Math.Pow((float)(l.location - _intersection.intersectionPoint).Length, 2) * victim.color.Y /*+ victim.highlightColor.Y * (float)Math.Pow(dot2, 250) */) + victim.color.Y * 0.1f;
                        color.Z = l.intensity.Z * (float)(1 / Math.Pow((float)(l.location - _intersection.intersectionPoint).Length, 2) * victim.color.Z/*+ victim.highlightColor.Z * (float)Math.Pow(dot2, 250) */) + victim.color.Z * 0.1f;
                    }
                    
                    if(color.X > 1)
                        color.X = 1;
                    if (color.Y > 1)
                        color.Y = 1;
                    if (color.Z > 1)
                        color.Z = 1;

                    return color;
                }
            }
            return new Vector3(victim.color.X * 0.1f, victim.color.Y * 0.1f, victim.color.Z * 0.1f);
        }

        public int GetColor(float R, float G, float B)
        {

            int R2 = (int)(R*255);
            int G2 = (int)(G * 255);
            int B2 = (int)(B * 255);
            return (R2 << 16) + (G2 <<8) + B2;
        }
    }
}