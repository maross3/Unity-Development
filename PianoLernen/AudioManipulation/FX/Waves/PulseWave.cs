using DefaultNamespace;

namespace FX.Waves
{
    /// <summary>
    /// Similar to a <see cref="SquareWave"/>, but with a variable duty cycle that determines the width of the positive part of the waveform.
    /// </summary>
    /// <TODO>
    /// properties: period, phase, duty cycle.
    /// period is 1f / frequency.
    /// Find out what the numerator represents and what to name it. Allow user to manipulate
    /// </TODO>
    public class PulseWave : SynthWave
    {
        public override string GetSerializationOutput()
        {
            return "null";
        }

        /// <summary>
        /// Similar to a <see cref="SquareWave"/>, but with a variable duty cycle that determines the width of the positive part of the waveform.
        /// </summary>
        protected override float UpdateAudioClip(float time)
        {
            var period = 1f / frequency;
            var phase = time % period / period;
            var dutyCycle = 0.5f; // Adjust the duty cycle between 0 and 1
            var sample = phase < dutyCycle ? amplitude : -amplitude;
            return sample;
        }
    }
}