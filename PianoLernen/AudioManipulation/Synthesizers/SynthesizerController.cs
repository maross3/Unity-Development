using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace AudioManipulation.Synthesizers
{
    public class SynthesizerController : MonoBehaviour
    {
        public int numberOfVoices = 12; // Adjust the number of voices as desired
        public List<AudioClip> audioClips; // Populate with your audio clips
        [FormerlySerializedAs("audioSouce")] public AudioSource audioSource; // Reference to the audio source component
        
        private PolyphonicSynthesizer synthesizer;
        private List<IAudioEffect> audioEffects;
        private float[] audioBuffer;
        private void Awake()
        {
            // todo look more into framesize
            // Calculate the frame size based on the sample rate and frame size
            int frameSize = (int)(44100 * 0.025f);  // 1102 samples 
            // Calculate the buffer size based on the sample rate and frame size
            int bufferSize = AudioSettings.outputSampleRate;
            // Initialize the audio buffer
            audioBuffer = new float[bufferSize];
            audioSource = GetComponent<AudioSource>();
            audioClips = AudioClipsGenerator.GenerateAllNoteClips();
            audioSource.clip = audioClips[audioClips.Count / 2];
            // Populate audio effects
            audioEffects = new List<IAudioEffect>();
            audioEffects.AddRange(GetComponents<IAudioEffect>());
            audioEffects.AddRange(GetComponentsInChildren<IAudioEffect>());

            var voices = new List<Voice>();
            // Create and initialize the synthesizer
            for (var i = 0; i < numberOfVoices; i++)
            {
                voices.Add(new 
                    Voice(new NoteData(Vector2.zero, -1, Note.A, -1), audioEffects));
            }
            synthesizer = new PolyphonicSynthesizer(
                new RoundRobinAllocationStrategy(voices), audioClips);
            
            synthesizer.Initialize(audioEffects);
            audioSource.Play();
        }

        public void Update()
        {
            audioSource.clip.SetData(audioBuffer, 0);
        }
        
            // todo, the buffer makes crazy allocations because size of data is 2048
            //Debug.Log($"trying to write {data.Length} to size {audioBuffer.Length}");
        private void OnAudioFilterRead(float[] data, int channels)
        {
            // Process audio through the synthesizer
            synthesizer.ProcessAudio(data, channels);
            audioBuffer = data;
        } 
    }
}