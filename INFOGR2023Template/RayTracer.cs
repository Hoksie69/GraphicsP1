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
            for(int x = 0; x < screen.width; x++)
            {
                for (int y = 0; y < screen.height; y++)
                {
                    Vector3 pointOnPlane = camera.screenPlane[0] + (x / screen.width) * cameraPlaneBasisU + (y / screen.height) * cameraPlaneBasisV;
                    Vector3 rayDirection = pointOnPlane - camera.position;
                    rayDirection.Normalize();

                    TraceRay(rayDirection);
                }
            }
        }

        public void TraceRay(Vector3 rayDirection)
        {
            int t = 1;
            Vector3 ray;
            while(t < 100)
            {
                ray = camera.position + t * rayDirection;
                if (scene.Intersection(ray))
                {

                }
            }
        }
    }

}
