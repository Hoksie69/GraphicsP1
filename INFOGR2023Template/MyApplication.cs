using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Template
{
    class MyApplication
    {
        // member variables
        public Surface screen;
        Surface map;
        float[,] h;

        int originalScreenX = 640;
        int originalScreenY = 400;

        float a = 0;

        // constructor
        public MyApplication(Surface screen)
        {
            this.screen = screen;
        }
        // initialize
        public void Init()
        {
            map = new Surface("../../../assets/coin.png");
            h = new float[256, 256];
            for (int y = 0; y < 256; y++) for (int x = 0; x < 256; x++)
                h[x, y] = ((float)(map.pixels[x + y * 256] & 255)) / 256;
        }
        // tick: renders one frame
        public void Tick()
        {
            int calcGreen(int shade) { return shade * 256; }
            int calcRed(int shade) { return shade * 256 * 256; }
            int MixColor(int red, int green, int blue)
            {
                return (red << 16) + (green << 8) + blue;
            }

            screen.Clear(0);

            int centerX = 320, centerY = 200;

            float x1 = 220, y1 = 150;
            float rx1 = 320 + (float)((x1 - centerX) * Math.Cos(a) - (y1 - centerY) * Math.Sin(a));
            float ry1 = 200 + (float)((x1 - centerX) * Math.Sin(a) + (y1 - centerY) * Math.Cos(a));

            //top right corner
            float x2 = 420, y2 = 150;
            float rx2 = 320 + (float)((x2 - centerX) * Math.Cos(a) - (y2 - centerY) * Math.Sin(a));
            float ry2 = 200 + (float)((x2 - centerX) * Math.Sin(a) + (y2 - centerY) * Math.Cos(a));

            //bottom left corner
            float x3 = 220, y3 = 250;
            float rx3 = 320 + (float)((x3 - centerX) * Math.Cos(a) - (y3 - centerY) * Math.Sin(a));
            float ry3 = 200 + (float)((x3 - centerX) * Math.Sin(a) + (y3 - centerY) * Math.Cos(a));

            //bottom right corner
            float x4 = 420, y4 = 250;
            float rx4 = 320 + (float)((x4 - centerX) * Math.Cos(a) - (y4 - centerY) * Math.Sin(a));
            float ry4 = 200 + (float)((x4 - centerX) * Math.Sin(a) + (y4 - centerY) * Math.Cos(a));

            a += 0.05f;

            screen.Line((int)rx1, (int)ry1, (int)rx2, (int)ry2, 0xffffff);
            screen.Line((int)rx1, (int)ry1, (int)rx3, (int)ry3, 0xffffff);
            screen.Line((int)rx2, (int)ry2, (int)rx4, (int)ry4, 0xffffff);
            screen.Line((int)rx3, (int)ry3, (int)rx4, (int)ry4, 0xffffff);

            Matrix4 M = Matrix4.CreatePerspectiveFieldOfView(1.6f, 1.3f, .1f, 1000);
            GL.LoadMatrix(ref M);
            GL.Translate(0, 0, 1);
            GL.Rotate(110, 1, 0, 0);
            GL.Rotate(a * 180 / Math.PI, 0, 0, -1);
        }
        public void RenderGL()
        {
            float z = 1f;
            GL.Color3(1.0f, 0.0f, 0.0f);
            for (int y = 0; y < 255; y++) for (int x = 0; x < 255; x++)
            {
                GL.Begin(PrimitiveType.Triangles);
                GL.Vertex3(x / 256, y / 256, h[x,y] * z);
                GL.Vertex3(x + 1 / 256, y / 256, h[x + 1, y] * z);
                GL.Vertex3(x / 256, y + 1 / 256, h[x, y + 1] * z);
                GL.End();
            }
            for (int y = 0; y < 255; y++) for (int x = 0; x < 255; x++)
            {
                GL.Begin(PrimitiveType.Triangles);
                GL.Vertex3(x + 1 / 256, y + 1 / 256, h[x + 1, y + 1] * z);
                GL.Vertex3(x + 1 / 256, y / 256, h[x + 1, y] * z);
                GL.Vertex3(x / 256, y + 1 / 256, h[x, y + 1] * z);
                GL.End();
            }
        }
        /*
        public void RenderGL()
        {
            float centerX = 0.5f;
            float centerY = 0.5f;
            float zScale = 2.0f;

            GL.Color3(1.0f, 0.0f, 0.0f);
            for (int y = 0; y < 255; y++)
            {
                for (int x = 0; x < 255; x++)
                {
                    float z1 = h[x, y] * zScale / (1 + (x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));
                    float z2 = h[x + 1, y] * zScale / (1 + (x + 1 - centerX) * (x + 1 - centerX) + (y - centerY) * (y - centerY));
                    float z3 = h[x, y + 1] * zScale / (1 + (x - centerX) * (x - centerX) + (y + 1 - centerY) * (y + 1 - centerY));
                    float z4 = h[x + 1, y + 1] * zScale / (1 + (x + 1 - centerX) * (x + 1 - centerX) + (y + 1 - centerY) * (y + 1 - centerY));

                    GL.Begin(PrimitiveType.Triangles);
                    GL.Vertex3(x / 256.0f, y / 256.0f, z1);
                    GL.Vertex3((x + 1) / 256.0f, y / 256.0f, z2);
                    GL.Vertex3(x / 256.0f, (y + 1) / 256.0f, z3);
                    GL.End();

                    GL.Begin(PrimitiveType.Triangles);
                    GL.Vertex3((x + 1) / 256.0f, y / 256.0f, z2);
                    GL.Vertex3((x + 1) / 256.0f, (y + 1) / 256.0f, z4);
                    GL.Vertex3(x / 256.0f, (y + 1) / 256.0f, z3);
                    GL.End();
                }
            }
        }*/
    }
}