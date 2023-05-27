using System.Collections.Generic;
using System.Linq;

namespace AudioManipulation.Synthesizers
{
    public class RoundRobinAllocationStrategy : IVoiceAllocationStrategy
    {
        private List<Voice> voices;
        private int currentIndex;

        public RoundRobinAllocationStrategy(List<Voice> availableVoices)
        {
            voices = availableVoices;
            currentIndex = 0;
        }

        public void AllocateVoice(NoteData note)
        {
            if (currentIndex >= voices.Count) currentIndex = 0;
            voices[currentIndex].SetFrequency(note.Frequency);
            voices[currentIndex].SetAmplitude(note.Amplitude);
            currentIndex++;
        }

           public void StopVoice(Note note)
        {
            var voice = voices.FirstOrDefault(v => v.Note == note);
            voice?.Stop();
        }

        public void ReleaseVoice(Note note)
        {
            // Find the voice associated with the note and release it
            var voice = voices.FirstOrDefault(v => v.Note == note);
            voice?.Release();
        } 
    }
}