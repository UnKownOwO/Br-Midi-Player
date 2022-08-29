using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace Br_Midi_Player.lib
{
    static class Key
    {
        public static IDictionary<int, VirtualKeyCode> keys = new Dictionary<int, VirtualKeyCode>() {
            {42,VirtualKeyCode.OEM_2},
            {43,VirtualKeyCode.VK_B},
            {44,VirtualKeyCode.NUMPAD0},
            {45,VirtualKeyCode.VK_N},
            {46,VirtualKeyCode.DELETE},
            {47,VirtualKeyCode.VK_M},
            {48,VirtualKeyCode.VK_A},
            {49,VirtualKeyCode.VK_K},
            {50,VirtualKeyCode.VK_S},
            {51,VirtualKeyCode.VK_L},
            {52,VirtualKeyCode.VK_D},
            {53,VirtualKeyCode.VK_F},
            {54,VirtualKeyCode.OEM_1},
            {55,VirtualKeyCode.VK_G},
            {56,VirtualKeyCode.NUMPAD2},
            {57,VirtualKeyCode.VK_H},
            {58,VirtualKeyCode.NUMPAD3},
            {59,VirtualKeyCode.VK_J},
            {60,VirtualKeyCode.VK_Q},
            {61,VirtualKeyCode.VK_I},
            {62,VirtualKeyCode.VK_W},
            {63,VirtualKeyCode.VK_O},
            {64,VirtualKeyCode.VK_E},
            {65,VirtualKeyCode.VK_R},
            {66,VirtualKeyCode.VK_P},
            {67,VirtualKeyCode.VK_T},
            {68,VirtualKeyCode.NUMPAD5},
            {69,VirtualKeyCode.VK_Y},
            {70,VirtualKeyCode.NUMPAD6},
            {71,VirtualKeyCode.VK_U},
            {72,VirtualKeyCode.VK_1},
            {73,VirtualKeyCode.VK_8},
            {74,VirtualKeyCode.VK_2},
            {75,VirtualKeyCode.VK_9},
            {76,VirtualKeyCode.VK_3},
            {77,VirtualKeyCode.VK_4},
            {78,VirtualKeyCode.VK_0},
            {79,VirtualKeyCode.VK_5},
            {80,VirtualKeyCode.NUMPAD7},
            {81,VirtualKeyCode.VK_6},
            {82,VirtualKeyCode.NUMPAD8},
            {83,VirtualKeyCode.VK_7},
            {84,VirtualKeyCode.F1},
            {85,VirtualKeyCode.F8},
            {86,VirtualKeyCode.F2},
            {87,VirtualKeyCode.F9},
            {88,VirtualKeyCode.F3},
            {89,VirtualKeyCode.F4},
            {90,VirtualKeyCode.F10},
            {91,VirtualKeyCode.F5},
            {92,VirtualKeyCode.DIVIDE},
            {93,VirtualKeyCode.F6},
            {94,VirtualKeyCode.MULTIPLY},
            {95,VirtualKeyCode.F7},
        };

    }
}
