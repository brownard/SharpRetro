using SharpGL;
using System;

namespace SharpRetro.DirectX.GL
{
  public abstract class AbstractBuffer : IDisposable
  {
    protected OpenGL _gl;
    protected uint _id;

    protected AbstractBuffer(OpenGL gl)
    {
      _gl = gl;
    }

    public void Create()
    {
      uint[] ids = new uint[1];
      Create(1, ids);
      _id = ids[0];
    }

    public void Bind()
    {
      Bind(_id);
    }

    public void UnBind()
    {
      Bind(0);
    }

    public uint Id
    {
      get { return _id; }
    }

    protected abstract void Create(uint count, uint[] ids);

    protected abstract void Destroy(uint id);

    protected abstract void Bind(uint id);

    public void Dispose()
    {
      if (_id > 0)
      {
        Destroy(_id);
        _id = 0;
      }
    }
  }
}
