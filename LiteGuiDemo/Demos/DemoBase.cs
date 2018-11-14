namespace LiteGuiDemo.Demos
{
    internal abstract class DemoBase
    {
        internal abstract bool Startup();
        internal abstract void Shutdown();
        internal abstract void Tick(float Seconds);
        internal abstract void Render();
    }
}