using NAudio.Midi;
using UnityEngine;

public static class MidiUtil 
{
    private static Note[] keyNames = { Note.C, Note.CSharp, Note.D, Note.DSharp, Note.E, Note.F, Note.FSharp,Note.G, Note.GSharp, Note.A, Note.ASharp, Note.B };

    public static int ExtractDataOne(int rawMessage) =>
        (rawMessage >> 8) & 0xFF;

    public static int GetOctave(this MidiInMessageEventArgs midiEvent) =>
        ExtractDataOne(midiEvent.RawMessage) / 12;

    public static Note GetNote(this MidiInMessageEventArgs midiEvent) =>
        ConvertKeyNumberToKeyName(ExtractDataOne(midiEvent.RawMessage));
    
    public static float GetFrequencyFromNoteNumber(int noteNumber)
    {
        // The frequency of A4 note (MIDI note number 69.... hehehe)
        var a = 440f; 
        var h = Mathf.Pow(2f, 1f / 12f);
        float n = noteNumber - 69;

        return a * Mathf.Pow(h, n);
    }
    private static Note ConvertKeyNumberToKeyName(int keyNumber)
    {
        int noteIndex = keyNumber % 12;
        var keyName = keyNames[noteIndex];
        return keyName;
    }
}
