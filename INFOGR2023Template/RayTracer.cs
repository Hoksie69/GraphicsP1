using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;

namespace INFOGR2023Template
{
    internal class RayTracer
    {
        Camera camera = new Camera(new Vector3(0,0,0), new Vector3(1,0,0), new Vector3(0,1,0));
        Scene scene = new Scene();
        Surface screen;
        Vector3 cameraPlaneBasisU;
        Vector3 cameraPlaneBasisV;
        
        public RayTracer(Surface _screen)
        {
            screen = _screen;
            cameraPlaneBasisU = camera.screenPlane[1] - camera.screenPlane[0];
            cameraPlaneBasisV = camera.screenPlane[2] - camera.screenPlane[0];
        }

        public void Render()
        {
            for(float x = 0; x < screen.width; x++)
            {
                for (float y = 0; y < screen.height; y++)
                {
                    Vector3 pointOnPlane = camera.screenPlane[0] + (x / screen.width) * cameraPlaneBasisU + (y / screen.height) * cameraPlaneBasisV;
                    Vector3 rayDirection = pointOnPlane - camera.position;
                    rayDirection.Normalize();

                    Intersection tempIntersection = scene.SceneIntersection(camera.position, rayDirection);
                    if(tempIntersection != null) 
                    {
                        Vector3 tempColor = GetShadow(tempIntersection.victim, tempIntersection.intersectionPoint);
                        screen.Plot((int)x, (int)y, GetColor((int)tempColor.X, (int)tempColor.Y, (int)tempColor.Z));
                    }
                    else
                    {
                        screen.Plot((int)x, (int)y, GetColor(200,200,200));
                    }
                }
            }
        }

        public Vector3 GetShadow(Primitive victim, Vector3 intersectionPoint)
        {
            foreach (Light l in scene.lightsList)
            {

                Vector3 shadowRayDirection = l.location - intersectionPoint;
                shadowRayDirection.Normalize();

                if (!scene.ShadowIntersection(intersectionPoint, shadowRayDirection, victim))
                {
                    return victim.color;
                }   
                else return new Vector3(0, 0, 0); 
            }
            return new Vector3(0, 0, 0);
        }

        public int GetColor(int R, int G, int B)
        {
            return (R << 16) + (G <<8) + B;
        }

    }
}
