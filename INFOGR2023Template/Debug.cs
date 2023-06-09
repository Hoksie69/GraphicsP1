﻿using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template;

namespace INFOGR2023Template
{
    internal class Debug
    {
        Surface screen;
        RayTracer rayTracer;
        public static List<(Vector2, Vector2)> rayList = new List<(Vector2, Vector2)>();
        public static List<(Vector2, Vector2)> shadowRayList = new List<(Vector2, Vector2)>();
        public static List<(Vector2, Vector2)> secondaryRayList = new List<(Vector2, Vector2)>();

        bool showViewRay = true;
        bool showShadowRay = true;
        bool showSecondaryRay = true;

        public Debug(Surface _screen, RayTracer _rayTracer) 
        {
            rayTracer = _rayTracer;
            screen = _screen;
        }

        public void PlotPixel(int x, int y, Vector3 Color)
        {
            x = x + screen.width / 2;
            y = y + screen.height / 2;
            screen.Plot(x, y, rayTracer.GetColor((int)Color.X, (int)Color.Y, (int)Color.Z));
        }

        public void PlotLine(Vector2 start, Vector2 eind, Vector3 Color)
        {
            start.X = start.X + screen.width / 2;
            start.Y = start.Y + screen.height / 2;
            eind.X = eind.X + screen.width / 2;
            eind.Y = eind.Y + screen.height / 2;
            screen.Line((int)start.X, (int)start.Y, (int)eind.X, (int)eind.Y, rayTracer.GetColor((int)Color.X, (int)Color.Y, (int)Color.Z));
        }

        public void DebugOutput()
        {
            float scaleX = 1f / 16f * ((float)screen.width / 2f);
            float scaleY = 1f / 10f * ((float)screen.height / 2f);
            if(showShadowRay)
                foreach ((Vector2, Vector2) ray in shadowRayList)
                    PlotLine(new Vector2(ray.Item1.X * scaleX, ray.Item1.Y * scaleY), new Vector2(ray.Item2.X * scaleX, ray.Item2.Y * scaleY), new Vector3(255, 255, 0));

            if (showViewRay)
                foreach ((Vector2, Vector2) ray in rayList)
                    PlotLine(new Vector2(ray.Item1.X * scaleX, ray.Item1.Y * scaleY), new Vector2(ray.Item2.X * scaleX, ray.Item2.Y * scaleY), new Vector3(0, 255, 255));

            if (showSecondaryRay)
                foreach((Vector2, Vector2) ray in secondaryRayList)
                    PlotLine(new Vector2(ray.Item1.X * scaleX, ray.Item1.Y * scaleY), new Vector2(ray.Item2.X * scaleX, ray.Item2.Y * scaleY), new Vector3(125, 0, 255));             
            
            PlotPixel((int)(rayTracer.camera.position.X * scaleX), (int)(rayTracer.camera.position.Z * scaleY), new Vector3(255, 255, 255));
            Console.WriteLine(rayTracer.camera.position);
            PlotLine(new Vector2(rayTracer.CamPlane[0].X * scaleX, rayTracer.CamPlane[0].Z * scaleY), new Vector2(rayTracer.CamPlane[1].X * scaleX, rayTracer.CamPlane[1].Z * scaleY), new Vector3(255, 255, 255));

            
            foreach(Primitive cirkel in rayTracer.scene.primitivesList) 
            { 
                if(cirkel is Sphere)
                {
                    Sphere Uranus = (Sphere)cirkel;
                    for(float i = 0; i < 360; i += 0.5f)
                    {
                        double x = Math.Cos(MathHelper.DegreesToRadians(i)) * Uranus.radius;
                        double y = Math.Sin(MathHelper.DegreesToRadians(i)) * Uranus.radius;
                        PlotPixel((int)((x + Uranus.position.X) * scaleX), (int)((y + Uranus.position.Z) * scaleY), Uranus.color);
                    }
                }
            }
        }
    }
}
