using NAudio.Wave;
using UnityEngine;

// Audio Effect interface
// create a class that fucks with the sample
// adhere to IAudioEffect
// clear and repopulate the effects whenever adding one
// goal is to make all basic audio effects
// the UI lets you drag and drop them on a square and apply it to synth
// allow me to script my own in the ui too
// ui framework is cool, but maybe focus on sound now

namespace DefaultNamespace
{
    // todo for synthesizer:
    // Sound:
    // serialize the settings of each component via interface to store the synthesizer

    // FX: 
    // Envelope Shaping: Add dynamics and realism
    // Attack: How quickly the sound reaches its peak volume
    // Decay: How quickly the sound drops to the sustain level
    // Sustain: The volume level while the sound is held
    // Release: How quickly the sound fades out after the key is released

    // Filtering: Add character and tone
    // Low Pass: Cutoff frequencies above a certain point
    // High Pass: Cutoff frequencies below a certain point
    // Band Pass: Cutoff frequencies above and below a certain point
    // Band Reject: Cutoff frequencies between a certain point
    //  notch filters

    // Modulation: Add movement and interest (depth)
    // Chorus
    // Flanger
    // Phaser
    // Tremolo

    // Reverb: Add space and depth

    // Delay: adding spaciousness and rhythmic interest
    // Echos
    // Repeats

    // Distortion: Lively and Expressive
    // Saturation: Lively and Expressive
    // Distortion or Saturation effects to add warmth, grit, or overdrive

    // EQ: Add clarity and balance; Boost or cut specific frequency ranges to achieve a desired tonal character
    // Compression: Add punch and control
    // Limiting: Add loudness and control

    // Stereo Imaging: Add width and depth
    // Panning
    // Spatial Effects
    // done
    // create sound
    // apply wave forms based off user input
    
    
    
    // DONE
    // OOP refactor: Synth have components, components do work, use interface
    // polyphonics
    public abstract class SynthWave : MonoBehaviour, IAudioEffect
    {
        private readonly int _sampleRate = 44100;
        public float frequency;
        public float amplitude;

        public void ApplyEffect(ref float[] samples, float freq, float amp)
        {
            frequency = freq;
            amplitude = amp;
            for (var i = 0; i < samples.Length; i++)
            {
                var time = i / (float) Synthesizer.outputSampleRate;
                samples[i] += UpdateAudioClip(time);
            }
        }

        public void ApplyEffect(ref byte[] samples, float freq, float amp)
        {
            
            var waveFormat = new WaveFormat(_sampleRate, 16, 1);
            for (var i = 0; i < samples.Length; i += waveFormat.BlockAlign)
            {
                var time = (float) i / (_sampleRate * waveFormat.BlockAlign);
                var sample = UpdateAudioClip(time);

                var sampleValue = (short) (sample * short.MaxValue);
                samples[i] = (byte) (sampleValue & 0xFF);
                samples[i + 1] = (byte) (sampleValue >> 8);
            }
        }

        public abstract string GetSerializationOutput();
        protected abstract float UpdateAudioClip(float time);
    }
}