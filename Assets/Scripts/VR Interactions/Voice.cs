using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

// skoreye vsego ne rabotayet. posmotret kogda budet norm sostoyaniye
[RequireComponent(typeof(AudioSource))]
public class Voice : NetworkBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip recording;

    private string micName;

    private int lastMicPos = 0;
    private int recordSamples;
    [SerializeField] private int sampleRate = 16000;
    [SerializeField] private int chunkSize = 256;
    public float noiseThreshhold = 0.01f;
    public float frequencyThreshold = 0.95f;

    private object queueLock = new object();
    public Queue<float> playbackQueue = new Queue<float>();

    public void noiseGateThreshholdTune(float _volumeThreshhold)
    {
        noiseThreshhold = _volumeThreshhold;
    }

    public void frequencyFilterThrshholdTune(float _frequencyThreshhold)
    {
        frequencyThreshold = _frequencyThreshhold;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
        {
            print("is owner");
            if (Microphone.devices.Length > 0)
            {
                micName = Microphone.devices[0];
                recording = Microphone.Start(micName, true, 1, sampleRate);
                print("using mic: " + micName);
            }
            else
            {
                print("no mic");
            }
        }
        else
        {
            print("not owner");
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = AudioClip.Create("VoiceStream", sampleRate * 10, 1, sampleRate, true, OnAudioRead);
        audioSource.loop = true;
        audioSource.Play();
    }

    void Update()
    {
        if (string.IsNullOrEmpty(micName))
            return;

        if (!IsOwner)
            return;

        int _currentPosition = Microphone.GetPosition(micName);
        if (_currentPosition < lastMicPos)
        {
            lastMicPos = 0;
        }
        int _samplesAvailable = _currentPosition - lastMicPos;

        if (_samplesAvailable >= chunkSize)
        {
            float[] _chunk = new float[chunkSize];
            recording.GetData(_chunk, lastMicPos);

            //frequencyFilter(_chunk);
            //NoiseGate(_chunk);

            lastMicPos += chunkSize;

            byte[] _compressed = CompressfloatToByte(_chunk);
            SendChunk(_compressed);
        }
    }

    // multiplayer stuff
    [ServerRpc]
    void SendChunk(byte[] data)
    {
        ReciveChunk(data);
    }

    [ObserversRpc(BufferLast = false)]
    void ReciveChunk(byte[] data)
    {
        if (IsOwner)
            return;

        float[] floats = DecompressByteToFloat(data);

        foreach (float _sample in floats)
        {
            lock (queueLock)
            {
                if (playbackQueue.Count < sampleRate)
                    playbackQueue.Enqueue(_sample);
            }
        }
    }


    // queue dequeue
    void OnAudioRead(float[] _data)
    {
        lock (queueLock)
        {
            if (playbackQueue.Count < 2048)
            {
                for (int i = 0; i < _data.Length; i++)
                    _data[i] = 0f;

                return;
            }

            for (int i = 0; i < _data.Length; i++)
            {
                if (playbackQueue.Count > 0)
                    _data[i] = playbackQueue.Dequeue();
                else
                    _data[i] = 0f;
            }
        }
    }

    // compression decompression
    byte[] CompressfloatToByte(float[] _flaotInput)
    {
        short[] _shortOutput = new short[_flaotInput.Length];

        for (int i = 0; i < _flaotInput.Length; i++)
        {
            float _sample = Mathf.Clamp(_flaotInput[i], -1f, 1f);
            _shortOutput[i] = (short)(_sample * 32767);
        }

        byte[] _bytes = new byte[_shortOutput.Length * 2];

        for (int i = 0; i < _shortOutput.Length; i++)
        {
            _bytes[i * 2] = (byte)(_shortOutput[i] & 0xff);
            _bytes[i * 2 + 1] = (byte)((_shortOutput[i] >> 8) & 0xff);
        }

        return _bytes;
    }

    float[] DecompressByteToFloat(byte[] _byteInput)
    {
        short[] _shortOutput = new short[_byteInput.Length / 2];

        for (int i = 0; i < _shortOutput.Length; i++)
        {
            _shortOutput[i] = System.BitConverter.ToInt16(_byteInput, i * 2);
        }

        float[] _floatOutput = new float[_shortOutput.Length];

        for (int i = 0; i < _shortOutput.Length; i++)
        {
            _floatOutput[i] = _shortOutput[i] / 32767f;
        }

        return _floatOutput;
    }

    /*
    // audio filters
    void NoiseGate(float[] _samples)
    {
        if (!IsOwner)
            return;

        float sum = 0f;

        for (int i = 0; i < _samples.Length; i++)
            sum += Mathf.Abs(_samples[i]);

        float avg = sum / _samples.Length;

        if (avg < noiseThreshhold)
        {
            for (int i = 0; i < _samples.Length; i++)
                _samples[i] *= 0.1f;
            print("NoiseGate: voice cut");
        }
    }
    

    float _lastInput = 0f;
    float _lastOutput = 0f;
    void frequencyFilter(float[] _samples)
    {
        if (!IsOwner)
            return;

        float RC = 1.0f / (frequencyThreshold * 2 * Mathf.PI);
        float dt = 1.0f / sampleRate;
        float alpha = RC / (RC + dt);

        for (int i = 0; i < _samples.Length; i++)
        {
            float _input = _samples[i];
            float _output = alpha * (_lastOutput + _input - _lastInput);

            _samples[i] = _output;

            _lastInput = _input;
            _lastOutput = _output;
        }
    }
    */
}