using SharpRetro.Client.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace SharpRetro.Client
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    EmulatorViewModel _emulatorViewModel;

    public MainWindow()
    {
      InitializeComponent();

      _emulatorViewModel = new EmulatorViewModel();
      CompositionTarget.Rendering += OnRetroRender;
    }

    private void OnRetroRender(object sender, EventArgs e)
    {
      _emulatorViewModel.InitDirect3D(new WindowInteropHelper(this).Handle);
      _emulatorViewModel.Run();
      RenderTexture();
    }

    protected void RenderTexture()
    {
      if (d3dimg.IsFrontBufferAvailable)
      {
        d3dimg.Lock();
        Int32Rect dirtyRect;
        d3dimg.SetBackBuffer(D3DResourceType.IDirect3DSurface9, _emulatorViewModel.GetBackBuffer(out dirtyRect));
        d3dimg.AddDirtyRect(dirtyRect);
        d3dimg.Unlock();
      }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      _emulatorViewModel.Dispose();
      base.OnClosing(e);
    }
  }
}
