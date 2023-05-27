using DefaultNamespace;
using UnityEngine;

namespace FX.Waves
{
    /// <summary>
    /// A smooth, pure tone with no harmonic content other than the fundamental frequency.
    /// </summary>
    public class SinWave : SynthWave
    {
        public override string GetSerializationOutput()
        {
            return "todo, return json data as string";
        }

        /// <summary>
        /// A smooth, pure tone with no harmonic content other than the fundamental frequency.
        /// </summary>
        protected override float UpdateAudioClip(float time)
        {
            var angle = time * frequency * 2f * Mathf.PI;
            var sample = Mathf.Sin(angle) * amplitude;
            return sample;
        }
    }
}