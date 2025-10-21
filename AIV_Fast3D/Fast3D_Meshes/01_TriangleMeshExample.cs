using Aiv.Fast2D;
using Aiv.Fast3D;
using OpenTK;

namespace Fast3D_Meshes
{
    class _01_TriangleMeshExample
    {
        static public void Run()
        {
            Window win = new Window(800, 600, "Mesh");

            PerspectiveCamera camera = new PerspectiveCamera(new Vector3(0, 0, 10), new Vector3(0, 180, 0), 60, 0.1f, 1000f);

            Mesh3 triangle = new Mesh3();
            triangle.v = new float[]
            {
               -0.5f, 0f, 0f, //left
                0.5f, 0f, 0f, //right
                0f, 1f, 0f    //up
            };
            triangle.UpdateVertex();

            triangle.Position3 = new Vector3(3f, 0, 0);
            triangle.Scale3 = new Vector3(3f);
            triangle.Pivot3 = new Vector3(0f, 0.5f, 0);

            while(win.IsOpened)
            {
                //triangle.DrawColor(1f, 0f, 0f);
                triangle.DrawWireframe(1f, 0f, 0f);
                win.Update();
            }
        }
    }
}
