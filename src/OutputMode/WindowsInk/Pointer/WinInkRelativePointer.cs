using System.Numerics;
using OpenTabletDriver.Plugin.Platform.Display;
using OpenTabletDriver.Plugin.Platform.Pointer;
using OpenTabletDriver.Plugin.Tablet;

namespace VoiDPlugins.OutputMode
{
    public unsafe class WinInkRelativePointer : WinInkBasePointer, IRelativePointer
    {
        private Vector2 _maxPoint;
        private Vector2 _currentPoint;
        private Vector2 _error;

        public WinInkRelativePointer(TabletReference tabletReference, IVirtualScreen screen) : base("Windows Ink", tabletReference)
        {
            _maxPoint = new Vector2(screen.Width, screen.Height);
            _currentPoint = _maxPoint / 2;
        }

        public void SetPosition(Vector2 delta)
        {
            Instance!.EnableButtonBit((int)WindowsInkButtonFlags.InRange);
            delta += _error;
            _error = new Vector2(delta.X % 1, delta.Y % 1);

            _currentPoint = Vector2.Clamp(_currentPoint + delta, Vector2.Zero, _maxPoint);
            RawPointer->X = (ushort)_currentPoint.X;
            RawPointer->Y = (ushort)_currentPoint.Y;
        }
    }
}