using INFOGR2023Template;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Windows.Input;

namespace Template
{
    class MyApplication
    {
        bool debugMode = false;

        // member variables
        public Surface screen;
        public RayTracer rayTracer;
        GameWindow window;
        public Debug debug;
        // constructor
        public MyApplication(Surface screen, OpenTKApp window)
        {
            this.screen = screen;
            this.window = window;
        }
        // initialize
        public void Init()
        {
            rayTracer = new RayTracer(screen);
            debug = new Debug(screen, rayTracer);
        }
        // tick: renders one frame
        public void Tick()
        {
            if (window.IsKeyPressed(Keys.Q))
            {
                rayTracer.camera.FOV -= 15;
                rayTracer.camera.GetPlane();
                Debug.rayList.Clear();
            }
            if (window.IsKeyPressed(Keys.O))
            {
                rayTracer.camera.AimAt(new OpenTK.Mathematics.Vector3(0, 0, 1));
                rayTracer.camera.GetPlane();
                Debug.rayList.Clear();
            }
            if (window.IsKeyPressed(Keys.E))
            {
                rayTracer.camera.FOV += 15;
                rayTracer.camera.GetPlane();
                Debug.rayList.Clear();
            }

            if (window.IsKeyPressed(Keys.Right))
            { 
                rayTracer.camera.GetNewAngle(22.5f);
                rayTracer.camera.GetPlane();
                Debug.rayList.Clear();
            }

            if (window.IsKeyPressed(Keys.Left))
            {
                rayTracer.camera.GetNewAngle(-22.5f);
                rayTracer.camera.GetPlane();
                Debug.rayList.Clear();
            }
            
            if (window.IsKeyPressed(Keys.Up))
            {
                rayTracer.camera.GetNewAngleY(-22.5f);
                rayTracer.camera.GetPlane();
                Debug.rayList.Clear();
            }
            
            if (window.IsKeyPressed(Keys.Down))
            {
                rayTracer.camera.GetNewAngleY(-22.5f);
                rayTracer.camera.GetPlane();
                Debug.rayList.Clear();
            }

            if (window.IsKeyPressed(Keys.W))
            {
                rayTracer.camera.position += rayTracer.camera.look_at;
                foreach(Primitive s in rayTracer.scene.primitivesList)
                {
                    Console.WriteLine(s.position);
                }
                rayTracer.camera.GetPlane();
                Debug.rayList.Clear();
            }

            if (window.IsKeyPressed(Keys.D))
            {
                rayTracer.camera.position += rayTracer.camera.right_direction;
                rayTracer.camera.GetPlane();
                Debug.rayList.Clear();
            }

            if (window.IsKeyPressed(Keys.S))
            {
                rayTracer.camera.position -= rayTracer.camera.look_at;
                rayTracer.camera.GetPlane();
                Debug.rayList.Clear();
            }

            if (window.IsKeyPressed(Keys.A))
            {
                rayTracer.camera.position -= rayTracer.camera.right_direction;
                rayTracer.camera.GetPlane();
                Debug.rayList.Clear();
            }

            if (window.IsKeyPressed(Keys.Space))
            {
                rayTracer.camera.position += rayTracer.camera.up_direction;
                rayTracer.camera.GetPlane();
                Debug.rayList.Clear();
            }

            if (window.IsKeyPressed(Keys.LeftShift))
            {
                rayTracer.camera.position -= rayTracer.camera.up_direction;
                rayTracer.camera.GetPlane();
                Debug.rayList.Clear();
            }


            if (window.IsKeyDown(Keys.LeftAlt) && debugMode == false)
            {
                debugMode = !debugMode;
            }
            if (window.IsKeyDown(Keys.Backspace) && debugMode == true)
            {
                debugMode = !debugMode;
            }

            screen.Clear(0);
            if (!debugMode)
            {
                rayTracer.Render();
            }
            else
            {
                debug.DebugOutput();
            }
        }
    }
}