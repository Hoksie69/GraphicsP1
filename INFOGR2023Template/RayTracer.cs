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
                        Vector3 tempColor = GetShadow(tempIntersection.victim, tempIntersection.intersectionPoint);
                        screen.Plot((int)x, (int)y, GetColor((int)tempColor.X, (int)tempColor.Y, (int)tempColor.Z));
                        if(y == 200 && x % 10 == 0)
                            Debug.rayList.Add((new Vector2(camera.position.X, camera.position.Z), new Vector2(tempIntersection.intersectionPoint.X, tempIntersection.intersectionPoint.Z)));
                    }
                    else
                    {
                        screen.Plot((int)x, (int)y, GetColor(200,200,200));
                        if(y == 200 && x % 10 == 0)
                            Debug.rayList.Add((new Vector2(camera.position.X, camera.position.Z), new Vector2(camera.position.X + rayDirection.X * 20, camera.position.Z + rayDirection.Z * 20)));

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

                if (!scene.ShadowIntersection(intersectionPoint, shadowRayDirection))
                {
                    return victim.color;
                }   
            }
            return new Vector3(0, 0, 0);
        }

        public int GetColor(int R, int G, int B)
        {
            return (R << 16) + (G <<8) + B;
        }
    }
}