using WindowsInput;
using WindowsInput.Native;

namespace Br_Midi_Player.lib
{
    internal class Press
    {
        private static readonly IInputSimulator input = new InputSimulator();

        // 模拟按键按下
        public static void KeyDown(VirtualKeyCode key)
        {
            input.Keyboard.KeyDown(key);
        }

        // 模拟按键放开
        public static void KeyUp(VirtualKeyCode key)
        {
            input.Keyboard.KeyUp(key);
        }
    }
}
