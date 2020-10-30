// Decompiled with JetBrains decompiler
// Type: ActivityBrowser.CaptureScreenshot
// Assembly: Piggyback3, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C45B2B95-8862-4F73-BBB7-A401C9603E2E
// Assembly location: C:\Program Files (x86)\Developmental Optometry\Piggyback 3\Piggyback3.exe

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ActivityBrowser
{
  public static class CaptureScreenshot
  {
    public static BitmapSource Capture(Rect area)
    {
      try
      {
        IntPtr dc = CaptureScreenshot.GetDC(IntPtr.Zero);
        IntPtr compatibleDc = CaptureScreenshot.CreateCompatibleDC(dc);
        IntPtr compatibleBitmap = CaptureScreenshot.CreateCompatibleBitmap(dc, (int) area.Width, (int) area.Height);
        CaptureScreenshot.SelectObject(compatibleDc, compatibleBitmap);
        CaptureScreenshot.BitBlt(compatibleDc, 0, 0, (int) area.Width, (int) area.Height, dc, (int) area.X, (int) area.Y, CaptureScreenshot.TernaryRasterOperations.SRCCOPY);
        BitmapSource sourceFromHbitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(compatibleBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        CaptureScreenshot.DeleteObject(compatibleBitmap);
        CaptureScreenshot.ReleaseDC(IntPtr.Zero, dc);
        CaptureScreenshot.ReleaseDC(IntPtr.Zero, compatibleDc);
        return sourceFromHbitmap;
      }
      catch (Exception ex)
      {
      }
      return (BitmapSource) null;
    }

    public static BitmapSource Capture(HandleRef hWnd)
    {
      try
      {
        CaptureScreenshot.API_RECT lpRect;
        if (CaptureScreenshot.GetWindowRect(hWnd, out lpRect))
        {
          Size size = new Size((double) (lpRect.Right - lpRect.Left), (double) (lpRect.Bottom - lpRect.Top));
          return CaptureScreenshot.Capture(new Rect(new Point((double) lpRect.Left, (double) lpRect.Top), size));
        }
      }
      catch (Exception ex)
      {
      }
      return (BitmapSource) null;
    }

    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateBitmap(
      int nWidth,
      int nHeight,
      uint cPlanes,
      uint cBitsPerPel,
      IntPtr lpvBits);

    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("gdi32.dll")]
    private static extern bool BitBlt(
      IntPtr hdc,
      int nXDest,
      int nYDest,
      int nWidth,
      int nHeight,
      IntPtr hdcSrc,
      int nXSrc,
      int nYSrc,
      CaptureScreenshot.TernaryRasterOperations dwRop);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(HandleRef hWnd, out CaptureScreenshot.API_RECT lpRect);

    private enum TernaryRasterOperations : uint
    {
      BLACKNESS = 66, // 0x00000042
      NOTSRCERASE = 1114278, // 0x001100A6
      NOTSRCCOPY = 3342344, // 0x00330008
      SRCERASE = 4457256, // 0x00440328
      DSTINVERT = 5570569, // 0x00550009
      PATINVERT = 5898313, // 0x005A0049
      SRCINVERT = 6684742, // 0x00660046
      SRCAND = 8913094, // 0x008800C6
      MERGEPAINT = 12255782, // 0x00BB0226
      MERGECOPY = 12583114, // 0x00C000CA
      SRCCOPY = 13369376, // 0x00CC0020
      SRCPAINT = 15597702, // 0x00EE0086
      PATCOPY = 15728673, // 0x00F00021
      PATPAINT = 16452105, // 0x00FB0A09
      WHITENESS = 16711778, // 0x00FF0062
    }

    public struct API_RECT
    {
      public int Left;
      public int Top;
      public int Right;
      public int Bottom;
    }
  }
}
