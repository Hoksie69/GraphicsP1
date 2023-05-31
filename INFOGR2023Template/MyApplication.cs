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
        }
        // tick: renders one frame
        public void Tick()
        {

            if (window.IsKeyPressed(Keys.LeftAlt))
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
                rayTracer.Debug();
            }
        }
    }
}