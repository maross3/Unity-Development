using System.Collections.Generic;
using UnityEngine;

namespace AudioManipulation.Synthesizers
{
    // refactored. double check this code. Throw away if none useful :)
    public class PolyphonicSynthesizer
    {
        public float frequency = 440f;
        public float amplitude = 0.2f;
        
        private List<IAudioEffect> voiceEffects;
        private Dictionary<Note, Voice> activeVoices;
        private IVoiceAllocationStrategy voiceAllocationStrategy;
        
        public PolyphonicSynthesizer(IVoiceAllocationStrategy allocationStrategy, List<AudioClip> audioClips)
        {
            activeVoices = new Dictionary<Note, Voice>();
            voiceAllocationStrategy = allocationStrategy;
            
        }

        public void Initialize(List<IAudioEffect> audioEffects)
        {
            voiceEffects = audioEffects;
            MidiInputListener.OnMidiDown += HandleMidiDownDown;
            MidiInputListener.OnMidiUp += HandleMidiUpEvent;
        }
        
        public void ProcessAudio(float[] data, int channels)
        {
            foreach (var voice in activeVoices.Values)
            {
                //voice.GenerateSamples(data, frequency, amplitude);
            }
        }
        
        private void HandleMidiUpEvent(NoteData note)
        {
            if (!activeVoices.ContainsKey(note.note)) return;
            voiceAllocationStrategy.ReleaseVoice(note.note);
            activeVoices.Remove(note.note);
        }
        
        private void HandleMidiDownDown(NoteData note)
        {
            if (activeVoices.ContainsKey(note.note))
            {
                voiceAllocationStrategy.StopVoice(note.note);
                activeVoices.Remove(note.note);
            }
            else
            {
                voiceAllocationStrategy.AllocateVoice(note);
                activeVoices.Add(note.note, new Voice(note, voiceEffects));
            }
        }
    }
}