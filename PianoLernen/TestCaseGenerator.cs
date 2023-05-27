using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestCaseGenerator : MonoBehaviour
{
    public static List<NoteData> notes;
    
    public int numNotes = 20; // Number of notes to generate
    public float minInterval = 0.5f; // Minimum note down interval in seconds
    public float maxInterval = 2f; // Maximum note down interval in seconds
    public int minTargetTimeStamp = 0; // Minimum target timestamp
    public int maxTargetTimeStamp = 100; // Maximum target timestamp
    public int minOctave = 3; // Minimum octave
    public int maxOctave = 5; // Maximum octave

    [Button]
    public List<NoteData> GenerateSortedTestCase()
    {
        List<NoteData> testcase = new List<NoteData>();

        for (int i = 0; i < numNotes; i++)
        {
            float interval = Random.Range(minInterval, maxInterval);
            int targetTimeStamp = Random.Range(minTargetTimeStamp, maxTargetTimeStamp) * 1000;
            Note note = (Note)Random.Range(0, System.Enum.GetValues(typeof(Note)).Length);
            int octave = Random.Range(minOctave, maxOctave + 1);

            NoteData noteData = new NoteData(new Vector2(interval, interval), targetTimeStamp, note, octave);
            testcase.Add(noteData);
        }
        testcase.RadixSort();
        return testcase;
    }

    private void Awake()
    {
        notes = GenerateSortedTestCase();
    }
}
