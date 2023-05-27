namespace AudioManipulation.Synthesizers
{
    public interface IVoiceAllocationStrategy
    {
        void AllocateVoice(NoteData note);
        void StopVoice(Note note);
        void ReleaseVoice(Note note); 
    }
}