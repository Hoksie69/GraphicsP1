using INFOGR2023Template;

namespace Template
{
    class MyApplication
    {
        // member variables
        public Surface screen;
        public RayTracer rayTracer;
        // constructor
        public MyApplication(Surface screen)
        {
            this.screen = screen;
        }
        // initialize
        public void Init()
        {
            rayTracer = new RayTracer(screen);
        }
        // tick: renders one frame
        public void Tick()
        {
            screen.Clear(0);
            rayTracer.Render();
        }
    }
}