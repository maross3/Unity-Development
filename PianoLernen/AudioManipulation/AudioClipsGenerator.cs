using System.Collections.Generic;
using UnityEngine;

namespace AudioManipulation
{
    public static class AudioClipsGenerator
    {
        public static float amplitude = 0.5f; // Adjust the amplitude as desired

        public static List<AudioClip> GenerateAllNoteClips()
        {
            List<AudioClip> noteClips = new List<AudioClip>();

            for (int noteNumber = 0; noteNumber < 128; noteNumber++)
            {
                float frequency = MidiUtil.GetFrequencyFromNoteNumber(noteNumber);
                AudioClip noteClip = GenerateAudioClip(frequency);
                noteClips.Add(noteClip);
            }

            return noteClips;
        }

        private static AudioClip GenerateAudioClip(float frequency)
        {
            int sampleRate = AudioSettings.outputSampleRate;
            float duration = 1.0f; // Adjust the duration of the audio clip as desired
            int numSamples = (int)(sampleRate * duration);
            float[] samples = new float[numSamples];

            for (int i = 0; i < numSamples; i++)
            {
                float time = i / (float)sampleRate;
                float angle = time * frequency * 2f * Mathf.PI;
                float sample = Mathf.Sin(angle) * amplitude;
                samples[i] = sample;
            }

            AudioClip audioClip = AudioClip.Create("NoteClip", numSamples, 1, sampleRate, false);
            audioClip.SetData(samples, 0);
            return audioClip;
        }
    }
}