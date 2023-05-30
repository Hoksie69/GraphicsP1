using OpenTK.Mathematics;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2023Template
{
    internal class Primitive
    {
        Vector3 position = new Vector3();
        Vector3 direction = new Vector3();

        public Primitive(Vector3 _position) 
        {
            position = _position;
        }

        public void Sphere(float radius)
        {
            float x, y, z, xy;
            float nx, ny, nz, lengthInv = 1.0f / radius;

            int sectorAmount = 36;
            int stackAmount = 18;

            float sectorStep = (float)(2 * Math.PI / sectorAmount);
            float stackStep = (float)(Math.PI / stackAmount);
            float sectorAngle, stackAngle;

            for (int i = 0; i < sectorAmount; i++)
            {
                stackAngle = (float)(Math.PI / 2 - i * stackStep);
                xy = (float)(radius * Math.Cos(stackAngle));
                z = (float)(radius * Math.Sin(stackAngle));

                for (int j = 0; j < stackAmount; j++)
                {
                    sectorAngle = j * sectorStep;

                    x = (float)(xy * Math.Cos(sectorAngle));
                    y = (float)(xy * Math.Cos(sectorAngle));
                    vertices.push_back(x);
                    vertices.push_back(y);
                    vertices.push_back(z);
                }
            }
        }

        public void Plane(Vector3 direction)
        {

        }
    }
}
