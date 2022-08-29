using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Br_Midi_Player.lib;

public static class WindowHelper
{
    // 判断游戏窗口是否拥有焦点中
    public static bool IsGameFocused(String processName)
    {
        var window = FindWindowByProcessName(processName);
        return window != null &&
            IsWindowFocused((IntPtr)window);
    }

    // 保持游戏窗口置顶
    public static void EnsureGameOnTop(String processName)
    {
        var window = FindWindowByProcessName(processName);
        if (window is null) return;

        SwitchToThisWindow((IntPtr)window, true);
    }

    // 判断窗口是否拥有焦点中
    private static bool IsWindowFocused(IntPtr windowPtr)
    {
        var hWnd = GetForegroundWindow();
        return hWnd.Equals(windowPtr);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    private static extern IntPtr GetForegroundWindow();

    // 通过进程名获得窗口Handle指针
    private static IntPtr? FindWindowByProcessName(string? processName)
    {
        var process = Process.GetProcessesByName(processName);
        return process.FirstOrDefault(p => p.MainWindowHandle != IntPtr.Zero)?.MainWindowHandle;
    }

    [DllImport("user32.dll")]
    private static extern void SwitchToThisWindow(IntPtr hWnd, bool fUnknown);
}