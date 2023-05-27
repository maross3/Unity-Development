using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMonitor : MonoBehaviour
{
    public float updateInterval = 0.03f; // Update interval in seconds
    private float currentTime;
    public static float currentTimeStamp;
    public Queue<NoteData> noteQueue;
    
    public void SongStart() =>
       StartCoroutine(UpdateTime()); 

    private float Eval(Queue<NoteData> notes) =>
        notes.Count == 0 ? 0f : notes.Dequeue().noteDownInterval.x;
    
    private IEnumerator UpdateTime()
    {
        while (true)
        {
            updateInterval = Eval(noteQueue) - updateInterval;
            yield return new WaitForSeconds(updateInterval);
            currentTime += updateInterval;
            currentTimeStamp = Mathf.Floor(currentTime * 1000f); // Convert to milliseconds
        }
    }
}
