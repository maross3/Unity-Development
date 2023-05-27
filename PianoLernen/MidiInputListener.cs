using System;
using System.Collections.Generic;
using System.Net;
using FX.Waves;
using Sirenix.OdinInspector;
using UnityEngine;
using NAudio.Midi;
using Unity.VisualScripting;
using UnityEditor.UIElements;

public class MidiInputListener : MonoBehaviour
{
    private TimeMonitor _monitor;
    private List<MidiEvent> events;
    private MidiIn midiIn;
    private bool monitoring;
    private List<string> midiDevices = new List<string>();
    public int selectedIndex = 0;
    
    // going to be used to check for chords
    public int curNotes;

    private void Start()
    {
        _monitor = GetComponent<TimeMonitor>();
        _monitor.noteQueue = new Queue<NoteData>(TestCaseGenerator.notes);
        AddDevices();
    }

    private void OnEnable()
    {
        StartListening();
    }

    private void OnDisable()
    {
        StopListening();
    }

    [Button]
    public void SongStart()
    {
        _monitor.SongStart();
    }

    /// <summary>
    /// Adds all recognized MIDI devices to the list of devices
    /// </summary>
    private void AddDevices()
    {
        for (var i = 0; i < MidiIn.NumberOfDevices; i++)
            midiDevices.Add(MidiIn.DeviceInfo(i).ProductName);
        events = new List<MidiEvent>();
        for (var i = 50; i < 62; i++) AddNoteEvent(i);
    }

    // todo, look into channels
    private void AddNoteEvent(int noteNumber)
    {
        const int CHANNEL = 2;
        var noteOnEvent = new NoteOnEvent(0, CHANNEL, noteNumber, 100, 50);

        events.Add(noteOnEvent);
        events.Add(noteOnEvent.OffEvent);
    }

    private void StartListening()
    {
        if (monitoring)
        {
            Debug.LogError(@"Already monitoring MIDI input");
            return;
        }

        midiIn = new MidiIn(selectedIndex);
        midiIn.MessageReceived += InputMessageReceived;
        midiIn.ErrorReceived += InputErrorReceived;
        midiIn.Start();
        monitoring = true;
    }

    private void StopListening()
    {
        if (!monitoring)
        {
            Debug.LogError(@"Not monitoring MIDI input");
            return;
        }

        midiIn.Stop();
        monitoring = false;

        midiIn.Dispose();
        midiIn.MessageReceived -= InputMessageReceived;
        midiIn.ErrorReceived -= InputErrorReceived;
        midiIn = null;
        monitoring = false;
    }

    private void InputErrorReceived(object sender, MidiInMessageEventArgs e)
    {
        Debug.LogError($"[ERR] Timestamp: {e.Timestamp}, message: {e.RawMessage}, midi event: {e.MidiEvent}");
    }

    private void InputMessageReceived(object sender, MidiInMessageEventArgs e)
    {
        if (e.MidiEvent.CommandCode == MidiCommandCode.NoteOn)
        {
            var note = MidiUtil.ExtractDataOne(e.RawMessage);
            curNotes |= note; 
            var noteData = new NoteData(Vector2.zero, e.Timestamp, e.GetNote(), e.GetOctave());
            noteData.Amplitude = 0.1f;
            OnMidiDown?.Invoke(noteData);
        }
        else if (e.MidiEvent.CommandCode == MidiCommandCode.NoteOff)
        {
            var note = MidiUtil.ExtractDataOne(e.RawMessage);
            curNotes &= ~note;
            OnMidiUp?.Invoke(new NoteData(Vector2.zero, e.Timestamp, e.GetNote(), e.GetOctave()));
        }
    }

    [Button]
    private void startButton_Click() => StartListening();

    [Button]
    private void stopButton_Click() => StopListening();

    public static event Action<NoteData> OnMidiDown;
    public static event Action<NoteData> OnMidiUp;
}