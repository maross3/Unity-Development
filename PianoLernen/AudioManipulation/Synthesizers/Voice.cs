using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace AudioManipulation.Synthesizers
{
    public class Voice
    {
        public Note Note { get; private set; }
        private int intNote;
        public int Octave { get; private set; }
        public float Frequency { get; private set; }
        public float Amplitude { get; private set; }
        
        public List<IAudioEffect> audioEffects;
        private bool stopped;
        
        public Voice(NoteData note, List<IAudioEffect> audioEffects)
        {
            this.audioEffects = audioEffects;
            Note = note.note;
            Octave = note.octave;
            intNote = ConvertNoteToInt(Note);
        }

        private int ConvertNoteToInt(Note note) =>
            note switch
            {
                Note.C => 1,
                Note.D => 2,
                Note.E => 3,
                Note.F => 4,
                Note.G => 5,
                Note.A => 6,
                Note.B => 7,
                _ => 0
            };

        public void GenerateSamples(float[] data)
        { 
            foreach (var effect in audioEffects)
                effect.ApplyEffect(ref data, Frequency * Mathf.Pow(2, intNote/ 12f) * Mathf.Pow(2, Octave),  Amplitude);
        }

        public void SetFrequency(float frequency)
        {
            Frequency = frequency;
        }

        public void SetAmplitude(float amplitude)
        {
            Amplitude = amplitude;
        }

        public void Stop()
        {
            stopped = true;
        }

        public void Release()
        {
            stopped = true;
        } 
    }
}