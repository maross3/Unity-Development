using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Buffer data variables

        private const int ChunkSize = 10000;
        private int currentChunkIndex = 0;
    // Start is called before the first frame update
    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Callback when connected to Photon server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon server");
        PhotonNetwork.JoinRandomRoom();
    }

    public void SynchronizeMesh(ComputeBuffer meshBuffer, ComputeBuffer noiseBuffer)
    {
        if (!photonView.IsMine) return;
        _meshBuffer = meshBuffer;
        SendBufferData(SerializeComputeBuffer(noiseBuffer));
    }
    // Callback when failed to join a random room
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room. Creating a new room.");

        // Create a new room
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions());
    }

    // Callback when successfully joined a room
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room");
    }

    // Method to send buffer data to other clients
    public void SendBufferData(byte[] noiseBuffer)
    {
        // Call a Photon RPC to send the buffer data to other clients
        photonView.RPC("ReceiveBufferData", RpcTarget.Others, noiseBuffer);
    }

    public ComputeBuffer _meshBuffer;
    public ComputeBuffer _noiseBuffer;
    
    // Photon RPC method to receive buffer data from the authoritative client
    [PunRPC]
    private void ReceiveBufferData(byte[] noiseBuffer)
    {
        if (!photonView.IsMine) return;
        // Deserialize the buffer data
        ComputeBuffer receivedNoiseBuffer = DeserializeComputeBuffer(noiseBuffer);
        _noiseBuffer?.Release();
        _noiseBuffer = receivedNoiseBuffer;
    }

    // Method to serialize a ComputeBuffer into a byte array
    private byte[] SerializeComputeBuffer(ComputeBuffer buffer)
    {
        int bufferSize = buffer.count * sizeof(int);
        int[] bufferData = new int[buffer.count];
        buffer.GetData(bufferData);

        byte[] serializedData = new byte[bufferSize];
        System.Buffer.BlockCopy(bufferData, 0, serializedData, 0, bufferSize);

        return serializedData;
    }

    // Method to deserialize a byte array into a ComputeBuffer
    private ComputeBuffer DeserializeComputeBuffer(byte[] serializedData)
    {
        ComputeBuffer buffer = new ComputeBuffer(serializedData.Length / sizeof(int), sizeof(int));
        int[] bufferData = new int[buffer.count];
        System.Buffer.BlockCopy(serializedData, 0, bufferData, 0, serializedData.Length);

        buffer.SetData(bufferData);

        return buffer;
    }
    private float[] largeBuffer = new float[7700000]; // Adjust the size as needed

    
  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (_meshBuffer == null)return;
            int startIndex = currentChunkIndex * ChunkSize;
            int length = Mathf.Min(ChunkSize, _meshBuffer.count - startIndex);
            float[] currentChunk = new float[length * 7];
    
            // Get the data from the compute buffer
            _meshBuffer.GetData(currentChunk, startIndex, 0, length * 7);
            SerializeFloatArray(stream, currentChunk);
            
            currentChunkIndex++;
            if (currentChunkIndex >= GetSubBufferCount()) currentChunkIndex = 0;
        }
        else
        {
            if (_meshBuffer == null)return;
            var receivedChunk = DeserializeFloatArray(stream);
            var startIndex = currentChunkIndex * ChunkSize;
            
            DeserializeComputeBuffer(receivedChunk, startIndex);

            currentChunkIndex++;
            if (currentChunkIndex >= GetSubBufferCount()) currentChunkIndex = 0;
        }
    }
  private void DeserializeComputeBuffer(float[] receivedChunk, int startIndex)
  {
      _meshBuffer.SetData(receivedChunk, startIndex, 0, receivedChunk.Length);
  }

    private void SerializeFloatArray(PhotonStream stream, float[] array)
    {
        for (var i = 0; i < array.Length; i++)
            stream.SendNext(array[i]);
    }

    private float[] DeserializeFloatArray(PhotonStream stream)
    {
        float[] array = new float[ChunkSize];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (float)stream.ReceiveNext();
        }
        return array;
    }

    private int GetSubBufferCount()
    {
        // Calculate the total number of sub-buffers based on the chunk size and large buffer size
        int totalChunks = Mathf.CeilToInt((float)largeBuffer.Length / ChunkSize);
        return totalChunks;
    }
}
