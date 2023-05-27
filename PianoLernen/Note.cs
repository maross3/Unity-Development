using System;
using UnityEngine;

// game name idea? MelodyCraft


// check notes: 
// if ((input & Note.ASharpMinor) != 0 && (~input ^ Note.ASharpMinor) != 0)
[Flags]
public enum Note
{
    None = 0,
    AFlat = 1,
    A = 2,
    ASharp = 4,
    BFlat = 8,
    B = 16,
    BSharp = C,
    CFlat = B,
    C = 32,
    CSharp = 64,
    DFlat = CSharp,
    D = 128,
    DSharp = 256,
    EFlat = DSharp,
    E = 512,
    ESharp = F,
    F = 1024,
    FSharp = ESharp,
    GFlat = FSharp,
    G = 2048,
    GSharp = AFlat
}

public class NoteData
{
    public readonly Vector2 noteDownInterval;
    public readonly int targetTimeStamp;
    public float currentTimeStamp => TimeMonitor.currentTimeStamp;
    public float Frequency { get; set; }
    public float Amplitude { get; set; }

    public readonly Note note;
    public readonly int octave;
    public NoteData(Vector2 interval, int targetTimeStamp, Note note, int octave)
    {
        noteDownInterval = interval;
        this.targetTimeStamp = targetTimeStamp;
        this.note = note;
        this.octave = octave;
    }

    public override string ToString()
    {
        return $"Note Interval: {noteDownInterval}, TargetTimestamp: {targetTimeStamp}, Note: {note}, Octave: {octave}";
    }
}
