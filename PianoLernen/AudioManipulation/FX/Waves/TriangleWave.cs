using DefaultNamespace;
using UnityEngine;

namespace FX.Waves
{
    public class TriangleWave : SynthWave
    {
        public override string GetSerializationOutput()
        {
            throw new System.NotImplementedException();
        }

        protected override float UpdateAudioClip(float time)
        {
            var period = 1f / frequency;
            var phase = time / period;
            var value = 2f * (Mathf.Abs((phase - Mathf.Floor(phase + 0.5f))) - 0.5f); 
            
            return value * amplitude;
        }
    }
}