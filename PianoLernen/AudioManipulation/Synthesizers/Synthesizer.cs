using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AudioManipulation.Synthesizers;
using DefaultNamespace;
using FX.Waves;
using NAudio.Wave;
using Sirenix.OdinInspector;
using UnityEngine;

public interface IAudioEffect
{
    // T Describe<T>();
    void ApplyEffect(ref byte[] samples, float freq, float amp);
    void ApplyEffect(ref float[] samples, float freq, float amp);
    string GetSerializationOutput();
}

[RequireComponent(typeof(AudioSource))]
public class Synthesizer : MonoBehaviour
{
    public static int outputSampleRate;
    
    public float frequency = 440f;
    public float amplitude = 0.5f;
    public int maxConcurrentKeys = 7;

    private readonly List<IAudioEffect> _audioEffects = new();
    private static List<Voice> _activeVoices = new();
    private readonly List<Voice> _voices = new();
    private AudioSource _audioSource;
    private bool _processing;

    #region extract logic
    // todo add wavetype property to abstract accessor in SynthWave
    // find a good name like 'descriptor' and make it a part of IAudioEffect
    // likely that we will need to index them, if not for functionality, for 
    // user's searching for the right effect
   [OnValueChanged("SwapWaveType")] 
    public WaveType waveType;

    private void SwapWaveType()
    {
        var wave = GetComponent<SynthWave>();
        _audioEffects.Remove(_audioEffects.Find(x => (SynthWave) x == wave));
        if (wave != null) Destroy(wave);
        
        switch (waveType)
        {
            case WaveType.Sine:
                wave = gameObject.AddComponent<SinWave>();
                break;
            case WaveType.Square:
                wave = gameObject.AddComponent<SquareWave>();
                break;
            case WaveType.Sawtooth:
                wave = gameObject.AddComponent<Sawtooth>();
                break;
            case WaveType.Triangle:
                wave = gameObject.AddComponent<TriangleWave>();
                break;
            case WaveType.Pulse :
                wave = gameObject.AddComponent<PulseWave>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _audioEffects.Add(wave);
    }
    #endregion
    
    /// <summary>
    /// Initialize and populate Audio Effects
    /// </summary>
    private void PopulateEffects()
    {
        _audioEffects.AddRange(GetComponents<IAudioEffect>());
        _audioEffects.AddRange(GetComponentsInChildren<IAudioEffect>());
    }
    
    private void ClearAudioEffects()
    {
        foreach(var fx in _audioEffects) Destroy(fx as MonoBehaviour);
        _audioEffects.Clear();
    }

    private void Start()
    {
        PopulateEffects();
        SwapWaveType();
        _audioSource = gameObject.GetComponent<AudioSource>();
        MidiInputListener.OnMidiDown += OnKeyDown;
        MidiInputListener.OnMidiUp += OnKeyUp;
        GenerateAudioClip();
        _audioSource.Stop();
        _audioSource.loop = false;
    }

    private void OnKeyUp(NoteData obj)
    {
        _voices.Remove(_voices.Find(x => x.Note == obj.note));
        if (_voices.Count == 0) Stop();
    }


    private void OnKeyDown(NoteData obj)
    {
        if (_voices.Count < maxConcurrentKeys)
            _voices.Add(new Voice(obj, _audioEffects.ToList()));
        if (_voices.Count == 1) Play();
    }

    private void Play()
    {
        _processing = true;
    }

    private void Stop()
    {
        _processing = false;
    }

    private void Update()
    {
        if (_processing && !_audioSource.isPlaying) _audioSource.Play();
        else if (!_processing && _audioSource.isPlaying) _audioSource.Stop();

        outputSampleRate = AudioSettings.outputSampleRate;
    }


    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (_audioEffects == null || !_processing) return;
        _activeVoices = _voices.ToList();

        foreach (var voice in _activeVoices)
        {
            voice.SetAmplitude(amplitude / 10);
            voice.SetFrequency(frequency);
            voice.GenerateSamples(data);
        }
    }

    /// <summary>
    /// Creates a base "noise" audio clip.
    /// </summary>
    /// <returns>An audio clip of the synth.</returns>
    private AudioClip GenerateAudioClip()
    {
        var sampleRate = AudioSettings.outputSampleRate;
        var numSamples = (int) (sampleRate * _audioSource.clip.length);
        var samples = new float[numSamples];

        for (var i = 0; i < numSamples; i++)
        {
            var time = i / (float) sampleRate;
            var angle = time * frequency * 2f * Mathf.PI;
            var sample = Mathf.Sin(angle) * amplitude;
            samples[i] = sample;
        }

        var audioClip = AudioClip.Create("SynthClip", numSamples, 1, sampleRate, false);
        audioClip.SetData(samples, 0);
        return audioClip;
    }

    #region Generic Audio

    private byte[] GenerateFileAudioClip()
    {
        var sampleRate = 44100; // Set your desired sample rate
        var numSamples = (int) (sampleRate * _audioSource.clip.length);
        var audioData = new byte[numSamples * 2]; // 16-bit audio (2 bytes per sample)

        var memoryStream = new MemoryStream(audioData);
        var binaryWriter = new BinaryWriter(memoryStream);

        for (var i = 0; i < numSamples; i++)
        {
            var time = i / (float) sampleRate;
            var angle = time * frequency * 2f * MathF.PI;
            var sample = MathF.Sin(angle) * amplitude;

            // Convert the float sample to a 16-bit signed short value
            var sampleValue = (short) (sample * short.MaxValue);

            // Write the sample value as two bytes (little-endian)
            binaryWriter.Write(sampleValue);
        }

        binaryWriter.Close();
        memoryStream.Close();

        return audioData;
    }

    /// <summary>
    /// Play audio using nAudio, this is generic implementation to decouple unity from project.
    /// </summary>
    private void PlayAudio()
    {
        var sampleRate = 44100;
        var bufferSize = 1024;

        var waveFormat = new WaveFormat(sampleRate, 16, 1);
        var waveProvider = new BufferedWaveProvider(waveFormat);
        waveProvider.BufferLength = bufferSize * 2;
        waveProvider.DiscardOnBufferOverflow = true;

        var waveOut = new WaveOutEvent();
        waveOut.Init(waveProvider);
        waveOut.Play();

        var buffer = new byte[bufferSize * 2];
        _audioEffects.ForEach(effect => effect.ApplyEffect(ref buffer, frequency, amplitude / 10));
        waveProvider.AddSamples(buffer, 0, buffer.Length);
    }

    #endregion
}

public enum WaveType
{
    Square,
    Pulse,
    Triangle,
    Sawtooth,
    Sine,
}