using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast3D;
using System.Drawing;

namespace _84_Lezione_24_06_Heightmap
{
    static class HeightmapGenerator
    {
        public static Mesh3 Load(string filepath, float quadSize = 0.1f, float maxHeight = 5.0f, float textureUVMul = 1.0f, bool useGradient = false)
        {
            Mesh3 mesh = new Mesh3();

            // We need a way to retrieve the pixels value RGB for the whole image,
            // so we get a bitmap of it to be able to work with it
            Bitmap image = new Bitmap($"Assets/Heightmaps/{filepath}.png");
            int mapW = image.Width;
            int mapH = image.Height;

            // We'll use each pixel's R value to determine the height of the vertices
            // we'll use to create the mesh, so the Y value of each vertex

            // We'll use a specific quadSize value to determine the width and widness
            // of the vertices, so the X and Z value of each vertex

            // Size of the mesh (number of quads for rows and columns)
            float meshWidth = (mapW - 1) * quadSize;
            float meshHeight = (mapH - 1) * quadSize;

            // Total number of quads
            int numQuads = (mapW - 1) * (mapH - 1);
            // Each quad = 2 triangle = 6 vertices
            int numVertex = numQuads * 6;

            // Create arrays for vertices and UVs
            mesh.v = new float[numVertex * 3];
            mesh.uv = new float[numVertex * 2];

            // We'll use these lists first to calculate and record vertices positions, 
            // UV coordinates and the quads's indices, the we'll use these to set the
            // proper mesh vertices and UVs
            List<float> positions = new List<float>();
            List<float> textCoords = new List<float>();
            List<int> indices = new List<int>();

            Color c;

            for (int z = 0; z < mapH; z++)
            {
                for (int x = 0; x < mapW; x++)
                {
                    c = image.GetPixel(x, z);

                    // Vertex Y (height) will be retrieved from pixel's R value
                    // normalized and multiplied by the max Height we want to use
                    // (a value of 1.0f will match the max Height)
                    float vY = (float)c.R / 255.0f * maxHeight;

                    // Set vertex position
                    positions.Add(x * quadSize);
                    positions.Add(vY);
                    positions.Add(z * quadSize);

                    // Set vertex UV
                    if(!useGradient)
                    {
                        // Normal Texture UVs mapping
                        textCoords.Add(quadSize * (float)x / meshWidth * textureUVMul);
                        textCoords.Add(quadSize * (float)z / meshWidth * textureUVMul);
                    }
                    else
                    {
                        // Gradient UVs mapping
                        textCoords.Add((float)c.R / 255.0f);
                        textCoords.Add(0);
                    }

                    if(x < mapW - 1 && z < mapH - 1)
                    {
                        // Create indices (y * width + x)
                        int leftTop = z * mapW + x;
                        int leftBottom = (z + 1) * mapW + x;
                        int rightTop = leftTop + 1;
                        int rightBottom = leftBottom + 1;

                        // First triangle
                        indices.Add(leftTop);
                        indices.Add(rightTop);
                        indices.Add(leftBottom);
                        // Second triangle
                        indices.Add(rightTop);
                        indices.Add(rightBottom);
                        indices.Add(leftBottom);
                    }
                }
            }

            for (int i = 0; i < indices.Count; i++)
            {
                // Vertices
                mesh.v[i * 3] = positions[indices[i] * 3];              // x
                mesh.v[i * 3 + 1] = positions[indices[i] * 3 + 1];      // y
                mesh.v[i * 3 + 2] = positions[indices[i] * 3 + 2];      // z

                // UVs
                mesh.uv[i * 2] = textCoords[indices[i] * 2];            // u
                mesh.uv[i * 2 + 1] = textCoords[indices[i] * 2 + 1];    // v
            }

            mesh.UpdateVertex();
            mesh.UpdateUV();
            mesh.RegenerateNormals();

            return mesh;
        }
    }
}
