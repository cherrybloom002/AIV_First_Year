using Aiv.Fast2D;
using Aiv.Fast3D;
using OpenTK;

namespace Fast3D_Meshes
{
    class _02_QuadMeshExample
    {
        static public void Run()
        {
            Window win = new Window(800, 600, "Mesh");

            PerspectiveCamera camera = new PerspectiveCamera(new Vector3(0, 0, 10), new Vector3(0, 180, 0), 60, 0.1f, 1000f);

            Mesh3 triangle = new Mesh3();
            triangle.v = new float[]
            {
                //Triangle Left
               -0.5f, -0.5f, 0f, //bottom-left
                0.5f,  0.5f, 0f, //top-right
               -0.5f,  0.5f, 0f, //top-left

                //Triangle Right
               -0.5f, -0.5f, 0f, //bottom-left
                0.5f, -0.5f, 0f, //bottom-right
                0.5f,  0.5f, 0f  //top-right
            };
            triangle.UpdateVertex();

            triangle.Position3 = new Vector3(0, 1.5f, 0);
            triangle.Scale3 = new Vector3(3f);
            //triangle.Pivot3 = new Vector3(0f, 0.5f, 0);

            while(win.IsOpened)
            {
                //triangle.DrawColor(1f, 0f, 0f);
                triangle.DrawWireframe(1f, 0f, 0f);
                win.Update();
            }
        }
    }
}
