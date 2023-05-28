using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Marching Cubes")]
public class MarchingCubes : ScriptableObject
{
    public float Threshold = 0.4f;

    public Mesh GenerateMesh(Texture3D volumeTexture)
    {
        var width = volumeTexture.width;
        Debug.Log($"Texture width: {width}");
        var height = volumeTexture.height;
        var depth = volumeTexture.depth;

        var vertices = new List<Vector3>();
        var triangles = new List<int>();

        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                for (int z = 0; z < depth - 1; z++)
                {
                    var v0 = volumeTexture.GetPixel(x, y, z).r;
                    var v1 = volumeTexture.GetPixel(x + 1, y, z).r;
                    var v2 = volumeTexture.GetPixel(x + 1, y, z + 1).r;
                    var v3 = volumeTexture.GetPixel(x, y, z + 1).r;
                    var v4 = volumeTexture.GetPixel(x, y + 1, z).r;
                    var v5 = volumeTexture.GetPixel(x + 1, y + 1, z).r;
                    var v6 = volumeTexture.GetPixel(x + 1, y + 1, z + 1).r;
                    var v7 = volumeTexture.GetPixel(x, y + 1, z + 1).r;

                    // Perform the marching cubes algorithm
                    var cubeIndex = 0;
                    if (v0 < Threshold) cubeIndex |= 1;
                    if (v1 < Threshold) cubeIndex |= 2;
                    if (v2 < Threshold) cubeIndex |= 4;
                    if (v3 < Threshold) cubeIndex |= 8;
                    if (v4 < Threshold) cubeIndex |= 16;
                    if (v5 < Threshold) cubeIndex |= 32;
                    if (v6 < Threshold) cubeIndex |= 64;
                    if (v7 < Threshold) cubeIndex |= 128;

                    var triangleIndices = MarchingCubesTables.TriangleTable[cubeIndex];
                    
                    for (var i = 0; i < triangleIndices.Length; i += 3)
                    {
                        var index1 = triangleIndices[i];
                        var index2 = triangleIndices[i + 1];
                        var index3 = triangleIndices[i + 2];
                        
                        var vertex1 = InterpolateVertex(index1, x, y, z, Threshold, v0, v1, v2, v3, v4, v5, v6, v7);
                        var vertex2 = InterpolateVertex(index2, x, y, z, Threshold, v0, v1, v2, v3, v4, v5, v6, v7);
                        var vertex3 = InterpolateVertex(index3, x, y, z, Threshold, v0, v1, v2, v3, v4, v5, v6, v7);

                        vertices.Add(vertex1);
                        vertices.Add(vertex2);
                        vertices.Add(vertex3);
                        
                        triangles.Add(vertices.Count - 3);
                        triangles.Add(vertices.Count - 2);
                        triangles.Add(vertices.Count - 1);
                    }
                }
            }
        }

        // Create a new mesh and assign the vertices and triangles
        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        mesh.Optimize();
        
        return mesh;
    }
    
    private Vector3 InterpolateVertex(int index, int x, int y, int z, float threshold, float v0, float v1, float v2, float v3, float v4, float v5, float v6, float v7)
    {
        switch (index)
        {
            case 0: return Vector3.Lerp(new Vector3(x, y, z), new Vector3(x + 1, y, z), (threshold - v0) / (v1 - v0));
            case 1: return Vector3.Lerp(new Vector3(x + 1, y, z), new Vector3(x + 1, y, z + 1),  (threshold - v1) / (v2 - v1));
            case 2: return Vector3.Lerp(new Vector3(x + 1, y, z + 1), new Vector3(x, y, z + 1),  (threshold - v2) / (v3 - v2));
            case 3: return Vector3.Lerp(new Vector3(x, y, z + 1), new Vector3(x, y, z), (threshold - v3) / (v0 - v3));
            case 4: return Vector3.Lerp(new Vector3(x, y + 1, z), new Vector3(x + 1, y + 1, z), (threshold - v4) / (v5 - v4));
            case 5: return Vector3.Lerp(new Vector3(x + 1, y + 1, z), new Vector3(x + 1, y + 1, z + 1), (threshold - v5) / (v6 - v5));
            case 6: return Vector3.Lerp(new Vector3(x + 1, y + 1, z + 1), new Vector3(x, y + 1, z + 1),  (threshold - v6) / (v7 - v6));
            case 7: return Vector3.Lerp(new Vector3(x, y + 1, z + 1), new Vector3(x, y + 1, z), (threshold - v7) / (v4 - v7));
            case 8: return Vector3.Lerp(new Vector3(x, y, z), new Vector3(x, y + 1, z), (threshold - v0) / (v4 - v0));
            case 9: return Vector3.Lerp(new Vector3(x + 1, y, z), new Vector3(x + 1, y + 1, z), (threshold - v1) / (v5 - v1));
            case 10: return Vector3.Lerp(new Vector3(x + 1, y, z + 1), new Vector3(x + 1, y + 1, z + 1), (threshold - v2) / (v6 - v2));
            case 11: return Vector3.Lerp(new Vector3(x, y, z + 1), new Vector3(x, y + 1, z + 1), (threshold - v3) / (v7 - v3));
            default: return Vector3.zero;
        }
    }

   
}

