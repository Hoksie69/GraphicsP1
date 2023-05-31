using INFOGR2023Template;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Windows.Input;

namespace Template
{
    class MyApplication
    {
        bool debugMode = true;

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

            if (window.IsKeyDown(Keys.LeftAlt) && debugMode == false)
                debugMode = !debugMode;
            if (window.IsKeyDown(Keys.Backspace) && debugMode == true)
                debugMode = !debugMode;

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