using SharpRetro.Games;
using System;

namespace SharpRetro.Emulators
{
  public interface IEmulator : IDisposable
  {
    void Init();
    bool Load(IGame game);
    void Unload();
    void Run();
    void Reset();
  }
}
