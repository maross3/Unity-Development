using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextureSerializer : MonoBehaviour
{
    public static byte[] SerializeTexture(Texture2D texture)
    {
        return texture.EncodeToPNG(); // Convert the texture to PNG format (byte array)
    }

    public static Texture2D DeserializeTexture(byte[] serializedData)
    {
        Texture2D texture = new Texture2D(1, 1); // Create a new Texture2D object
        texture.LoadImage(serializedData); // Load the image data into the texture
        return texture;
    }

    public static void SaveTextureToFile(Texture2D texture, string filePath)
    {
        byte[] serializedData = SerializeTexture(texture);
        File.WriteAllBytes(filePath, serializedData);
    }

    public static Texture2D LoadTextureFromFile(string filePath)
    {
        byte[] serializedData = File.ReadAllBytes(filePath);
        return DeserializeTexture(serializedData);
    }
    
    public static Texture2D LoadTextureFromFileAsync(string filePath)
    {
        byte[] serializedData = File.ReadAllBytes(filePath);
        return DeserializeTexture(serializedData);
    }
}
