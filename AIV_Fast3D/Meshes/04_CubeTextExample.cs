using Aiv.Fast2D;
using Aiv.Fast3D;
using OpenTK;
using System;

namespace Fast3D_Meshes
{
    class _04_CubeTextExample
    {
        static public void Run()
        {
            Window win = new Window(800, 600, "Mesh");
            win.EnableDepthTest();
            win.CullBackFaces();

            PerspectiveCamera camera = new PerspectiveCamera(new Vector3(0, 0, 10), new Vector3(0, 180, 0), 60, 0.1f, 1000f);

            Mesh3 triangle = new Mesh3();
            triangle.v = new float[]
            {
                //FRONT
                //Triangle Left
               -1, -1,  1, //bottom-left
                1,  1,  1, //top-right
               -1,  1,  1, //top-left

                //Triangle Right
               -1, -1,  1, //bottom-left
                1, -1,  1, //bottom-right
                1,  1,  1, //top-right

                //BACK
                //Triangle Left
                1, -1, -1, //bottom-left
               -1,  1, -1, //top-right
                1,  1, -1, //top-left

                //Triangle Right
                1, -1, -1, //bottom-left
               -1, -1, -1, //bottom-right
               -1,  1, -1, //top-right

               //RIGHT
                //Triangle Left
                1, -1,  1, //bottom-left
                1,  1, -1, //top-right
                1,  1,  1, //top-left

                //Triangle Right
                1, -1,  1, //bottom-left
                1, -1, -1, //bottom-right
                1,  1, -1, //top-right

               //LEFT
                //Triangle Left
               -1, -1, -1, //bottom-left
               -1,  1,  1, //top-right
               -1,  1, -1, //top-left

                //Triangle Right
               -1, -1, -1, //bottom-left
               -1, -1,  1, //bottom-right
               -1,  1,  1, //top-right

               //TOP
                //Triangle Left
               -1,  1,  1, //bottom-left
                1,  1, -1, //top-right
               -1,  1, -1, //top-left

                //Triangle Right
               -1,  1,  1, //bottom-left
                1,  1,  1, //bottom-right
                1,  1, -1, //top-right

               //BOTTOM
                //Triangle Left
               -1, -1, -1, //bottom-left
                1, -1,  1, //top-right
               -1, -1,  1, //top-left

                //Triangle Right
               -1, -1, -1, //bottom-left
                1, -1, -1, //bottom-right
                1, -1,  1, //top-right

            };
            triangle.UpdateVertex();

            //triangle.Position3 = new Vector3(0, 1.5f, 0);
            triangle.Scale3 = new Vector3(1.5f);
            //triangle.Pivot3 = new Vector3(0f, 0.5f, 0);

            Texture wood = new Texture("Assets/wood-box.jpg");

            
            triangle.uv = new float[]
            {
                //FRONT
                //Triangle Left
                0f, 0f,   //bottom-left
                1f, 1f,   //top-right
                0f, 1f,   //top-left

                //Triangle Right
                0f, 0f,   //bottom-left
                1f, 0f,   //bottom-right
                1f, 1f,   //top-right

                //BACK
                //Triangle Left
                0f, 0f,   //bottom-left
                1f, 1f,   //top-right
                0f, 1f,   //top-left

                //Triangle Right
                0f, 0f,   //bottom-left
                1f, 0f,   //bottom-right
                1f, 1f,   //top-right

                //RIGHT
                //Triangle Left
                0f, 0f,   //bottom-left
                1f, 1f,   //top-right
                0f, 1f,   //top-left

                //Triangle Right
                0f, 0f,   //bottom-left
                1f, 0f,   //bottom-right
                1f, 1f,   //top-right

                //LEFT
                //Triangle Left
                0f, 0f,   //bottom-left
                1f, 1f,   //top-right
                0f, 1f,   //top-left

                //Triangle Right
                0f, 0f,   //bottom-left
                1f, 0f,   //bottom-right
                1f, 1f,   //top-right

                //UP
                //Triangle Left
                0f, 0f,   //bottom-left
                1f, 1f,   //top-right
                0f, 1f,   //top-left

                //Triangle Right
                0f, 0f,   //bottom-left
                1f, 0f,   //bottom-right
                1f, 1f,   //top-right

                //BOTTOM
                //Triangle Left
                0f, 0f,   //bottom-left
                1f, 1f,   //top-right
                0f, 1f,   //top-left

                //Triangle Right
                0f, 0f,   //bottom-left
                1f, 0f,   //bottom-right
                1f, 1f,   //top-right
            };
            
            FlipUvOnY(triangle.uv);
            triangle.UpdateUV();

            triangle.vn = new float[]
           {
                //FRONT
                //Triangle Left
               0, 0,  1, //bottom-left
               0, 0,  1, //top-right
               0, 0,  1, //top-left
            
                //Triangle Right
               0, 0,  1, //bottom-left
                0, 0,  1, //bottom-right
                0,  0,  1, //top-right

                //BACK
                //Triangle Left
                0, 0, -1, //bottom-left
                0, 0, -1, //top-right
                0, 0, -1, //top-left

                //Triangle Right
                0, 0, -1, //bottom-left
                0, 0, -1, //bottom-right
                0, 0, -1, //top-right

               //RIGHT
                //Triangle Left
                1, 0,  0, //bottom-left
                1, 0, 0, //top-right
                1, 0, 0, //top-left

                //Triangle Right
                1, 0, 0, //bottom-left
                1, 0, 0, //bottom-right
                1, 0, 0, //top-right

               //LEFT
                //Triangle Left
               -1, 0, 0, //bottom-left
               -1, 0, 0, //top-right
               -1, 0, 0, //top-left

                //Triangle Right
               -1, 0, 0, //bottom-left
               -1, 0, 0, //bottom-right
               -1, 0, 0, //top-right

               //TOP
                //Triangle Left
                0,  1, 0, //bottom-left
                0,  1, 0, //top-right
                0,  1, 0, //top-left

                //Triangle Right
                0,  1,  0, //bottom-left
                0,  1,  0, //bottom-right
                0,  1,  0, //top-right

               //BOTTOM
                //Triangle Left
               0, -1, 0, //bottom-left
               0, -1, 0, //top-right
               0, -1, 0, //top-left

                //Triangle Right
               0, -1, 0, //bottom-left
               0, -1, 0, //bottom-right
               0, -1, 0, //top-right

           };

           triangle.UpdateNormals();

            DirectionalLight light = new DirectionalLight(new Vector3(-1f, 0, 0));
            Vector3 ambient = new Vector3(0.2f);

            while (win.IsOpened)
            {
                //triangle.DrawColor(1f, 0f, 0f);
                //triangle.DrawWireframe(1f, 0f, 0f);
                
                if (win.GetKey(KeyCode.Right))
                {
                    triangle.EulerRotation3 += new Vector3(0, 30 * win.DeltaTime, 0);
                }


                //triangle.DrawTexture(wood);
                triangle.DrawPhong(wood, light, ambient);
                win.Update();
            }
        }

        private static void FlipUvOnY(float[] uv)
        {
            for(int i=1; i < uv.Length; i+=2)
            {
                uv[i] = 1f - uv[i];
            }
        }
    }
}
