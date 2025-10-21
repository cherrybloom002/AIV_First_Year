using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using Aiv.Fast3D;
using OpenTK;

namespace _84_Lezione_24_06_Heightmap
{
    class Program
    {
        // Variables
        static float cameraMovSpeed = 20.0f;
        static float cameraRotSpeed = 10.0f;
        static Vector2 mouseRawPosition;

        static PerspectiveCamera camera;

        static void Main(string[] args)
        {
            // WINDOW SETUP
            Window window = new Window(1024, 768, "Heightmap");
            window.EnableDepthTest();
            // Camera
            camera = new PerspectiveCamera(new Vector3(0.0f, 0.0f, -6.0f), new Vector3(0.0f, 0.0f, 0.0f), 60.0f, 0.1f, 1000.0f); ;



            // TEXTURES
            Texture gradient = new Texture("Assets/Textures/gradient.png");
            Texture island = new Texture("Assets/Textures/texture.png");
            Texture face = new Texture("Assets/Heightmaps/face.png");



            // MESH
            Mesh3 mesh = HeightmapGenerator.Load("gs", maxHeight: 20, useGradient: true);
            mesh.Pivot3 = Vector3.Zero;
            mesh.Position3 = new Vector3(0.0f, 1.0f, 0.0f);


            int drawMode = 1;

            // MAIN LOOP
            while (window.IsOpened)
            {
                // Show FPS on Window Title Bar
                window.SetTitle($"Heightmap   -   FPS: {1f / window.DeltaTime}");



                // INPUT--------------------------------------------------------
                CameraInput(window, camera);
                // DrawMode
                if (window.GetKey(KeyCode.Num1))
                {
                    drawMode = 1;   //texture
                }
                else if (window.GetKey(KeyCode.Num2))
                {
                    drawMode = 2;   //wireframe
                }


                // UPDATE-------------------------------------------------------



                // DRAW
                switch (drawMode)
                {
                    case 2:
                        mesh.DrawWireframe(0.0f, 255.0f, 0.0f, 255.0f);
                        break;

                    default:
                        mesh.DrawTexture(gradient);
                        break;
                }

                window.Update();
            }
        }

        static void CameraInput(Window window, PerspectiveCamera camera)
        {
            // Forward (Z Axis)
            if (window.GetKey(KeyCode.W))
            {
                camera.Position3 += camera.Forward * cameraMovSpeed * window.DeltaTime;
            }
            else if (window.GetKey(KeyCode.S))
            {
                camera.Position3 -= camera.Forward * cameraMovSpeed * window.DeltaTime;
            }

            // Right (X Axis)
            if (window.GetKey(KeyCode.A))
            {
                camera.Position3 -= camera.Right * cameraMovSpeed * window.DeltaTime;
            }
            else if (window.GetKey(KeyCode.D))
            {
                camera.Position3 += camera.Right * cameraMovSpeed * window.DeltaTime;
            }

            // Up (Y Axis)
            if (window.GetKey(KeyCode.Q))
            {
                camera.Position3 -= camera.Up * cameraMovSpeed * window.DeltaTime;
            }
            else if (window.GetKey(KeyCode.E))
            {
                camera.Position3 += camera.Up * cameraMovSpeed * window.DeltaTime;
            }

            // Escape
            if (window.GetKey(KeyCode.Esc))
            {
                window.Exit();
            }

            Vector2 deltaMouse = (window.RawMousePosition - mouseRawPosition) * cameraRotSpeed * window.DeltaTime;
            mouseRawPosition = window.RawMousePosition;

            camera.EulerRotation3 += new Vector3(-deltaMouse.Y, deltaMouse.X, 0.0f);
        }
    }
}
