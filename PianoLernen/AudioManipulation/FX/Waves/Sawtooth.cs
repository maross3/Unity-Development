using DefaultNamespace;
using UnityEngine;

namespace FX.Waves
{
    public class Sawtooth : SynthWave
    {

        public override string GetSerializationOutput()
        {
            throw new System.NotImplementedException();
        }

        protected override float UpdateAudioClip(float time)
        {
            var period = 1f / frequency;
            var phase = time / period;
            var value = 2f * ((phase - Mathf.Floor(phase)));
            return (value - 1f) * amplitude;
        }
    }
}