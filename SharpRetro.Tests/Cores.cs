using Moq;
using NUnit.Framework;
using SharpRetro.DirectX.Video;
using SharpRetro.Emulators;
using SharpRetro.Games;
using SharpRetro.Libretro.Audio;
using SharpRetro.Libretro.Cores;
using SharpRetro.Libretro.Emulators;
using SharpRetro.Libretro.Environment;
using SharpRetro.Libretro.Input;
using SharpRetro.Libretro.Interop;
using SharpRetro.Libretro.Native;
using SharpRetro.Libretro.Video;
using SharpRetro.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpRetro.Tests
{
  public class Cores
  {
    [Test]
    public void TestCoreEntryPointInit()
    {
      var library = new Mock<ILibrary>();
    }

    [Test]
    public void TestCoreInit()
    {
      string coreDirectory = @"E:\Games\Cores";
      string corePath = @"catsfc_libretro.dll";
      string gamePath = @"E:\Games\SNES\Super Mario Kart (USA).sfc";
      ILibrary library = new Library(Path.Combine(coreDirectory, corePath));
      ILibretroCore core = new LibretroCore(library);
      IEnvironmentHandler environment = new Libretro.Environment.Environment()
      {
        LibretroPath = coreDirectory,
        SystemDirectory = coreDirectory,
        SaveDirectory = coreDirectory
      };

      var video = new Mock<IVideoOutput>();
      video.Setup(v => v.OnVideoRefresh(It.IsAny<IntPtr>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));

      var audio = new Mock<IAudioOutput>();
      audio.Setup(a => a.OnAudioSample(It.IsAny<short>(), It.IsAny<short>()));
      audio.Setup(a => a.OnAudioSampleBatch(It.IsAny<IntPtr>(), It.IsAny<uint>())).Returns((IntPtr p, uint frames) => frames);

      var input = new Mock<IInput>();
      input.Setup(i => i.OnInputPoll());
      input.Setup(i => i.OnInputState(It.IsAny<uint>(), It.IsAny<uint>(), It.IsAny<uint>(), It.IsAny<uint>())).Returns(0);
      input.Setup(i => i.TrySetRumbleInterface(ref It.Ref<retro_rumble_interface>.IsAny)).Returns(false);

      IEmulator emulator = new LibretroEmulator(core, environment, new TextureOutput(null, null), audio.Object, input.Object, new ConsoleLogger());
      emulator.Init();

      IGame game = new Game { Path = gamePath, Data = File.ReadAllBytes(gamePath) };
      bool loaded = emulator.Load(game);

      for (int i = 0; i < 600; i++)
      {
        emulator.Run();
        //Thread.Sleep(20);
      }

      return;
    }
  }
}
