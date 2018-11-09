using SharpRetro.Games;
using System;

namespace SharpRetro.Emulators
{
  public interface IEmulator : IDisposable
  {
    void Init();
    void Deinit();
    bool Load(IGame game);
    void Unload();
    void Run();
    void Reset();
  }
}
