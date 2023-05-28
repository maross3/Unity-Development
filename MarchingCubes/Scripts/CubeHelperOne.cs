using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class CubeHelperOne : MonoBehaviour
{
    public Texture2D[] sliceImages;
    public int numberImages;
    public string imagePathPrefix;
    public string imagePathExtension;
    public float depthScale;

    // Start is called before the first frame update
    void Start()
    {
        sliceImages = new Texture2D[numberImages]; // Initialize the array to hold the Texture2D objects
        for (int i = 0; i < numberImages; i++)
        {
            var imagePath = imagePathPrefix + i + imagePathExtension;
            LoadImage(i, imagePath);
        }
    }

    private void CreateMesh()
    {
        int width = sliceImages.Max(img => img.width);
        int height = sliceImages.Max(img => img.height);
        int depth = sliceImages.Length;
Debug.Log($"Image width: {width}");
Debug.Log($"Image height: {height}");
        // Create a new Texture3D
        Texture3D volumeTexture = new Texture3D(width, height, depth, TextureFormat.R8, false);
        // Create a Color array to hold the pixel values
        Color[] colors = new Color[width * height * depth];

        // Iterate through the stack of images and assign pixel values to the Color array
        for (int i = 0; i < depth; i++)
        {
            // Load the 2D image (assuming it's stored as a Texture2D)
            Texture2D sliceTexture = sliceImages[i];

            // Iterate through each pixel in the image
            for (var x = 0; x < width - 1  && x < volumeTexture.width - 1; x++)
            {
                for (var y = 0; y < height - 1 && y < volumeTexture.height - 1; y++)
                {
                    // Get the pixel color from the image
                    var pixelColor = sliceTexture.GetPixel(x, y);
                    
                    // Calculate the index in the 1D Color array
                    var index = x + y * width + i * (width * height);

                    // Assign the pixel color to the Color array
                    colors[index] = pixelColor;
                }
            }
        }

        // Set the pixel data to the Texture3D
        volumeTexture.SetPixels(colors);
        volumeTexture.Apply();
 
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        
        // Create a MarchingCubes scriptable object
        var marchingCubes = ScriptableObject.CreateInstance<MarchingCubes>();

        // Set the threshold value to determine the surface
        float threshold = 0.7f;
        marchingCubes.Threshold = threshold;

        Mesh mesh = marchingCubes.GenerateMesh(volumeTexture);
        // meshFilter.mesh = mesh;
        meshFilter.sharedMesh = mesh;

        Material material = new Material(Shader.Find("Standard"));
        
        // Enable GPU instancing for better performance (optional)
        // meshRenderer.additionalVertexStreams = mesh;

        material.renderQueue = (int) RenderQueue.Background;
        material.SetInt("_CullMode", (int)CullMode.Off);
        
        material.SetFloat("_Metallic", 0.0f);
        material.SetFloat("_Glossiness", 0.6f);
        material.SetInt("_SrcBlend", (int) BlendMode.One);
        material.SetInt("_DstBlend", (int) BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.EnableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        // material.SetTexture("_MainTex", volumeTexture);
        
        meshRenderer.sharedMaterial = material;
    }
    
    private void LoadImage(int index, string imagePath)
    {
        var texture = TextureSerializer.LoadTextureFromFile(imagePath); 
        sliceImages[index] = texture;
    }
    // Update is called once per frame
    void Update()
    {
        if (numberImages != sliceImages.Length) return;
        numberImages = 0;
        CreateMesh();
    }
}