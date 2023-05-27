using DefaultNamespace;
using UnityEngine;

namespace FX.Waves
{
/// <summary>
///  A waveform with a square or rectangular shape, consisting of odd harmonics.
/// </summary>
    public class SquareWave : SynthWave
    {
         public override string GetSerializationOutput()
        {
            return "null";
        }

/// <summary>
///  A waveform with a square or rectangular shape, consisting of odd harmonics.
/// </summary>
         protected override float UpdateAudioClip(float time)
         {
             var sample = Mathf.Sin(2 * Mathf.PI * frequency * time);
             if (sample >= 0) sample = amplitude;
             else sample = -amplitude;
             return sample;
         }
    }
}