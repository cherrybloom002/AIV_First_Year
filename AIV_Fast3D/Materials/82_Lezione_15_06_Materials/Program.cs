using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using Aiv.Fast3D;
using OpenTK;

namespace _82_Lezione_15_06_Materials
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
            Window window = new Window(800, 600, "Materials");
            window.EnableDepthTest();
            window.CullBackFaces();
            // Camera
            camera = new PerspectiveCamera(new Vector3(0.0f, 0.0f, -6.0f), new Vector3(0.0f, 0.0f, 0.0f), 60.0f, 0.1f, 1000.0f); ;
            // Lights
            DirectionalLight light = new DirectionalLight(new Vector3(-1.0f, 0.5f, 0.0f));
            Vector3 ambientLight = new Vector3(0.3f);



            // ASSETS INITIALIZATION
            // Colors
            Vector4 red = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
            Vector4 green = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
            Vector4 blue = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);

            Vector4 yellow = red + green;
            Vector4 purple = red + blue;

            // Textures
            Texture wallDiffuse = new Texture("Assets/Wall/wall_diffuse.jpg");
            Texture wallNormal = new Texture("Assets/Wall/wall_normalmap.jpg");

            Texture r2d2Diffuse = new Texture("Assets/R2D2/R2D2_Diffuse.jpg");
            Texture r2d2Normal = new Texture("Assets/R2D2/R2D2_Normal.jpg");
            Texture r2d2Specular = new Texture("Assets/R2D2/R2D2_Specular.jpg");
            Texture r2d2Emissive = new Texture("Assets/R2D2/R2D2_Emissive.jpg");
            Texture r2d2Reflection = new Texture("Assets/R2D2/R2D2_Reflection.jpg");

            // Materials
            Material wallMaterial = new Material();
            wallMaterial.Ambient = ambientLight;
            wallMaterial.Diffuse = wallDiffuse;
            wallMaterial.NormalMap = wallNormal;
            wallMaterial.Lights = new Light[] { light };

            Material r2d2Material = new Material();
            r2d2Material.Ambient = ambientLight;
            r2d2Material.Diffuse = r2d2Diffuse;
            r2d2Material.NormalMap = r2d2Normal;
            r2d2Material.SpecularMap = r2d2Specular;
            r2d2Material.EmissiveMap = r2d2Emissive;
            r2d2Material.Lights = new Light[] { light };



            // OBJECTS
            Mesh3 wall = SceneImporter.LoadMesh("Assets/Wall/quad.obj")[0];
            wall.Position3 = new Vector3(0.0f, -7.0f, 20.0f);
            wall.EulerRotation3 = new Vector3(270.0f, 0.0f, 0.0f);
            wall.Scale3 *= 10.0f;

            Mesh3 r2d2 = SceneImporter.LoadMesh("Assets/r2d2/r2-d2.obj")[0];
            r2d2.Position3 = new Vector3(0.0f, 0.0f, 20.0f);
            r2d2.Scale3 *= 0.1f;
            r2d2.Pivot3 = CalculateCenterPivot(r2d2);



            // MAIN LOOP
            while (window.IsOpened)
            {
                // Show FPS on Window Title Bar
                window.SetTitle($"Materials   -   FPS: {1f / window.DeltaTime}");



                // INPUT--------------------------------------------------------
                CameraInput(window, camera);



                // UPDATE-------------------------------------------------------
                r2d2.EulerRotation3 += new Vector3(0.0f, 30.0f * window.DeltaTime, 0.0f);



                // DRAW---------------------------------------------------------
                //wall.DrawColor(purple);
                //wall.DrawPhong(yellow, light, new Vector3(1.0f, 1.0f, 1.0f));
                //wall.DrawTexture(wallNormal);
                wall.DrawPhong(wallMaterial);

                //r2d2.DrawCel(yellow, light, ambientLight);
                //r2d2.DrawTexture(r2d2Diffuse);
                r2d2.DrawPhong(r2d2Material);

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

        private static Vector3 CalculateCenterPivot(Mesh3 mesh)
        {
            Vector3 mins = new Vector3(float.MaxValue);
            Vector3 maxs = new Vector3(float.MinValue);

            for (int i = 0; i < mesh.v.Length; i += 3)
            {
                float x = mesh.v[i + 0];
                float y = mesh.v[i + 1];
                float z = mesh.v[i + 2];

                if (mins.X > x) mins.X = x;
                if (mins.Y > y) mins.Y = y;
                if (mins.Z > z) mins.Z = z;

                if (maxs.X < x) maxs.X = x;
                if (maxs.Y < y) maxs.Y = y;
                if (maxs.Z < z) maxs.Z = z;
            }

            return (maxs + mins) / 2.0f;
        }
    }
}
