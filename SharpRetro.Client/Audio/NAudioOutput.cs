using NAudio.Wave;
using SharpRetro.Libretro.Audio;
using SharpRetro.Libretro.Environment;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace SharpRetro.Client.Audio
{
  class NAudioOutput : IAudioOutput
  {
    protected BufferedWaveProvider _provider;
    protected IWavePlayer _player;
    protected Timing _timing;
    protected bool _isInit;
    protected byte[] _buffer = new byte[0];

    protected void Init(Timing timing)
    {
      if (_isInit)
        DeInit();
      WaveFormat format = new WaveFormat((int)timing.SampleRate, 2);
      _provider = new BufferedWaveProvider(format);
      _provider.BufferDuration = TimeSpan.FromMilliseconds(400);
      _player = new WaveOut(WaveCallbackInfo.FunctionCallback());
      _player.Init(_provider);
      _isInit = true;
    }

    protected void DeInit()
    {
      _player.Stop();
      _player.Dispose();
    }

    public void SetTiming(Timing timing)
    {
      _timing = timing;
      Init(timing);
    }

    public void OnAudioSample(short left, short right)
    {
      if (!_isInit)
        return;
      AllocateBuffer(4);
      _buffer[0] = (byte)left;
      _buffer[1] = (byte)(left >> 8);
      _buffer[2] = (byte)right;
      _buffer[3] = (byte)(right >> 8);
      AddSamples(4);
    }

    public uint OnAudioSampleBatch(IntPtr data, uint frames)
    {
      if (!_isInit)
        return 0;
      int count = (int)frames * 4;
      AllocateBuffer(count);
      Marshal.Copy(data, _buffer, 0, count);
      AddSamples(count);
      return frames;
    }

    protected void AllocateBuffer(int count)
    {
      if (_buffer.Length < count)
        _buffer = new byte[count];
    }

    protected void AddSamples(int count)
    {
      while (_provider.BufferLength - _provider.BufferedBytes < count)
        Thread.Sleep(10);
      _provider.AddSamples(_buffer, 0, count);

      if (_player.PlaybackState != PlaybackState.Playing)
        _player.Play();
    }
  }
}
